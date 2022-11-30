using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class StoreKitGUIManager : MonoBehaviourGUI
{
#if UNITY_IPHONE
	private List<StoreKitProduct> _products;


	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
	}


	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Get Can Make Payments" ) )
		{
			bool canMakePayments = StoreKitBinding.canMakePayments();
			Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
		}


		if( GUILayout.Button( "Request Product Data" ) )
		{
			// array of product ID's from iTunesConnect. MUST match exactly what you have there!
			var productIdentifiers = new string[] { "anotherProduct", "tt", "testProduct", "sevenDays", "oneMonthSubsciber" };
			StoreKitBinding.requestProductData( productIdentifiers );
		}


		if( GUILayout.Button( "Restore Completed Transactions" ) )
		{
			StoreKitBinding.restoreCompletedTransactions();
		}


		if( GUILayout.Button( "Enable High Detail Logs" ) )
		{
			StoreKitBinding.enableHighDetailLogs( true );
		}


		endColumn( true );


		// enforce the fact that we can't purchase products until we retrieve the product data
		if( _products != null && _products.Count > 0 )
		{
			if( GUILayout.Button( "Purchase Random Product" ) )
			{
				var productIndex = Random.Range( 0, _products.Count );
				var product = _products[productIndex];

				Debug.Log( "preparing to purchase product: " + product.productIdentifier );
				StoreKitBinding.purchaseProduct( product.productIdentifier, 1 );
			}
		}
		else
		{
			GUILayout.Label( "Once you successfully request product data a purchase button will appear here for testing" );
		}


		if( GUILayout.Button( "Get Saved Transactions" ) )
		{
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();

			// Print all the transactions to the console
			Debug.Log( "\ntotal transaction received: " + transactionList.Count );

			foreach( StoreKitTransaction transaction in transactionList )
				Debug.Log( transaction.ToString() + "\n" );
		}


		if( GUILayout.Button( "Turn Off Auto Confirmation of Transactions" ) )
		{
			// this is used when you want to validate receipts on your own server or when dealing with iTunes hosted content
			// you must manually call StoreKitBinding.finishPendingTransactions when the download/validation is complete
			StoreKitManager.autoConfirmTransactions = false;
		}


		if( GUILayout.Button( "Finish All Pending Transactions" ) )
		{
			// this is only necessary in the case where you turned off confirmation of transactions (see above)
			StoreKitBinding.finishPendingTransactions();
		}


		if( GUILayout.Button( "Cancel Downloads" ) )
		{
			StoreKitBinding.cancelDownloads();
		}


		if( GUILayout.Button( "Display App Store" ) )
		{
			StoreKitBinding.displayStoreWithProductId( "656176278" );
		}

		endColumn();
	}
#endif
}
