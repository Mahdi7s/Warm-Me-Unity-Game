using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum ButtonType
{
    None,
    About,
    Close,
    CloseStore,
    Facebook,
    MuteFx,
    MuteMusic,
    NextLevel,
    Play,
    Pause,
    Resume,
    Settings,
    Home,
    Quit,
    Refresh,
    Store,
    Twitter,
    NextChapter,
    SetTriggerOnSpecifiedAnimator,
    GoToChapter1,
    StoreBackButton,
    HelpResume,
    Levels
}

public class ButtonScript : MonoBehaviour
{
    public static bool isGoneToTwitter;
    public static bool isGoneToFacebook;
    public AudioClip hitSound;
    [Tooltip("For Close Store Button")]
    public Animator parentPanel;
    public StoreManager storeManager;

    public ButtonType buttonType = ButtonType.None;
    public ParticleSystem ExplodeParticle = null;

    public float secondsToNormalState = 1.0f;
    public Animator backgroundFade;
    public Animator specifiedAnimator;
    public string triggerName;
    public Sprite[] helpSprites;
    public SpriteRenderer helpSpriteRenderer;

    public bool RefreshMoneyBack = false;

    private Animator anim = null;
    private bool doClick = false;
    private int helpResumeCounter;

    void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audio.clip = hitSound;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        if (backgroundFade != null)
        {
            GameState.backgroundFade = backgroundFade;
        }
    }

    void OnApplicationPause()
    {
        GameState.Pause(true);
    }

    private string textErr = "";
    void Update()
    {
        try{
        audio.mute = !GameState.AudioFx;
        if (buttonType == ButtonType.MuteMusic)
        {
            if (GameState.AudioMusic)
            {
                anim.SetTrigger("On");
                anim.ResetTrigger("Off");
            } else
            {
                anim.SetTrigger("Off");
                anim.ResetTrigger("On");
            }

        } else if (buttonType == ButtonType.MuteFx)
        {
            if (GameState.AudioFx)
            {
                anim.SetTrigger("On");
                anim.ResetTrigger("Off");
            } else
            {
                anim.SetTrigger("Off");
                anim.ResetTrigger("On");
            }
        } else
        {
            secondsToNormalState -= Time.deltaTime;
            if (secondsToNormalState <= 0)
            {
                anim.SetBool("Normal", true);
            }
        }
        var touchedColl = TouchUtility.GetTouchedCollider();
        if (touchedColl != null)
        {
            if (touchedColl == collider2D && !doClick)
            {
                if (ExplodeParticle != null)
                {
                    ExplodeParticle.renderer.sortingLayerName = renderer.sortingLayerName;
                    ExplodeParticle.renderer.sortingOrder = renderer.sortingOrder;
                    ExplodeParticle.Play();
                }
                doClick = true;
                anim.SetTrigger("Click");
                audio.Play();
            }
        }

        var state = anim.GetCurrentAnimatorStateInfo(0);
        if (doClick && (state.IsName("End")
            || buttonType == ButtonType.MuteFx || buttonType == ButtonType.MuteMusic
            || buttonType == ButtonType.Resume || buttonType == ButtonType.Pause || buttonType == ButtonType.Settings))
        {
            switch (buttonType)
            {
                case ButtonType.Refresh:
                    if (RefreshMoneyBack)
                    {
                        GameState.ChangeStoreCoins(GameState.GetStoreCoins() + GameState.CurrentLevelPurchases.Sum(x => x.Key.GetItemCoinPrice() * x.Value));
                    }
                    GameState.ChangeTemperature(-1 * GameState.GetTemperature());
                    GameState.RefreshLevel();
                    break;
                case ButtonType.Play:
                    GameState.LoadMenu("Chapters");
                    break;
                case ButtonType.Pause:
                    GameState.Pause(true);
                    break;
                case ButtonType.Resume:
                    GameState.Resume(true);
                    break;
                case ButtonType.Home:
                    GameState.LoadMenu("Main");
                    break;
                case ButtonType.Levels:
                    GameState.LoadMenu("Levels" + GameState.CurrentChapter.ToString());
                    break;
                case ButtonType.Quit:
                    notifyJoorchin();
                    Application.Quit();
                    break;
                case ButtonType.Facebook:
                    if (!isGoneToFacebook && ConnectionUtility.IsConnectedToInternet())
                    {
                        GameState.ChangeStoreCoins(GameState.GetStoreCoins() + 3);
                        isGoneToFacebook = true;
                    }
                    Application.OpenURL("https://www.facebook.com/choc01ate");
                    break;
                case ButtonType.Twitter:
                    if (!isGoneToTwitter && ConnectionUtility.IsConnectedToInternet())
                    {
                        GameState.ChangeStoreCoins(GameState.GetStoreCoins() + 3);
                        isGoneToTwitter = true;
                    }
                        Application.OpenURL("http://instagram.com/shuculat");
                    break;
                case ButtonType.MuteFx:
                    GameState.AudioFx = !GameState.AudioFx;
                    break;
                case ButtonType.MuteMusic:
                    GameState.AudioMusic = !GameState.AudioMusic;
                    break;
                case ButtonType.NextLevel:
                    if (GameState.Level < GameSettings.maxLevelsPerChapter)
                    {
                        GameState.Level++;
                        GameState.LoadMenu("Levels" + GameState.Chapter.ToString());
                    } else if (GameState.Chapter <= GameSettings.maxNumberOfChapters)
                    {
                        GameState.Level = 1;
                        GameState.LoadMenu("EndChapter");
                    }
                    break;
                case ButtonType.CloseStore:
                    parentPanel.SetTrigger("Hide");
                    storeManager.BtnPlay.SetActive(true);
                    storeManager.BtnBack.SetActive(true);
                    storeManager.EnableAllColliders();
                    break;
                case ButtonType.NextChapter:
                    GameState.Chapter++;
                    if (GameState.Chapter < GameSettings.maxNumberOfChapters)
                    {
                        GameState.LoadMenu("Levels" + GameState.Chapter.ToString());
                    } else
                    {
                        // این بلاک منوی انتخاب فصلها را نشان داده و فصل پیش فرض آن را فصل Coming Soon میگذارد.
                        SlideScript.defaultChapterNumber = GameSettings.maxNumberOfChapters + 2;
                        GameState.Chapter = GameSettings.maxNumberOfChapters;
                        GameState.LoadMenu("Chapters");

                    }
                    break;
                case ButtonType.SetTriggerOnSpecifiedAnimator:
                    specifiedAnimator.SetTrigger(triggerName);
                    break;
                case ButtonType.GoToChapter1:
                    GameState.OpenLevel(1, 1);
                    break;
                case ButtonType.StoreBackButton:
                    storeManager.ResetStorePurchases();
                    goto case ButtonType.Levels;
                    break;
                case ButtonType.HelpResume:
                    helpResumeCounter++;
                    helpResumeCounter %= helpSprites.Length;
                    helpSpriteRenderer.sprite = helpSprites [helpResumeCounter];
                    break;
            }
            doClick = false;
        }
        }
        catch(System.Exception ex){
            textErr += ex.Message + "\r\n";
        }
    }

    private void notifyJoorchin(){
        if (!PlayerPrefs.GetString("joorchin", "false").Equals("true"))
        {
            try
            {
                AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject rateUri = uriClass.CallStatic<AndroidJavaObject>("parse", "bazaar://details?id=com.chocolate.puzzlefriends");
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_VIEW"), rateUri);
                intent.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");

                NotificationManager manager = NotificationManager.GetNotificationManager();
                NotificationCompat.Builder builder = new NotificationCompat.Builder();
        
                builder.SetContentTitle("جورچینی متفاوت...");
                builder.SetContentText("با <<بفرمایید جورچین>> دوستان خود را به چالش بکشید");
                builder.SetSmallIcon();
                builder.SetContentIntent(intent);
                builder.SetDefaults(Notification.Default.Sound | Notification.Default.Vibrate);
                manager.Notify(0, builder.Build());

                PlayerPrefs.SetString("joorchin", "true");
                PlayerPrefs.Save();
            } catch(Exception ex)
            {
                errrrr += ex.Message + "\r\n";
            }
        }
    }

    private string errrrr="";
    /*
    void OnGUI(){
            GUI.color = Color.black;
            GUI.contentColor = Color.black;
            GUI.skin.label.fontSize = 24;
        
            GUI.Label(new Rect(20, 20, 300, 400), errrrr);
    }*/
}