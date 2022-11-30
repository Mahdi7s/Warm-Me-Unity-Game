using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_ANDROID
public static class IAP
{
#if UNITY_ANDROID
	// Android only. When the requestProductData call completes, this List will be populated with any purchases already made and returned by the Play store
	public static List<GooglePurchase> androidPurchasedItems = new List<GooglePurchase>();
#endif

	private const string CONSUMABLE_PAYLOAD = "consume";
	private const string NON_CONSUMABLE_PAYLOAD = "nonconsume";

	private static Action<List<IAPProduct>> _productListReceivedAction;
	private static Action<bool,string> _purchaseCompletionAction;

	#pragma warning disable
	private static Action<string> _purchaseRestorationAction;
	#pragma warning restore


	static IAP()
	{
#if UNITY_IPHONE
		// product list
		StoreKitManager.productListReceivedEvent += ( products ) =>
		{
			var convertedProducts = new List<IAPProduct>();
			foreach( var p in products )
				convertedProducts.Add( new IAPProduct( p ) );

			if( _productListReceivedAction != null )
				_productListReceivedAction( convertedProducts );
		};
		StoreKitManager.productListRequestFailedEvent += ( error ) =>
		{
			if( _productListReceivedAction != null )
				_productListReceivedAction( null );
		};

		// purchases
		StoreKitManager.purchaseSuccessfulEvent += ( transaction ) =>
		{
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( true, null );

			if( _purchaseRestorationAction != null )
				_purchaseRestorationAction( transaction.productIdentifier );
		};
		StoreKitManager.purchaseFailedEvent += ( error ) =>
		{
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( false, error );
		};
		StoreKitManager.purchaseCancelledEvent += ( error ) =>
		{
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( false, error );
		};

		// restoration
		StoreKitManager.restoreTransactionsFailedEvent += ( error ) =>
		{
			Debug.Log( "restore transactions failed: " + error );
			// we null out the _purchaseRestorationAction so that it won't get called if the user later purchases a product
			_purchaseRestorationAction = null;
		};

#elif UNITY_ANDROID
		// inventory
		GoogleIABManager.queryInventorySucceededEvent += ( purchases, skus ) =>
		{
			// Android is a bit different and stores purchased items for later consumption so we save off those items here
			androidPurchasedItems = purchases;

			var convertedProducts = new List<IAPProduct>();
			foreach( var p in skus )
				convertedProducts.Add( new IAPProduct( p ) );

			if( _productListReceivedAction != null )
				_productListReceivedAction( convertedProducts );
		};
		GoogleIABManager.queryInventoryFailedEvent += ( error ) =>
		{
			Debug.Log( "fetching prouduct data failed: " + error );
			if( _productListReceivedAction != null )
				_productListReceivedAction( null );
		};

		// purchases
		GoogleIABManager.purchaseSucceededEvent += ( purchase ) =>
		{
			if( purchase.developerPayload == NON_CONSUMABLE_PAYLOAD )
			{
				if( _purchaseCompletionAction != null )
					_purchaseCompletionAction( true, null );
			}
			else
			{
				// we need to consume this one
				GoogleIAB.consumeProduct( purchase.productId );
			}
		};
		GoogleIABManager.purchaseFailedEvent += ( error, response ) =>
		{
			Debug.Log( "purchase failed: " + error );
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( false, error );
		};

		// consumption
		GoogleIABManager.consumePurchaseSucceededEvent += ( purchase ) =>
		{
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( true, null );
		};
		GoogleIABManager.consumePurchaseFailedEvent += ( error ) =>
		{
			if( _purchaseCompletionAction != null )
				_purchaseCompletionAction( false, null );
		};
#endif
	}


	// Initializes the billing system. Call this at app launch to prepare the IAP system.
	public static void init( string androidPublicKey )
	{
#if UNITY_ANDROID
		GoogleIAB.init( androidPublicKey );
#endif
	}


	// Accepts two arrays of product identifiers (one for iOS one for Android). All of the products you have for sale should be requested in one call.
	// On Android, after this call completes the androidPurchasedItems will be populated.
	public static void requestProductData( string[] iosProductIdentifiers, string[] androidSkus, Action<List<IAPProduct>> completionHandler )
	{
		_productListReceivedAction = completionHandler;

#if UNITY_ANDROID
		GoogleIAB.queryInventory( androidSkus );
#elif UNITY_IPHONE
		StoreKitBinding.requestProductData( iosProductIdentifiers );
#endif
	}


	// Purchases the given product and quantity. completionHandler provides if the purchase succeeded (bool) and an error message which will be populated if
	// the purchase failed.
	public static void purchaseConsumableProduct( string productId, Action<bool,string> completionHandler )
	{
		_purchaseCompletionAction = completionHandler;
		_purchaseRestorationAction = null;

#if UNITY_ANDROID
		GoogleIAB.purchaseProduct( productId, CONSUMABLE_PAYLOAD );
#elif UNITY_IPHONE
		StoreKitBinding.purchaseProduct( productId, 1 );
#endif
	}


	// Purchases the given product and quantity. completionHandler provides if the purchase succeeded (bool) and an error message which will be populated if
	// the purchase failed.
	public static void purchaseNonconsumableProduct( string productId, Action<bool,string> completionHandler )
	{
		_purchaseCompletionAction = completionHandler;
		_purchaseRestorationAction = null;

#if UNITY_ANDROID
		GoogleIAB.purchaseProduct( productId, NON_CONSUMABLE_PAYLOAD );
#elif UNITY_IPHONE
		StoreKitBinding.purchaseProduct( productId, 1 );
#endif
	}


	// iOS Only. Restores all previous transactions. This is used when a user gets a new device and they need to restore their old purchases.
	// DO NOT call this on every launch. It will prompt the user for their password. Each transaction that is restored will have
	// the completion handler called for it
	public static void restoreCompletedTransactions( Action<string> completionHandler )
	{
		_purchaseCompletionAction = null;
		_purchaseRestorationAction = completionHandler;

#if UNITY_IPHONE
		StoreKitBinding.restoreCompletedTransactions();
#endif
	}

}
#endif