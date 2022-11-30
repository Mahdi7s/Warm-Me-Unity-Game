using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AdMobEventListener : MonoBehaviour
{
#if UNITY_IPHONE
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		AdMobManager.receivedAdEvent += adViewDidReceiveAdEvent;
		AdMobManager.failedToReceiveAdEvent += adViewFailedToReceiveAdEvent;
		AdMobManager.interstitialReceivedAdEvent += interstitialDidReceiveAdEvent;
		AdMobManager.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		AdMobManager.receivedAdEvent -= adViewDidReceiveAdEvent;
		AdMobManager.failedToReceiveAdEvent -= adViewFailedToReceiveAdEvent;
		AdMobManager.interstitialReceivedAdEvent -= interstitialDidReceiveAdEvent;
		AdMobManager.interstitialFailedToReceiveAdEvent -= interstitialFailedToReceiveAdEvent;
	}



	void adViewDidReceiveAdEvent()
	{
		Debug.Log( "adViewDidReceiveAdEvent" );
	}


	void adViewFailedToReceiveAdEvent( string error )
	{
		Debug.Log( "adViewFailedToReceiveAdEvent: " + error );
	}


	void interstitialDidReceiveAdEvent()
	{
		Debug.Log( "interstitialDidReceiveAdEvent" );
	}


	void interstitialFailedToReceiveAdEvent( string error )
	{
		Debug.Log( "interstitialFailedToReceiveAdEvent: " + error );
	}
#endif
}


