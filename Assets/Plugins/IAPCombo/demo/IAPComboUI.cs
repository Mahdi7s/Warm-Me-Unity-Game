using UnityEngine;
using System.Collections;
using Prime31;


public class IAPComboUI : MonoBehaviourGUI
{
#if UNITY_IPHONE || UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Init IAP System" ) )
		{
			var key = "your public key from the Android developer portal here";
			key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmffbbQPr/zqRjP3vkxr1601/eKsXm5kO2NzQge8m7PeUj5V+saeounyL34U8WoZ3BvCRKbw6DrRLs2DMoVuCLq7QtJggBHT/bBSHGczEXGIPjWpw6OQb24EWM0PaTRTH2x2mC/X6RwIKcPLJFmy68T38Eh0DXnF4jjiIoaD0W8AYLjLzv0WvbIfgtJlvmmwvI2/Kta1LRnW3/Ggi5jb9UmXZAUIBz8kQtSH5FUCmFOQHMzekfg8rQ4VO1nlWhnB58UPwsxWt/DNyDfqv2VMeA2+VJG0fkiMl/6vWA7+ianVTU3owXcvxJHseEDUVYo1wEKfhK7ErGB7sxDJx5wHXAwIDAQAB";
			IAP.init( key );
		}


		if( GUILayout.Button( "Request Product Data" ) )
		{
			var androidSkus = new string[] { "com.prime31.testproduct", "android.test.purchased", "android.test.purchased2", "com.prime31.managedproduct", "com.prime31.testsubscription" };
			var iosProductIds = new string[] { "anotherProduct", "tt", "testProduct", "sevenDays", "oneMonthSubsciber" };

			IAP.requestProductData( iosProductIds, androidSkus, productList =>
			{
				Debug.Log( "Product list received" );
				Utils.logObject( productList );
			});
		}


		if( GUILayout.Button( "Restore Transactions (iOS only)" ) )
		{
			IAP.restoreCompletedTransactions( productId =>
			{
				Debug.Log( "restored purchased product: " + productId );
			});
		}


		if( GUILayout.Button( "Purchase Consumable" ) )
		{
#if UNITY_ANDROID
		var productId = "android.test.purchased";
#elif UNITY_IPHONE
		var productId = "testProduct";
#endif
			IAP.purchaseConsumableProduct( productId, ( didSucceed, error ) =>
			{
				Debug.Log( "purchasing product " + productId + " result: " + didSucceed );
				
				if( !didSucceed )
					Debug.Log( "purchase error: " + error );
			});
		}


		if( GUILayout.Button( "Purchase Non-Consumable" ) )
		{
#if UNITY_ANDROID
		var productId = "android.test.purchased2";
#elif UNITY_IPHONE
		var productId = "tt";
#endif
			IAP.purchaseNonconsumableProduct( productId, ( didSucceed, error ) =>
			{
				Debug.Log( "purchasing product " + productId + " result: " + didSucceed );
				
				if( !didSucceed )
					Debug.Log( "purchase error: " + error );
			});
		}

		endColumn();
	}
#endif
}
