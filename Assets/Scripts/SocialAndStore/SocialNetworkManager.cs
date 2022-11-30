using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public enum SocialNetwork
{
    Facebook,
    Twitter,
    NoOfItems
}

public static class SocialNetworkManager
{
/*    private static Facebook facebook;
    /// <summary>
    /// This method will initialize Facebook and Twitter social networks. You can use this after disconnect to 
    /// reinitialize Facebook and Twitter profiles.
    /// </summary>
    static SocialNetworkManager()
    {
        if (ConnectionUtility.IsConnectedToInternet())
        {
            FacebookCombo.init();
            TwitterCombo.init("tzd7uTjWPegMfrKTvXS2V1efd", "eqHbda1vcnR8RAPSQFUFjh6D1MmWJpGrgtmnywQG9vGmB4l3In");
        }
    }

    /// <summary>
    /// This method will return the connection status of Facebook and Twitter social networks in a bool array.
    /// First element of array is Facebook's and second one is Twitter's.
    /// </summary>
    public static bool[] AreSessionsValid()
    {
        bool[] array = new bool[(int)SocialNetwork.NoOfItems];
        for (int cntr=0; cntr<array.Length; cntr++)
        {
            array [cntr] = false;
        }
        if (ConnectionUtility.IsConnectedToInternet())
        {
            array [(int)SocialNetwork.Facebook] = FacebookCombo.isSessionValid();
            array [(int)SocialNetwork.Twitter] = TwitterCombo.isLoggedIn();
        }
        return array;
    }

    /// <summary>
    /// Login the specified social network.
    /// </summary>
    /// <param name="sn">Sn.</param>
    public static bool Login(SocialNetwork sn)
    {
        switch (sn)
        {
            case SocialNetwork.Facebook:
                // Note: requesting publish permissions here will result in a crash. Only read permissions are permitted.
                var permissions = new string[] { "email" };
                FacebookCombo.loginWithReadPermissions(permissions);
                return FacebookCombo.isSessionValid();
            case SocialNetwork.Twitter:
                TwitterCombo.showLoginDialog();
                return TwitterCombo.isLoggedIn();
        }
        Debug.Log("This fucking social network you determined is not listed loser!");
        return false;
    }

    public static void ShareScore(SocialNetwork sn, string imagePathForTwitter="")
    {
        switch (sn)
        {
            case SocialNetwork.Facebook:
                if (AreSessionsValid() [(int)SocialNetwork.Facebook])
                {
                    Action<bool> completionHandler = delegate(bool myParam)
                    {
                        Debug.Log(myParam.ToString());
                    };
                    if (facebook == null)
                    {
                        facebook = new Facebook();
                    }
                    facebook.postScore(GameState.GetAllCoins(), completionHandler);
                } else
                {
                    if (Login(SocialNetwork.Facebook))
                    {
                        ShareScore(SocialNetwork.Facebook);
                    }
                }
                break;
            case SocialNetwork.Twitter:
                if (AreSessionsValid() [(int)SocialNetwork.Twitter])
                {
                    if (imagePathForTwitter == "")
                    {
                        TwitterCombo.postStatusUpdate("Posting through Warm Me Game: " + GameState.GetAllCoins().ToString());
                    } else
                    {
                        TwitterCombo.postStatusUpdate("Posting through Warm Me Game: " + GameState.GetAllCoins().ToString(), imagePathForTwitter);
                    }
                } else
                {
                    if (Login(SocialNetwork.Twitter))
                    {
                        ShareScore(SocialNetwork.Twitter);
                    }
                }
                break;
            default:
                Debug.Log("This fucking social network you determined is not listed loser!");
                break;
        }
    }

    public static void GetFacebookFriendsList()
    {
        Action<string, FacebookFriendsResult> completionHandler = delegate(string stringParam, FacebookFriendsResult facebookFriendResult)
        {
            GUI.Label(new Rect(500, 500, 100, 100), "stringParam: " + stringParam);
            GUI.Label(new Rect(600, 600, 100, 100), "facebookFriendResult: " + facebookFriendResult.ToString());
        };
    }
 * */
}