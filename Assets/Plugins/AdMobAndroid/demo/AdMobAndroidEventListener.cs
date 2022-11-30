using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AdMobAndroidEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		AdMobAndroidManager.dismissingScreenEvent += dismissingScreenEvent;
		AdMobAndroidManager.failedToReceiveAdEvent += failedToReceiveAdEvent;
		AdMobAndroidManager.leavingApplicationEvent += leavingApplicationEvent;
		AdMobAndroidManager.presentingScreenEvent += presentingScreenEvent;
		AdMobAndroidManager.receivedAdEvent += receivedAdEvent;
		AdMobAndroidManager.interstitialDismissingScreenEvent += interstitialDismissingScreenEvent;
		AdMobAndroidManager.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEvent;
		AdMobAndroidManager.interstitialLeavingApplicationEvent += interstitialLeavingApplicationEvent;
		AdMobAndroidManager.interstitialPresentingScreenEvent += interstitialPresentingScreenEvent;
		AdMobAndroidManager.interstitialReceivedAdEvent += interstitialReceivedAdEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		AdMobAndroidManager.dismissingScreenEvent -= dismissingScreenEvent;
		AdMobAndroidManager.failedToReceiveAdEvent -= failedToReceiveAdEvent;
		AdMobAndroidManager.leavingApplicationEvent -= leavingApplicationEvent;
		AdMobAndroidManager.presentingScreenEvent -= presentingScreenEvent;
		AdMobAndroidManager.receivedAdEvent -= receivedAdEvent;
		AdMobAndroidManager.interstitialDismissingScreenEvent -= interstitialDismissingScreenEvent;
		AdMobAndroidManager.interstitialFailedToReceiveAdEvent -= interstitialFailedToReceiveAdEvent;
		AdMobAndroidManager.interstitialLeavingApplicationEvent -= interstitialLeavingApplicationEvent;
		AdMobAndroidManager.interstitialPresentingScreenEvent -= interstitialPresentingScreenEvent;
		AdMobAndroidManager.interstitialReceivedAdEvent -= interstitialReceivedAdEvent;
	}



	void dismissingScreenEvent()
	{
		Debug.Log( "dismissingScreenEvent" );
	}


	void failedToReceiveAdEvent( string error )
	{
		Debug.Log( "failedToReceiveAdEvent: " + error );
	}


	void leavingApplicationEvent()
	{
		Debug.Log( "leavingApplicationEvent" );
	}


	void presentingScreenEvent()
	{
		Debug.Log( "presentingScreenEvent" );
	}


	void receivedAdEvent()
	{
		Debug.Log( "receivedAdEvent" );
	}


	void interstitialDismissingScreenEvent()
	{
		Debug.Log( "interstitialDismissingScreenEvent" );
	}


	void interstitialFailedToReceiveAdEvent( string error )
	{
		Debug.Log( "interstitialFailedToReceiveAdEvent: " + error );
	}


	void interstitialLeavingApplicationEvent()
	{
		Debug.Log( "interstitialLeavingApplicationEvent" );
	}


	void interstitialPresentingScreenEvent()
	{
		Debug.Log( "interstitialPresentingScreenEvent" );
	}


	void interstitialReceivedAdEvent()
	{
		Debug.Log( "interstitialReceivedAdEvent" );
	}
#endif
}


