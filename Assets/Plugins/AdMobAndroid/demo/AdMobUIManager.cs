using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class AdMobUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();


		if( GUILayout.Button( "Init" ) )
		{
			// replace with your app ID!
			AdMobAndroid.init( "a14de56b4e8babd" );
		}


		if( GUILayout.Button( "Set Test Devices" ) )
		{
			// replace with your test devices!
			AdMobAndroid.setTestDevices( new string[] { "6D13FA054BC989C5AC41900EE14B0C1B", "8E2F04DC5B964AFD3BC2D90396A9DA6E", "3BAB93112BBB08713B6D6D0A09EDABA1", "E2236E5E84CD318D4AD96B62B6E0EE2B" } );
		}


		if( GUILayout.Button( "Create Smart Banner" ) )
		{
			// place it on the top
			AdMobAndroid.createBanner( AdMobAndroidAd.smartBanner, AdMobAdPlacement.BottomCenter );
		}


		if( GUILayout.Button( "Create 320x50 banner" ) )
		{
			// replace with your ad unit ID!
			AdMobAndroid.createBanner( "ca-app-pub-8386987260001674/8398905145", AdMobAndroidAd.phone320x50, AdMobAdPlacement.TopCenter );
		}


		if( GUILayout.Button( "Create 300x250 banner" ) )
		{
			// center it on the top
			AdMobAndroid.createBanner( AdMobAndroidAd.tablet300x250, AdMobAdPlacement.BottomCenter );
		}


		if( GUILayout.Button( "Destroy Banner" ) )
		{
			AdMobAndroid.destroyBanner();
		}


		endColumn( true );


		if( GUILayout.Button( "Refresh Ad" ) )
		{
			AdMobAndroid.refreshAd();
		}


		if( GUILayout.Button( "Request Interstitial" ) )
		{
			// replace with your adUnitId!
			AdMobAndroid.requestInterstital( "ca-app-pub-8386987260001674/9875638345" );
		}


		if( GUILayout.Button( "Is Interstitial Ready?" ) )
		{
			var isReady = AdMobAndroid.isInterstitalReady();
			Debug.Log( "is interstitial ready? " + isReady );
		}


		if( GUILayout.Button( "Display Interstitial" ) )
		{
			AdMobAndroid.displayInterstital();
		}


		if( GUILayout.Button( "Hide Banner" ) )
		{
			AdMobAndroid.hideBanner( true );
		}


		if( GUILayout.Button( "Show Banner" ) )
		{
			AdMobAndroid.hideBanner( false );
		}

		endColumn();
	}
#endif
}
