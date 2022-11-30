using UnityEngine;
using System.Collections;

public class AdadPlugin : MonoBehaviour
{
    public bool DestroyOnStart = false;
    #if UNITY_ANDROID
    // Use this for initialization
    void Start()
    {
        if (DestroyOnStart)
        {
            CreateBannerView();
        }
        else
        {
            DestroyBanner();
        }
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    public static void DestroyBanner()
    {
        AndroidJavaClass pluginClass = new AndroidJavaClass("ir.adad.AdadUnityPlugin");
        pluginClass.CallStatic("setDisabled", new object[1] { "true" });
    }

    public static void CreateBannerView()
    {
        string verticalPosition = "bottom"; // "top" or "bottom"
        string horizontalPosition = "center"; // "left", "right" or "center"
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass pluginClass = new AndroidJavaClass("ir.adad.AdadUnityPlugin");
        pluginClass.CallStatic("createAdView",
		                       new object[3]
        {
            activity,
            verticalPosition,
            horizontalPosition
        });
    } 
#endif
}