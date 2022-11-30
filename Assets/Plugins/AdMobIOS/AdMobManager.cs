using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class AdMobManager : AbstractManager
{
#if UNITY_IPHONE
	// Fired when the ad view receives an ad
	public static event Action receivedAdEvent;
	
	// Fired when the ad view fails to receive an ad
	public static event Action<string> failedToReceiveAdEvent;
	
	// Fired when an interstitial is ready to show
	public static event Action interstitialReceivedAdEvent;
	
	// Fired when the interstitial download fails
	public static event Action<string> interstitialFailedToReceiveAdEvent;


	static AdMobManager()
	{
		AbstractManager.initialize( typeof( AdMobManager ) );
	}


	public void adViewDidReceiveAd( string empty )
	{
		if( receivedAdEvent != null )
			receivedAdEvent();
	}


	public void adViewFailedToReceiveAd( string error )
	{
		if( failedToReceiveAdEvent != null )
			failedToReceiveAdEvent( error );
	}


	public void interstitialDidReceiveAd( string empty )
	{
		if( interstitialReceivedAdEvent != null )
			interstitialReceivedAdEvent();
	}


	public void interstitialFailedToReceiveAd( string error )
	{
		if( interstitialFailedToReceiveAdEvent != null )
			interstitialFailedToReceiveAdEvent( error );
	}
	
#endif
}

