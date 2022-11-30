using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using System.Text;
/// <summary>
/// Chapter level script.
/// Property => HatItems: contaions all kind of hat items, purchasable and free ones
/// </summary>
public class ChapterLevelScript : PausableBehaviour
{
    [Header("Chapters & Levels")]
    [Range(1, GameSettings.maxNumberOfChapters)]
    public int
        Chapter = 1;
    [Range(1, GameSettings.maxLevelsPerChapter)]
    internal int
        Level = 1;
    internal int LevelsCount = 9;
    [Range(0f, 10f)]
    [Tooltip("This option specifies time decrease step per level; it should be balanced with items speed.")]
    public float
        timeDecreaseStep = 2.0f;
    [Header("Periods Settings")]
    [Range(60.0f, 5 * 60.0f)]
    public float
        LevelDurationSecs = 60.0f;
    [Range(5, 25)]
    public int
        LevelPeriodsCount = 10;

    [Range(0.1f, 0.95f)]
    public float
        OutStoreItemsTimePercent = 0.6f;

    [Header("Item Speed Settings")]
    public float
        BaseItemSpeed = 1.0f;
    public float ItemSpeedIncreaseStep = 0.02f;

    [Header("Temprature Settings")]
    public int
        MaxTemprature = 80;
    public int MinWinTemprature { get { return (int)(MaxTemprature * 0.75f); } }

    [Header("Containers")]
    public Transform
        HatsContainer = null;
    public Transform HatItemsContainer = null;

    public int HittedLetters = 0;
    public SandTimeScript timeScript;
    internal List<HatItemScript> HatItems = new List<HatItemScript>();
    private List<HatItemScript> FreeHatItems = new List<HatItemScript>();

    public int PositiveItemsCount = 0;
    public int HitPositiveCount = 0;

    public float WrenchDelay = 7.0f;
    // ---------------------------Privates--------------------------

    private int _freeItemsCount = 0;
    private int _purchaseItemsCount = 0;
    private int _hatsCount = 0;

    private float MinItemNormalSeconds = 0.5f;
    private float MaxItemNormalSeconds = 2.5f;

    internal List<HatScript> Hats = new List<HatScript>();

    private Dictionary<string, int> ItemCount = new Dictionary<string, int>(); // HatItemKind - change the to string so you use both pi and non together
    private Dictionary<float, string> SecondsItem = new Dictionary<float, string>();  // change the to string so you use both pi and non together
    private float _passedSecs = 0.0f;
    public int InstantiatedItems { set; get; }

    private bool _isLevelStarted = false;

    private QuoteController _quoteCtrl = null;
    private string _quote = string.Empty;
    private float speed;
    // -------------------------Protecteds----------------------------
    protected Dictionary<PurchasedItems, bool> lockDic = new Dictionary<PurchasedItems, bool>();
    protected Dictionary<HatItemKind, bool> hatItemsLockDic = new Dictionary<HatItemKind, bool>();


    // -------------------------Methods----------------------------
    protected virtual void Awake()
    {
        if (BackgroundMenuSound.Instance != null)
        {
            Destroy(BackgroundMenuSound.Instance.gameObject);
        }
        LevelsCount = GameSettings.maxLevelsPerChapter;
        Level = GameState.Level;
        speed = BaseItemSpeed + (/*Chapter - 1*/ 0) * 0.075f + (Level - 1) * 0.05f;
        LevelDurationSecs /= Speed;
    }
    protected virtual void Start()
    {
        Awake();
        GetComponent<AnimatorSpeedChanger>().SetSpeed(Speed);
        GameState.itemNormalSeconds = ItemNormalSeconds();
    }
    public int ItemsCount
    {
        set;
        get;
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public virtual void StartLevel()
    {
        _quoteCtrl = FindObjectOfType<QuoteController>();
        _quote = _quoteCtrl.GetQuote();

        Hats = HatsContainer.GetComponentsInChildren<HatScript>().ToList();
        _hatsCount = Hats.Count();

        var itemComponents = HatItemsContainer.GetComponentsInChildren<HatItemScript>();
        HatItems = itemComponents.Where(x => (this.Chapter >= x.ShowFromChapter) && (x.Kind != HatItemKind.None) && (x.Kind == HatItemKind.StoreItem || x.Priority > 0)).OrderBy(x => x.Priority).ToList();
        FreeHatItems = HatItems.Where(x => x.Kind != HatItemKind.StoreItem && x.Kind != HatItemKind.Letter).ToList();

        // the Priority of calling is very important
        CalculateLevelHatItems();
        CalculateLevelItemsCount();
        CalculateStoreItems();
        CalculatePeriodsItems();
        HatItems = HatItems.OrderBy(x => Guid.NewGuid()).ToList();
        ItemsCount = SecondsItem.Count();

        timeScript.Initialize(this);
        _isLevelStarted = true;
    }

    protected override void PFixedUpdate()
    {

    }

    protected override void POnGUI()
    {

    }

    protected override void Update()
    {
        base.Update();
        audio.mute = !GameState.AudioMusic;
    }

    protected override void PUpdate()
    {

        if (_isLevelStarted)
        {
            /*if (_passedSecs >= LevelDurationSecs)
            {
                // end level and show result
            } else
            {*/
                var theKey = SecondsItem.OrderBy(x => x.Key).Select(x => x.Key).FirstOrDefault(x => _passedSecs >= x);

                if (!theKey.Equals(default(float)))
                {
                    var kind = SecondsItem [theKey];
                    var availableHats = Hats.Where(x => x.CanShowNewItem(kind));
                    int letIdx = 0;

                    if (availableHats.Any())
                    {
                        var hat = availableHats.ElementAt(Random.Range(0, availableHats.Count()));
                        var itemObject = HatItems.FirstOrDefault(x =>
                        {
                            if (x.Kind == HatItemKind.StoreItem)
                            {
                                StoreItemScript sotreItem = (StoreItemScript)x;
                                return (ToStoreItemString(sotreItem.piKind) == kind);
                            } else
                            {
                                if (kind.StartsWith("Letter-") && x.Kind == HatItemKind.Letter && !string.IsNullOrEmpty(_quote.Trim()))
                                {
                                    var sbQuot = new StringBuilder(_quote);
                                    letIdx = _quote.IndexOf(kind.Last());
                                    sbQuot [letIdx] = ' ';
                                    _quote = sbQuot.ToString();
                                    sbQuot.Length = 0;

                                    return (_quote.Replace(" ", "").Length >= 0);
                                } else
                                {
                                    return (ToHatItemString(x.Kind) == kind);
                                }
                            }
                        });
                        if (itemObject != null && hat != null)
                        {
                            if (kind.StartsWith("Letter-"))
                            {
                                var haItm = itemObject.GetComponent<HatItemScript>();
                                var sprites = haItm.LettersParent.GetComponentsInChildren<SpriteRenderer>();
                                char letChar = kind.Last();
                                var letterIdx = letChar - (int)'a';
                                for (int i = 0; i < sprites.Length; i++)
                                {
                                    if (letterIdx == i)
                                    {
                                        haItm.LetterSprite.sprite = sprites [i].sprite;
                                    }
                                }
                            }
                            hat.NextItem = itemObject.InstantiateNew(hat, ItemNormalSeconds());
                            ++InstantiatedItems;
                            hat.NextItem.GetComponent<HatItemScript>().LetterIndex = letIdx;

                            SecondsItem.Remove(theKey);
                        } else if (itemObject == null)
                        {
                            Debug.LogError("Kind Null: " + kind);
                        } else
                        {
                            Debug.LogError("Hat Null: ");
                        }
                    } else // no any available hat found !!!
                    {
                        //return;
                    }
                    
                }
                _passedSecs += Time.fixedDeltaTime; // this should be bottom in the case of "if any hat not available"
            //}
        }
    }

    //--------------------------------------------- Protecteds ------------------------------------------------------

    protected virtual void CalculatePeriodsItems() //****
    {
        var periodSecs = LevelDurationSecs / LevelPeriodsCount;
        var periodItemCount = (int)(ItemCount.Sum(x => x.Value) / (LevelPeriodsCount)) + 1;

        if (periodItemCount <= 0)
            Debug.LogError("period item count is zero, what you set wrong(check \"LevelDurationSecs\" & \"LevelPeriodsCount\")?");
        if (LevelPeriodsCount <= 0)
            Debug.LogError("Level Periods count is zero, set it");

        //ItemCount = ItemCount.OrderBy(x => Guid.NewGuid()).ToDictionary(x => x.Key, x => x.Value);
        for (var i = 0; i < LevelPeriodsCount; i++)
        {
            for (var j = 0; j < periodItemCount; j++)
            {
                var availableItems = ItemCount.Where(x => x.Value > 0).Select(x => x.Key);
                var avaArr = availableItems.ToArray();
                if (!availableItems.Any())
                {
                    i = LevelPeriodsCount;
                    break;
                }

                var itemShowSec = Random.Range(periodSecs * i, periodSecs * (i + 1));
                var rndIdx = Random.Range(0, availableItems.Count());
                var itemKey = availableItems.ElementAt(rndIdx);

                SecondsItem [itemShowSec] = itemKey;
                //Debug.Log("item sec: " + itemShowSec.ToString() + ", key: " + itemKey.ToString());
                --ItemCount [itemKey];
            }
        }

        // reordering all items & top ordering store items...
        var toTime = LevelDurationSecs * OutStoreItemsTimePercent;

        var vals = SecondsItem.OrderBy(x => Guid.NewGuid()).Select(x => x.Value).ToArray();
        for (int i = 0; i < SecondsItem.Count(); i++)
        {
            var secKey = SecondsItem.Keys.ElementAt(i);
            if (vals[i].StartsWith(StoreItemKindPref) && secKey > toTime) // top to list , store item
            {
                var nSecKey = secKey - toTime / 2;
                while (SecondsItem.ContainsKey(nSecKey))
                {
                    nSecKey += 0.1f;
                }
                SecondsItem.Remove(secKey);
                SecondsItem[nSecKey] = vals[i];
                
                //Debug.LogError("Seems we have trouble...");
            }
            else
            {
                SecondsItem[secKey] = vals[i];
            }
        }

        // setting wrenches based on cigarettes...
        var wrenches = SecondsItem.Where(x => x.Value == ToStoreItemString(PurchasedItems.Wrench));
        var wrenchCount = wrenches.Count();
        Debug.Log("wrenchCount= " + wrenchCount.ToString());
        var cigarettes = SecondsItem.Where(x => x.Value.Contains("Cigarette"));
        var cigaretteCount = cigarettes.Count();
        Debug.Log("cigaretteCount= " + cigaretteCount.ToString());
        if (wrenchCount > 0)
        {
            var oneWrenchForCigarettes = (int)(cigaretteCount / wrenchCount);

            for (int i = 0, j = 0; i < cigaretteCount && j<wrenchCount; i += oneWrenchForCigarettes, j++)
            {
                var cig = cigarettes.ElementAt(i);
                var wrench = wrenches.ElementAt(j);

                var wrenchNewTime = cig.Key + WrenchDelay;                
                while (SecondsItem.ContainsKey(wrenchNewTime))
                {
                    wrenchNewTime += 0.01f;
                }
                SecondsItem.Remove(wrench.Key);
                SecondsItem [wrenchNewTime] = wrench.Value;
            }
        }
    }

    /// <summary>
    /// before we go here we should insert the store items to the container.
    /// </summary>
    protected virtual void CalculateLevelHatItems()
    {
        //var PrioritySum0 = FreeHatItems.Sum(x => x.Priority);
        //var PrioritySum = FreeHatItems.Distinct().Sum(x => x.Priority);
        var maxPrio = FreeHatItems.Max(x => x.Priority);
        var PrioritySum = FreeHatItems.Sum(x => maxPrio - (x.Priority - 1));
        var hatItemsCount = FreeHatItems.Count();
        for (var i = 0; i < hatItemsCount; i++)
        {
            var showingPercent = LevelItemPercent(PrioritySum, maxPrio - (FreeHatItems [i].Priority - 1));
            FreeHatItems [i].ShowingPercent = showingPercent;
        }
    }

    protected virtual void CalculateLevelItemsCount()
    {
        // more than one item can be shown in same time ******
        // random between empty hats for showing next item
        // zigma(item hit probability * item temprature)+zigma(item count) = minWinTemprature

        var lvlItemSpeed = Speed;
        /*
        if (lvlItemSpeed <= 0)
            Debug.LogError("each item speed is zero");
        else
            Debug.Log("each item speed: " + lvlItemSpeed);
        */
        var allItemsCount = (int)(LevelDurationSecs * _hatsCount / 4.0f / lvlItemSpeed);//(MinWinTemprature - (int)FreeHatItems.Sum(x => x.Temperature * lvlItemSpeed * (x.HitProbabilityPercent / 100))) * _hatsCount / 2;
        /*
        if (allItemsCount <= 0)
            Debug.LogError("level all items count is zero");
        else
            Debug.Log("level's all items count: " + allItemsCount);
        */
        string logItems = "\t*****Items Count*****\r\nKind\t\t\t\t\t\tPriority\t\t\t\t\t\tShowing Percent\t\t\t\t\t\tCount\r\n";
        foreach (var item in FreeHatItems)
        {
            var kind = ToHatItemString(item.Kind);
            var count = (int)allItemsCount * item.ShowingPercent / 100;
            ItemCount [kind] = count;

            logItems += kind + "\t\t\t\t\t\t" + item.Priority + "\t\t\t\t\t\t" + item.ShowingPercent + "\t\t\t\t\t\t" + count + "\r\n";
        }
        //Debug.Log(logItems);

        _freeItemsCount = allItemsCount;
    }

    protected virtual void CalculateStoreItems()
    {
        if (GameState.CurrentLevelPurchases != null && GameState.CurrentLevelPurchases.Any())
        {
            _purchaseItemsCount = GameState.CurrentLevelPurchases.Sum(x => x.Value);

            foreach (HatItemScript freeItem in FreeHatItems)
            {
                var key = ToHatItemString(freeItem.Kind);
                var count = ItemCount [key];
                count -= (count * _purchaseItemsCount / _freeItemsCount);

                ItemCount [key] = count;
            }

            var mouseTrapObj = HatItems.OfType<StoreItemScript>().First(x => x.piKind == PurchasedItems.MouseTrap);
            foreach (var piItem in GameState.CurrentLevelPurchases.Keys)
            {
                if (piItem == PurchasedItems.MouseTrap)
                {
                    for (var i = 0; i < GameState.CurrentLevelPurchases[piItem]; i++)
                    { 
                        var mouseTrap = (GameObject)GameObject.Instantiate(mouseTrapObj.gameObject);
                        mouseTrap.GetComponent<StoreItemScript>().ChapterLvlScr = this;
                        mouseTrap.SendMessage("Hit");
                    }
                } else
                {
                    ItemCount [ToStoreItemString(piItem)] = GameState.CurrentLevelPurchases [piItem];
                }
            }
        }

        var letterItemPref = HatItems.Single(x => x.Kind == HatItemKind.Letter);
        foreach (var ch in _quote)
        {
            var letKey = ToLetterItemString(ch.ToString());
            if (ItemCount.ContainsKey(letKey))
            {
                ++ItemCount [letKey];
            } else
            {
                ItemCount [letKey] = 1;
                //var gobj = (GameObject)GameObject.Instantiate(letterItemPref);
                //gobj.GetComponent<HatItemScript>().LetterScript.str = ch.ToString();
            }
        }
    }

    public void HatIsFalling()
    {
        var prevItems = SecondsItem.Count();

        var checkDict = new Dictionary<string, bool>
		{ 
			{ToHatItemString (HatItemKind.MatchGreen), false}, 
			{ ToHatItemString (HatItemKind.MatchYellow), false},
			{	ToHatItemString (HatItemKind.MatchBlue), false},
			{	ToHatItemString (HatItemKind.IceYellow), false},
			{ ToHatItemString (HatItemKind.CigaretteYellow), false}
		};
        int removedCount = 0;

        var keys = SecondsItem.OrderByDescending(x => x.Key).Select(x => x.Key);
        foreach (var key in keys)
        {
            var itmVal = SecondsItem[key];
            bool removed;
            if (checkDict.TryGetValue(itmVal, out removed))
            {
                if (!removed)
                {
                    //++InstantiatedItems;
                    SecondsItem.Remove(key);

                    checkDict[itmVal] = true;
                    ++removedCount;
                }
            }

            if (removedCount == checkDict.Count())
                break;
        }
        ItemsCount -= removedCount;
        Debug.Log("Before fall: " + prevItems + " - After: " + SecondsItem.Count());
    }

    // -------------------------------------------------------------------------------------------------------

    protected virtual int LevelItemPercent(int PrioritySum, int Priority)
    {
        return (int)((Priority * 100) / PrioritySum); //use chapter & level ****
    }

    public virtual float ItemNormalSeconds()
    {
        return ((MaxItemNormalSeconds - MinItemNormalSeconds) / LevelsCount) * (LevelsCount - Level + 1);
    }

    //------------------------------------------------ Helpers -----------------------------------------------

    public const string HatItemKindPref = "(HI)-";
    public const string StoreItemKindPref = "(SI)-";

    private bool IsStoreItem(string kindStr)
    {
        return kindStr.StartsWith(kindStr);
    }

    private HatItemKind ToHatItem(string kindStr)
    {
        return (HatItemKind)Enum.Parse(typeof(HatItemKind), kindStr.Replace(HatItemKindPref, string.Empty));
    }

    private PurchasedItems ToStoreItem(string kindStr)
    {
        return (PurchasedItems)Enum.Parse(typeof(PurchasedItems), kindStr.Replace(StoreItemKindPref, string.Empty));
    }

    private string ToHatItemString(HatItemKind kind)
    {
        if (kind == HatItemKind.StoreItem)
            Debug.LogError("ToHatItemString, don't use this method for storeItems");

        return HatItemKindPref + kind.ToString();
    }

    private string ToLetterItemString(string letter)
    {
        return "Letter-" + letter;
    }

    private string ToStoreItemString(PurchasedItems kind)
    {
        return StoreItemKindPref + kind.ToString();
    }

    public bool GetLockStatus(PurchasedItems purchasedItem)
    {
        return lockDic [purchasedItem];
    }

    public bool GetLockStatus(HatItemKind hatItemKind)
    {
        return hatItemsLockDic [hatItemKind];
    }

    public bool IsWordCompleted()
    {
        return _quote.Length == HittedLetters;
    }
}