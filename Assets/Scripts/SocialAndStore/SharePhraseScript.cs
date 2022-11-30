using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class SharePhraseScript : MonoBehaviour
{
 /*   public Collider2D FbButton;
    public Collider2D TwtButton;
    public Transform buttonsContainer;
    public Animator internetBoad;
    private bool initialized;
    private CaptureAndActiveButtonsScript buttonsActivatorScript;

    void Update()
    {
        var coll = TouchUtility.GetTouchedCollider();
        if (coll != null)
        {
            if (coll == FbButton)
            {
                ShareToFacebook();
            }
            else if (coll == TwtButton)
            {
                ShareToTwitter();
            }
        }
    }

    void Start()
    {
        if (ConnectionUtility.IsConnectedToInternet())
        {
            Init();
        }
        buttonsActivatorScript = GetComponent<CaptureAndActiveButtonsScript>();
    }

    private void ShareToFacebook()
    {
        if (ConnectionUtility.IsConnectedToInternet())
        {
            if (initialized)
            {
                string[] permissions;
                if (!FacebookCombo.isSessionValid())
                {
                    permissions = new string[] { "email" };
                    FacebookCombo.loginWithReadPermissions(permissions);
                }
                permissions = new string[] { "publish_actions", "publish_stream" };
                FacebookCombo.reauthorizeWithPublishPermissions(permissions, FacebookSessionDefaultAudience.Everyone);
                if (Facebook.instance != null)
                {
                    Facebook.instance.postImage(buttonsActivatorScript.Capture, "WarmMe Game", (str, obj) => { InitializeScript.Debugger = str; });
                }
            }
            else
            {
                Init();
                ShareToFacebook();
            }
        }
        else
        {
            internetBoad.SetTrigger("In");
        }
    }

    private void ShareToTwitter()
    {
        if (ConnectionUtility.IsConnectedToInternet())
        {
            if (initialized)
            {
                if (TwitterCombo.isLoggedIn())
                {
                    TwitterCombo.postStatusUpdate("Warm Me Game", buttonsActivatorScript.CapturePath);
                }
                else
                {
                    TwitterCombo.showLoginDialog();
                    TwitterCombo.postStatusUpdate("Warm Me Game", buttonsActivatorScript.CapturePath);
                }
            }
            else
            {
                Init();
                ShareToTwitter();
            }
        }
        else
        {
            internetBoad.SetTrigger("In");
        }
    }

    private void Init()
    {
        FacebookCombo.init();
        TwitterCombo.init("gwZnWoTWfvkg6DXC0tcqv81lb", "LTXBDuc2fQ8Nor31iDG0bnMCDsTGT14J3H3VpM0UjMjbB7473n");
        initialized = true;
    }
  * */
}