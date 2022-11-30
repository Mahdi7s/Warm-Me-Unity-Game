using UnityEngine;
using System.Collections;
using Prime31;


public class AdMobComboUI : MonoBehaviourGUI
{
#if UNITY_ANDROID || UNITY_IPHONE
	void OnGUI()
	{
		beginColumn();


		if( GUILayout.Button( "Init" ) )
		{
			// Replace with your Android and iOS publisher IDs!
			AdMob.init( "a14de56b4e8babd", "a150630f69e2282" );
		}


		if( GUILayout.Button( "Set Test Devices" ) )
		{
			// replace with your device info!
			AdMob.setTestDevices( new string[] { "6D13FA054BC989C5AC41900EE14B0C1B", "8E2F04DC5B964AFD3BC2D90396A9DA6E", "3BAB93112BBB08713B6D6D0A09EDABA1", "079adeed23ef3e9a9ddf0f10c92b8e18", "E2236E5E84CD318D4AD96B62B6E0EE2B" } );
		}


		if( GUILayout.Button( "Create Smart Banner" ) )
		{
			// place it on the top
			AdMob.createBanner( AdMobBanner.SmartBanner, AdMobLocation.BottomCenter );
		}


		if( GUILayout.Button( "Create 320x50 banner" ) )
		{
			// place it on the top
			AdMob.createBanner( AdMobBanner.Phone_320x50, AdMobLocation.TopCenter );
		}


		if( GUILayout.Button( "Create 300x250 banner" ) )
		{
			// center it on the top
			AdMob.createBanner( AdMobBanner.Tablet_300x250, AdMobLocation.BottomCenter );
		}


		if( GUILayout.Button( "Destroy Banner" ) )
		{
			AdMob.destroyBanner();
		}


		endColumn( true );


		if( GUILayout.Button( "Request Interstitial" ) )
		{
			AdMob.requestInterstital( "a14de56b4e8babd", "a14d3e67dfeb7ba" );
		}


		if( GUILayout.Button( "Is Interstitial Ready?" ) )
		{
			var isReady = AdMob.isInterstitalReady();
			Debug.Log( "is interstitial ready? " + isReady );
		}


		if( GUILayout.Button( "Display Interstitial" ) )
		{
			AdMob.displayInterstital();
		}

		endColumn();
	}



	#region Optional: Example of Subscribing to All Events

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


	void receivedAdEvent()
	{
		Debug.Log( "receivedAdEvent" );
	}


	void failedToReceiveAdEvent( string error )
	{
		Debug.Log( "failedToReceiveAdEvent: " + error );
	}


	void interstitialReceivedAdEvent()
	{
		Debug.Log( "interstitialReceivedAdEvent" );
	}


	void interstitialFailedToReceiveAdEvent( string error )
	{
		Debug.Log( "interstitialFailedToReceiveAdEvent: " + error );
	}

	#endregion

#endif
}
