using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class GoogleIABManager : AbstractManager
{
#if UNITY_ANDROID
	// Fired after init is called when billing is supported on the device
	public static event Action billingSupportedEvent;

	// Fired after init is called when billing is not supported on the device
	public static event Action<string> billingNotSupportedEvent;

	// Fired when the inventory and purchase history query has returned
	public static event Action<List<GooglePurchase>,List<GoogleSkuInfo>> queryInventorySucceededEvent;

	// Fired when the inventory and purchase history query fails
	public static event Action<string> queryInventoryFailedEvent;

	// Fired when a purchase completes allowing you to verify the signature on an external server if you would like
	public static event Action<string,string> purchaseCompleteAwaitingVerificationEvent;

	// Fired when a purchase succeeds
	public static event Action<GooglePurchase> purchaseSucceededEvent;

	// Fired when a purchase fails. Includes the result message and the response (int)
	public static event Action<string,int> purchaseFailedEvent;

	// Fired when a call to consume a product succeeds
	public static event Action<GooglePurchase> consumePurchaseSucceededEvent;

	// Fired when a call to consume a product fails
	public static event Action<string> consumePurchaseFailedEvent;


	static GoogleIABManager()
	{
		AbstractManager.initialize( typeof( GoogleIABManager ) );
	}


	public void billingSupported( string empty )
	{
		billingSupportedEvent.fire();
	}


	public void billingNotSupported( string error )
	{
		billingNotSupportedEvent.fire( error );
	}


	public void queryInventorySucceeded( string json )
	{
		if( queryInventorySucceededEvent != null )
		{
			var dict = json.dictionaryFromJson();
			queryInventorySucceededEvent( GooglePurchase.fromList( dict["purchases"] as List<object> ), GoogleSkuInfo.fromList( dict["skus"] as List<object> ) );
		}
	}


	public void queryInventoryFailed( string error )
	{
		queryInventoryFailedEvent.fire( error );
	}


	public void purchaseCompleteAwaitingVerification( string json )
	{
		if( purchaseCompleteAwaitingVerificationEvent != null )
		{
			var dict = json.dictionaryFromJson();
			var purchaseData = dict["purchaseData"].ToString();
			var signature = dict["signature"].ToString();

			purchaseCompleteAwaitingVerificationEvent( purchaseData, signature );
		}
	}

	public void purchaseSucceeded( string json )
	{
		purchaseSucceededEvent.fire( new GooglePurchase( json.dictionaryFromJson() ) );
	}


	public void purchaseFailed( string json )
	{
		if( purchaseFailedEvent != null )
		{
			var dict = Json.decode<Dictionary<string,object>>( json );
			purchaseFailedEvent( dict["result"].ToString(), int.Parse( dict["response"].ToString() ) );
		}
	}


	public void consumePurchaseSucceeded( string json )
	{
		if( consumePurchaseSucceededEvent != null )
			consumePurchaseSucceededEvent.fire( new GooglePurchase( json.dictionaryFromJson() ) );
	}


	public void consumePurchaseFailed( string error )
	{
		consumePurchaseFailedEvent.fire( error );
	}

#endif
}

