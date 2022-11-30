using UnityEngine;
using System.Collections;

public class AdMobManager : MonoBehaviour
{
   #if UNITY_ANDROID
    private const string adsPublishId = "pub-2500743392819794";
    private const string androidInterstitialUnitId = "ca-app-pub-2500743392819794/1949261267";
    private const string iosInterstitialUnitId = "ca-app-pub-2500743392819794/1949261267";

    public AdMobBanner BannerType = AdMobBanner.SmartBanner;
    public float ShowTime = 30.0f;
    public Font font;
    public Color color;
    public int size;
    public Rect rectangle;
    //public string err;

    public bool HideOnStart = false;

    private AdMobLocation adLocation = AdMobLocation.BottomCenter;
    private float showStartTime = 0.0f;
    private float timer = 0.0f;
    private static bool bannerShowed = false;

    // Use this for initialization
    void Start()
    {
        if (GameState.AdMobEnabled)
        {
            if (!HideOnStart)
            {
                Init();
                //err = "Hello\n";
            } else
            {
                HideBanner();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.AdMobEnabled)
        {
            if ((!bannerShowed) && ((timer += Time.deltaTime) >= showStartTime))
            {
                timer = 0.0f;
                bannerShowed = true;
                ShowBanner();
            } else if (bannerShowed && (timer += Time.deltaTime) >= ShowTime)
            {
                //HideBanner();
            }
        }
    }

    /*void OnGUI()
    {
        //#if UNITY_EDITOR
        //#if UNITY_ANDROID
        GUI.skin.font = font;
        GUI.skin.label.fontSize = size;
        GUI.contentColor = color;
        GUI.Label(rectangle, err);
        //
    }*/

    void OnEnable()
    {
        AdMob.receivedAdEvent += receivedAdEvent;
        AdMob.failedToReceiveAdEvent += failedToReceiveAdEvent;
        AdMob.interstitialReceivedAdEvent += interstitialReceivedAdEvent;
        AdMob.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEvent;
    }

    void OnDisable()
    {
        AdMob.receivedAdEvent -= receivedAdEvent;
        AdMob.failedToReceiveAdEvent -= failedToReceiveAdEvent;
        AdMob.interstitialReceivedAdEvent -= interstitialReceivedAdEvent;
        AdMob.interstitialFailedToReceiveAdEvent -= interstitialFailedToReceiveAdEvent;
    }

    // -------------------- Helpers --------------------

    private void Init()
    {
        if (GameSettings.publishDestination == PublishDestination.GooglePlayAndAppStore)
        {
            AdMob.init(adsPublishId, adsPublishId/*"ios public key"*/);
            AdMob.setTestDevices(new[] { SystemInfo.deviceUniqueIdentifier });
        }

        AdMobLocation[] adLocs = { AdMobLocation.TopCenter, AdMobLocation.BottomCenter, AdMobLocation.BottomLeft, AdMobLocation.TopLeft };
        adLocation = adLocs[Random.Range(0, adLocs.Length - 1)];
    }

    private void ShowBanner()
    {
        //try
        {
            if (GameSettings.publishDestination == PublishDestination.GooglePlayAndAppStore)
            {
                AdMob.createBanner(BannerType, adLocation);
            }
            else if (GameSettings.publishDestination == PublishDestination.Bazaar)
            {
                CreateBannerView();
            }
        }
        /*catch(UnityException e)
        {
            err+=e.ToString();
        }
        catch(UnassignedReferenceException e)
        {
            err+= e.ToString();
        }*/
    }

    private void HideBanner()
    {
        if (GameSettings.publishDestination == PublishDestination.Bazaar)
        {
            AdadPlugin.DestroyBanner();
        }
        else
        {
            AdMob.destroyBanner();   
        }             
    }

    private void CreateBannerView()
    {
        string verticalPosition = "bottom"; // "top" or "bottom"
        string horizontalPosition = "center"; // "left", "right" or "center"

        switch (adLocation)
        {
            case AdMobLocation.BottomLeft:
                horizontalPosition = "left";
                break;
            case AdMobLocation.BottomRight:
                horizontalPosition = "right";
                break;
            case AdMobLocation.TopCenter:
			verticalPosition = "bottom";
                break;
            case AdMobLocation.TopLeft:
			verticalPosition = "bottom";
                horizontalPosition = "left";
                break;
            case AdMobLocation.TopRight:
			verticalPosition = "bottom";
                horizontalPosition = "right";
                break;
        }

        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        /*if(playerClass!=null)
        {err+="Player class constructed.\n";}*/
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        /*if(activity!=null)
        {err+="currentActivity constructed.\n";}*/
        AndroidJavaClass pluginClass = new AndroidJavaClass("ir.adad.AdadUnityPlugin");
        /*if(pluginClass!=null)
        {err+="pluginClass constructed.\n";}
        else
        {
            err+="fucked Up\n";
        }*/
        pluginClass.CallStatic("createAdView", new object[3]
            {
                activity,
                verticalPosition,
                horizontalPosition
            });
    }

    private void RequestInterstital()
    {
        AdMob.requestInterstital(androidInterstitialUnitId, iosInterstitialUnitId);
    }

    private void DestroyBanner()
    {
        AdMob.destroyBanner();
    }

    private void receivedAdEvent()
    {
//#if UNITY_EDITOR
        Debug.Log("receivedAdEvent");
//#elif UNITY_ANDROID
            //err+="receivedAdEvent";
//#endif
    }


    private void failedToReceiveAdEvent(string error)
    {
//#if UNITY_EDITOR
        Debug.Log("failedToReceiveAdEvent: " + error);
//#elif UNITY_ANDROID
            //err+="failedToReceiveAdEvent: " + error;
//#endif
    }


    private void interstitialReceivedAdEvent()
    {
//#if UNITY_EDITOR
        Debug.Log("interstitialReceivedAdEvent");
//#elif UNITY_ANDROID
            //err+="interstitialReceivedAdEvent";
//#endif
    }


    private void interstitialFailedToReceiveAdEvent(string error)
    {
//#if UNITY_EDITOR
        Debug.Log("interstitialFailedToReceiveAdEvent: " + error);
//#elif UNITY_ANDROID
            //err+="interstitialFailedToReceiveAdEvent: " + error;
//#endif
    }

#endif
}
