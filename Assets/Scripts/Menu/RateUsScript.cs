using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class RateUsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        bool rated = PlayerPrefs.GetString("rated", "false").Equals("true");
        if (rated)
        {
            this.gameObject.SetActive(false);
            err += "start-disable\r\n";
        } else
        {
            err += "start-enable\r\n";
        }

	}
	
    private string err = "";
	// Update is called once per frame
	void Update () {
        var touchedColl = TouchUtility.GetTouchedCollider();
        if (touchedColl != null && touchedColl == collider2D)
        {
            err += "clicked\r\n";
            try
            {
                AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject rateUri = uriClass.CallStatic<AndroidJavaObject>("parse", "bazaar://details?id=com.chocolate.wm");
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
                intent.Call<AndroidJavaObject>("setData", rateUri);
                intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_EDIT"));
                intent.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");
                currentActivity.Call("startActivity", intent);

                GameState.ChangeStoreCoins(GameState.GetStoreCoins() + 500);
                PlayerPrefs.SetString("rated", "true");
                PlayerPrefs.Save();

                if (GameState.AudioFx)
                    audio.Play();
                gameObject.SetActive(false);
            } catch (Exception ex)
            {
                err += ex.Message + "\r\n";
            }
        }
    }

    /*
    void OnGUI(){
        GUI.color = Color.black;
        GUI.contentColor = Color.black;
        GUI.skin.label.fontSize = 24;
        
        GUI.Label(new Rect(20, 20, 300, 400), err);
    }*/
}
