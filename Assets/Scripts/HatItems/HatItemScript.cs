using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HatItemScript : PausableBehaviour
{
    #region Definition
    public Sprite name;
    public GameObject AnimationChild = null;
    public SlothScript sloth;
    public AnimatorSpeedChanger animatorSpeedChanger = null;
    public List<ParticleSystem> SpecialParticles = null;
    public List<Transform> SpecialChildren = null;
    [Header("Items Settings")]
    public HatItemKind
        Kind = HatItemKind.None;
    [Range(1, 10)]
    public int
        ShowFromChapter = 1;
    [Range(0, 10)]
    [Tooltip("the priority of free(non-purchasable) items, set bigger than zero for showing")]
    public int Priority = 0;
    [Range(0, 1000)]
    // change this in future ****
    public int Score = 0;
    internal int hatNo;
    [Range(0, 100)]
    internal int ShowingPercent = 0;
    [Range(-80, 80)]
    public int Temperature = 0;
    [Range(0, 100)]
    [Tooltip("set non-zero just for ciggarets")]
    public int ExplosionPower = 0;
    public bool PlayInAnimAfterSpecial = true;
    public int[] eggBronzeScores = new[] { 0, 5, 10, 15, 20 };
    [Tooltip("Deley to show eggs score. Works for bronze and golden eggs.")]
    public float eggsReactionDelay;
    public HatScript hat;
    public HatItemsContainerScript hatItemsContainerScript;
    public GUINumberScript guiScript;
    public QuoteController quoteController;
    public GameObject LettersParent = null;
    public SpriteRenderer LetterSprite = null;
    [Tooltip("This variable will be filled programmatically.")]

    protected Animator _anim = null;
    protected bool _playSpecial = false;

    protected bool shouldAutomaticallyHit = false;
    protected int reaction = 0;
    protected float deltaTime;
    protected float elapsedTime;

    private string hatName;
    private float _inStateSeconds = 0.0f;
    internal ChapterLevelScript ChapterLvlScr = null;

    #endregion

    public int LetterIndex { set; private get; }

    public float HitProbabilityPercent
    {
        get
        {
            return ShowingPercent * 0.8f;
        }
    }

    public int goodRate { get; private set; }

    public int badRate
    {
        get;
        private set;
    }

    //------------------------------------- Privates ------------------------------
    public virtual GameObject InstantiateNew(HatScript hatScr, float inStateSeconds)
    {
        if (hatScr != null && !hatScr.IsFalling())      // if hat is ready
        {
            if (Kind.IsPositiveItem())
            {
                ++ChapterLvlScr.PositiveItemsCount;
            }
            hat = hatScr;
            hatNo = hatScr.HatNo;
            var retval = (GameObject)GameObject.Instantiate(this.gameObject);
            // اگر کد زیر نباشد باعث دبده شدن آیتم‌ها پشت کلاه می‌شود.
            retval.transform.localScale = Vector3.zero;
            if (animatorSpeedChanger != null)
            {
                // کد زیر باعث تغییر سرعت تمامی آیتم‌ها به محض ایجاد شدن می‌گردد.
                AnimatorSpeedChanger.ChangeAnimatorSpeedInAllChildren(retval.transform, animatorSpeedChanger.speed);
            }
            retval.transform.parent = hatScr.ItemTransform;
            retval.transform.rotation = hatScr.gameObject.transform.rotation;   // آیتم ها را متناسب با چرخش کلاه می چرخاند.
            retval.transform.localPosition = Vector3.zero;
            if (Kind == HatItemKind.MatchBlue || Kind == HatItemKind.MatchRed || Kind == HatItemKind.MatchGreen || Kind == HatItemKind.MatchPink || Kind == HatItemKind.MatchYellow)
            {
                gameObject.transform.GetChild(0).transform.rotation = Quaternion.identity;// چون آتش همیشه به سمت بالا شعله ور می‌شود، پارتیکل مربوط به آتش را به موقعیت اول بر میگرداند.
            }
            var scr = retval.GetComponent<HatItemScript>();
            scr._inStateSeconds = inStateSeconds;
            scr.hatNo = hatNo;
            return retval;
        }
        return null;
    }

    protected virtual void Start()
    {
        if (ChapterLvlScr == null)
        {
            ChapterLvlScr = Camera.main.GetComponent<ChapterLevelScript>();
        }
        elapsedTime = 0.5f/animatorSpeedChanger.speed;// این زمان تاخیر لازم برای ترکیدن تخم مرغ و انفجار سیگارت را ایجاد میکند.
        if (Kind == HatItemKind.EggBronze)
        {
            Score = 0;
            guiScript.Hide();
        }

        _anim = AnimationChild.GetComponent<Animator>();
        _inStateSeconds = GameState.itemNormalSeconds;
    }

    protected override void POnGUI()
    {
    }

    protected override void PFixedUpdate()
    {
    }

    protected override void PUpdate()
    {
        audio.mute = !GameState.AudioFx;
        audio.pitch = Time.timeScale;
        deltaTime = Time.deltaTime;
        shouldAutomaticallyHit = hat.BoxOfMatches && !_anim.GetBool("Special");
        var state = _anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Normal"))
        {
            if ((_inStateSeconds -= deltaTime) <= 0)
            {
                _anim.SetBool("Out", false);
                _anim.SetBool("In", true);
            }
            if (shouldAutomaticallyHit)    // اگر آیتم باید به طور اتوماتیک زده شود و هنوز زده نشده است
            {
                switch (Kind)
                {
                    case HatItemKind.MatchBlue:
                    case HatItemKind.MatchGreen:
                    case HatItemKind.MatchPink:
                    case HatItemKind.MatchRed:
                    case HatItemKind.MatchYellow:
                        hat.MatchBoxRunner.Fire();
                        Hit();
                        shouldAutomaticallyHit = false;
                        break;
                }
            }
        }
        if (_playSpecial)
        {
            if (!_anim.GetBool("Special"))
            {
                if (SpecialParticles.Any())
                {
                    SpecialParticles.ForEach(x => x.Play());
                }
                if (SpecialChildren.Any())
                {
                    SpecialChildren.ForEach(x => x.gameObject.SetActive(true));
                }
                _anim.SetBool("Special", true);
                audio.Play();
                _playSpecial = false;
            }
        }
        if (reaction > 0)                           // if any thing should react
        {
            if (_anim.GetBool("Special"))           // if the item has been hitted. | can be removed, because reaction value shows that item has been already hitted
            {
                elapsedTime -= deltaTime;           // decrease the delay time
                if (elapsedTime <= 0.0f)            // if the delay has been passed
                {
                    switch (reaction)
                    {
                        case 1:                     // if item was one of cigarette types
                            foreach (PipeScript pipeScript in GameState.PipeScripts)
                            {
                                // Send the parameters to pipes to react appropriately.
                                pipeScript.PipeReaction(hatNo, Temperature, ExplosionPower);
                            }
                            sloth.Upsidedown();
                            reaction = 0;           // Resets the reaction
                            break;
                        case 2:                     // if item was an egg, bronze egg actually
                            eggsReactionDelay -= deltaTime;     // decrease the delay time | if the egg score does not show, remove this delay.
                            if (eggsReactionDelay < 0.0f)       // if delay has been passed
                            {
                                guiScript.Number = eggBronzeScores[Random.Range(0, eggBronzeScores.Length)];
                                //guiScript.Show();
                                hat.EnqueueCoin(guiScript.Number);          // Send the score to show by hat.
                                GameState.LevelCoins += (guiScript.Number * GameState.Combo);
                                reaction = 0;       // Resets the reaction
                            }
                            break;
                    }
                }
            }
        }
    }

    public virtual void Hit()
    {
        if (!_playSpecial)
        {
            _playSpecial = true;
            if (Kind.IsPositiveItem())
            {
                ++ChapterLvlScr.HitPositiveCount; // counting the positive hits
            }
            if (Kind == HatItemKind.CigaretteBlue || Kind == HatItemKind.CigaretteGreen || Kind == HatItemKind.CigaretteYellow)
            {
                reaction = 1;       // it will determine that the item is one of cigarette types
            }
            else if (Kind == HatItemKind.EggBronze)
            {
                reaction = 2;       // it will determine that the item is an egg, bronze egg actually
            }
            else if (Kind == HatItemKind.Letter)
            {
                quoteController.FillCharacter(LetterIndex);
                ++ChapterLvlScr.HittedLetters;
            }
            else
            {
                // Send the parameters to pipes to react appropriately.
                foreach (PipeScript pipeScript in GameState.PipeScripts)
                {
                    pipeScript.PipeReaction(hatNo, Temperature, ExplosionPower);
                }
            }
            hatItemsContainerScript.RecordItem(Kind.IsPositiveItem()); // record this item to show praise anim
            GameState.LevelCoins += (Score * GameState.Combo);
            GameState.ChangeTemperature(Temperature);
        }
    }
}