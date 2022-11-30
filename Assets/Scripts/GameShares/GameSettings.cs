//using UnityEngine;
//using System.Collections;
public enum PublishDestination
{
    Bazaar,
    GooglePlayAndAppStore,
    MyKet
}

internal static class GameSettings
{
    private const string bazaarPublicKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwD0wblPfDYEEO/9WFdvIakZDiXEEgf9+h9RpmN1lUrhskiM4XEIkx7YYYR6oALEKkDRrDcVBmfhVQhDSk9+ZvMWIaQBcf6+PKZFtZnUZEJysKGdd4iv25LN4C3+jXHlt8OgK6NM9UGba2PIpDUOLp7woEkWF/vgIdWmpRQDgBu7TvZpAvGksyZUeKZBI4Fk/GAcRgF1Ckf1QbJEyElMGIhEIQ9LIw2heyJglp3CMxsCAwEAAQ==";
    private const string googlePlayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmv3BBHcY3lXlPd8eq5e0VKWYcg+wI26Oj/i0EgbN+bMGREuc6q+w+9100xx0dDHgwyVR4sA6UgsUsSRaJiZJXvgP5herOcB9YdRAz7yMGnCho/ip1MaD7+SvsqBH+g+g1SnN92uIVl0eqqrpA5jk9hn70dXXPgTHD9I8diuuqWB04gcvJb1ldHTrs1tMPaua00AUg1iNRmLBuFfWwOAmbRAxmremF9XUXBDiYErkURw2sMYmzRHNQJvBsNRqTZA623atGX/yWXnRUh01bVpayGDqQ4lzN+ndLntO9Zx3DM336SLHikG7jmvDs+Ku9xjl1psbDOa8naQvnAykOaEYNQIDAQAB";
    private const string appStorePublicKey = "";
    private const string myKetPublicKey = "";
    internal const PublishDestination publishDestination = PublishDestination.Bazaar;//PublishDestination.GooglePlayAndAppStore;
    internal const float winnerMinTemp = 10.0f;
    internal const int minPipeDamage = 0;
    internal const int maxPipeDamage = 100;
    //internal const float hotSlothTemperature = 60f;
    //internal const float coldSlothTemperature = 20f;
    internal const int maxLevelsPerChapter = 9;
    internal const int maxNumberOfChapters = 7;
    
    internal static string PublicKey
    {
        get
        {
            switch (publishDestination)
            {
                case PublishDestination.Bazaar:
                    return bazaarPublicKey;
                case PublishDestination.GooglePlayAndAppStore:
                    #if UNITY_ANDROID
                    return googlePlayPublicKey;
                    #elif UNITY_IPHONE
                        return appStorePublicKey;
                    #endif
                case PublishDestination.MyKet:
                    return myKetPublicKey;
            }
            return null;
        }
    }

    internal static string BundleIdentifier
    {
        get
        {
            return "com.chocolate.";
        }
    }
}