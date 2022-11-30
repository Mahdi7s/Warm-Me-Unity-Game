using UnityEngine;
using System;
using System.Collections.Generic;
using Prime31;


#if UNITY_IPHONE
public class StoreKitManager : AbstractManager
{
	public static bool autoConfirmTransactions = true;


	// Fired when the product list your required returns. Automatically serializes the productString into StoreKitProduct's.
	public static event Action<List<StoreKitProduct>> productListReceivedEvent;

	// Fired when requesting product data fails
	public static event Action<string> productListRequestFailedEvent;

	// Fired anytime Apple updates a transaction if you called setShouldSendTransactionUpdateEvents with true. Check the transaction.transactionState to
	// know what state the transaction is currently in.
	public static event Action<StoreKitTransaction> transactionUpdatedEvent;

	// Fired when a product purchase has returned from Apple's servers and is awaiting completion. By default the plugin will finish transactions for you.
	// You can change that behavior by setting autoConfirmTransactions to false which then requires that you call StoreKitBinding.finishPendingTransaction
	// to complete a purchase.
	public static event Action<StoreKitTransaction> productPurchaseAwaitingConfirmationEvent;

	// Fired when a product is successfully paid for. The event will provide a StoreKitTransaction object that holds the productIdentifer and receipt of the purchased product.
	public static event Action<StoreKitTransaction> purchaseSuccessfulEvent;

	// Fired when a product purchase fails
	public static event Action<string> purchaseFailedEvent;

	// Fired when a product purchase is cancelled by the user or system
	public static event Action<string> purchaseCancelledEvent;

	// Fired when all transactions from the user's purchase history have successfully been added back to the queue. Note that this event will almost always
	// fire before each individual transaction is processed.
	public static event Action restoreTransactionsFinishedEvent;

	// Fired when an error is encountered while adding transactions from the user's purchase history back to the queue
	public static event Action<string> restoreTransactionsFailedEvent;

	// Fired when any SKDownload objects are updated by Apple. If using hosted content you should not be confirming the transaction until all downloads are complete.
	public static event Action<List<StoreKitDownload>> paymentQueueUpdatedDownloadsEvent;



    static StoreKitManager()
    {
		AbstractManager.initialize( typeof( StoreKitManager ) );

		// we ignore the results of this call because our only purpose is to trigger the creation of the required listener on the native side for transaction processing.
		StoreKitBinding.canMakePayments();
    }


	public void transactionUpdated( string json )
	{
		if( transactionUpdatedEvent != null )
			transactionUpdatedEvent( StoreKitTransaction.transactionFromJson( json ) );
	}


	public void productPurchaseAwaitingConfirmation( string json )
	{
		if( productPurchaseAwaitingConfirmationEvent != null )
			productPurchaseAwaitingConfirmationEvent( StoreKitTransaction.transactionFromJson( json ) );

		if( autoConfirmTransactions )
			StoreKitBinding.finishPendingTransactions();
	}


	public void productPurchased( string json )
	{
		if( purchaseSuccessfulEvent != null )
			purchaseSuccessfulEvent( StoreKitTransaction.transactionFromJson( json ) );
	}


	public void productPurchaseFailed( string error )
	{
		if( purchaseFailedEvent != null )
			purchaseFailedEvent( error );
	}


	public void productPurchaseCancelled( string error )
	{
		if( purchaseCancelledEvent != null )
			purchaseCancelledEvent( error );
	}


	public void productsReceived( string json )
	{
		if( productListReceivedEvent != null )
			productListReceivedEvent( StoreKitProduct.productsFromJson( json ) );
	}


	public void productsRequestDidFail( string error )
	{
		if( productListRequestFailedEvent != null )
			productListRequestFailedEvent( error );
	}


	public void restoreCompletedTransactionsFailed( string error )
	{
		if( restoreTransactionsFailedEvent != null )
			restoreTransactionsFailedEvent( error );
	}


	public void restoreCompletedTransactionsFinished( string empty )
	{
		if( restoreTransactionsFinishedEvent != null )
			restoreTransactionsFinishedEvent();
	}


	public void paymentQueueUpdatedDownloads( string json )
	{
		if( paymentQueueUpdatedDownloadsEvent != null )
			paymentQueueUpdatedDownloadsEvent( StoreKitDownload.downloadsFromJson( json ) );

	}

}
#endif
