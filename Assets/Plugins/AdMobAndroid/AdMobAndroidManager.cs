using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID
public class AdMobAndroidManager : AbstractManager
{
	// Fired when a new ad is loaded
	public static event Action receivedAdEvent;
	
	// Fired when an ad fails to be loaded
	public static event Action<string> failedToReceiveAdEvent;
	
	// Fired when a screen event ends (a screen event is an AdMob ad being shown)
	public static event Action dismissingScreenEvent;
	
	// Fired when touching an ad will take the user out of your game
	public static event Action leavingApplicationEvent;
	
	// Fired when a screen event is occurring
	public static event Action presentingScreenEvent;
	
	// Fired when an interstitial is loaded and ready for use
	public static event Action interstitialReceivedAdEvent;
	
	// Fired when an interstitial is dismissed
	public static event Action interstitialDismissingScreenEvent;
	
	// Fired when an interstitial fails to receive an ad
	public static event Action<string> interstitialFailedToReceiveAdEvent;
	
	// Fired when a user action on an interstitial causes them to leave your game
	public static event Action interstitialLeavingApplicationEvent;
	
	// Fired when an interstitial is presented
	public static event Action interstitialPresentingScreenEvent;


	static AdMobAndroidManager()
	{
		AbstractManager.initialize( typeof( AdMobAndroidManager ) );
	}


	public void dismissingScreen( string empty )
	{
		if( dismissingScreenEvent != null )
			dismissingScreenEvent();
	}


	public void failedToReceiveAd( string error )
	{
		if( failedToReceiveAdEvent != null )
			failedToReceiveAdEvent( error );
	}


	public void leavingApplication( string empty )
	{
		if( leavingApplicationEvent != null )
			leavingApplicationEvent();
	}


	public void presentingScreen( string empty )
	{
		if( presentingScreenEvent != null )
			presentingScreenEvent();
	}


	public void receivedAd( string empty )
	{
		if( receivedAdEvent != null )
			receivedAdEvent();
	}


	public void interstitialDismissingScreen( string empty )
	{
		if( interstitialDismissingScreenEvent != null )
			interstitialDismissingScreenEvent();
	}


	public void interstitialFailedToReceiveAd( string error )
	{
		if( interstitialFailedToReceiveAdEvent != null )
			interstitialFailedToReceiveAdEvent( error );
	}


	public void interstitialLeavingApplication( string empty )
	{
		if( interstitialLeavingApplicationEvent != null )
			interstitialLeavingApplicationEvent();
	}


	public void interstitialPresentingScreen( string empty )
	{
		if( interstitialPresentingScreenEvent != null )
			interstitialPresentingScreenEvent();
	}


	public void interstitialReceivedAd( string empty )
	{
		if( interstitialReceivedAdEvent != null )
			interstitialReceivedAdEvent();
	}

}
#endif
