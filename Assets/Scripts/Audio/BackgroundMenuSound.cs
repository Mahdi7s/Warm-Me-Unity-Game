using UnityEngine;
using System.Collections;

// This script will use to play menu sound on diffrent menu scenes.
public class BackgroundMenuSound : MonoBehaviour 
{
    private static BackgroundMenuSound instance = null;
	void Awake ()
    {
        if (instance != null && instance != this) // if any instance of this script is already exists
        {
            Destroy(gameObject);                  // Destroy game object of this script.
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform);         // Make this game object persistent.
        }
	}
	
	void Update ()
    {
        audio.mute = !GameState.AudioMusic;
	}

    void OnDestroy()                             // This function will be called when the game object is being destroyed.
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public static BackgroundMenuSound Instance
    {
        get 
        {
            return instance;
        }
    }
}
