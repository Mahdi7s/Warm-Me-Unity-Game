using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class CaptureAndActiveButtonsScript : MonoBehaviour
{
    [Tooltip("Delay after playing last medal animation.")]
    public float delay;
    [Tooltip("Last Medal Script: Select last medal in the scene.\nShare Message Script: Select share message game object, which has a Generic Animation Delay Script attached.")]
    public GenericAnimationDelayScript lastMedalScript, shareMessageScript;
    public Transform buttonsContainer;
    private bool captured, shouldContinue;
    private Texture2D capture;
    private string capturePath;

    void Start()
    {
        shareMessageScript.animationDelay = 5000f;          // Do not play share message animation
    }

    void Update()
    {
        audio.mute = !GameState.AudioMusic;
        if (shouldContinue)
        {
            delay -= Time.deltaTime;            // Delay after playing last medal animation
            if (delay <= 0.2f)                  // if last animation has been played compeletely
            {
                if (captured)                   // if the snapshot has been already taken
                {
                    if (delay <= 0.0f)          // if the delay has been passed
                    {
                        buttonsContainer.position = Vector3.zero;   // show buttons
                        shareMessageScript.animationDelay = 0.0f;   // show share message animation
                        shouldContinue = false;                     // Do not continue
                        lastMedalScript.animationDelay = 0.01f;     // This makes shouldContinue variable remain false.
                    }
                }
                else                            // if the snapshot has not been taken yet
                {
                    capture = new Texture2D(Screen.width, Screen.height);                   // facebook combo will use this capture
                    capture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                    capture.Apply();
                    captured = true;
                    capturePath = Application.persistentDataPath + "/" + DateTime.Now.Ticks + ".png";   // twitter combo will use this capture path
                    File.WriteAllBytes(capturePath, Capture);
                }
            }
        }
        else
        {
            shouldContinue = lastMedalScript.animationDelay <= 0.0f;        // while last medal animation does not played, do not continue
        }
    }

    // facebook combo will use this property
    public byte[] Capture
    {
        get
        {
            if (captured)
            {
                return capture.EncodeToPNG();
            }
            return null;
        }
    }

    // twitter combo will use this property
    public string CapturePath
    {
        get
        {
            if (captured)
            {
                return capturePath;
            }
            return null;
        }
    }
}
