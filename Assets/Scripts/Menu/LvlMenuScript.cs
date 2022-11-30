using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class LvlMenuScript : MonoBehaviour
{
    private int _chapter = 0;
    private int _level = 0;
    private bool isActive;
    private LevelsMenuScript levelsMenuScript;

    public int Chapter { get { return _chapter; } }
    public int Level { get { return _level; } }
    public bool activeStoryBoard;
    public string ElementNamePref = string.Empty;
    public Sprite openMelon;
    //public GUINumberScript Coins = null;  

    void Awake()
    {
        var chapterLevel = name.Replace(ElementNamePref, "").Split('-');
        _chapter = int.Parse(chapterLevel [0]);
        _level = int.Parse(chapterLevel [1]);
        levelsMenuScript = GetComponentInParent<LevelsMenuScript>();
    }

    void Start()
    {
        
    }
	
	
    void Update()
    {
	
    }

    // -------------------- public methods ------------------

    public void Initialize(LevelModel model, bool isActive)
    {
        this.isActive = isActive;
        var coins = model == null ? 0 : model.LevelCoins;
        if (isActive)
        {
            GetComponent<SpriteRenderer>().sprite = openMelon;
            if(coins > 0){
            var sr = levelsMenuScript.GetMedalSpriteRenderer(model.Level - 1);
            sr.GetComponentInChildren<ParticleSystem>().enableEmission = true;
            var spr = levelsMenuScript.GetSprite(model.LevelCup);
            if(spr != null)
                sr.sprite = spr;
            //Coins.Number = coins;
            }
        }
    }

    public void OnHit()
    {
        if (isActive)
        {
            if (activeStoryBoard)
            {
                if (Chapter == 1 && Level == 1)
                {
                    GameState.LoadMenu("Storyboard");
                }
            }
            else
            {
                /*Debug.Log(Chapter);
                Debug.Log(Level);*/
                GameState.OpenLevel(Chapter, Level);
            }
        }
    }
}