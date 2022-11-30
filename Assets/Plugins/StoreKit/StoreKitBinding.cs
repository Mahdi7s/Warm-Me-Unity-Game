using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


#if UNITY_IPHONE
public class StoreKitBinding
{
	[DllImport("__Internal")]
	private static extern bool _storeKitCanMakePayments();

	public static bool canMakePayments()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _storeKitCanMakePayments();
		return false;
	}


	[DllImport("__Internal")]
	private static extern void _storeKitSetApplicationUsername( string applicationUserName );

	// iOS 7+ only. This is used to help the store detect irregular activity.
	// The recommended implementation is to use a one-way hash of the user's account name to calculate the value for this property.
    public static void setApplicationUsername( string applicationUserName )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitSetApplicationUsername( applicationUserName );
    }


    [DllImport("__Internal")]
    private static extern string _storeKitGetAppStoreReceiptUrl();

	// iOS 7 only. Returns the location of the App Store receipt file. If called on an older iOS version it returns null.
    public static string getAppStoreReceiptLocation()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _storeKitGetAppStoreReceiptUrl();

		return null;
    }


	[DllImport("__Internal")]
	private static extern void _storeKitSendTransactionUpdateEvents( bool sendTransactionUpdateEvents );

	// By default, the transactionUpdatedEvent will not be called to avoid excessive string allocations. If you pass true to this method it will be called.
	public static void setShouldSendTransactionUpdateEvents( bool sendTransactionUpdateEvents )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitSendTransactionUpdateEvents( sendTransactionUpdateEvents );
	}


	[DllImport("__Internal")]
	private static extern void _storeKitEnableHighDetailLogs( bool shouldEnable );

	// Enables/disables high detail logs
	public static void enableHighDetailLogs( bool shouldEnable )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitEnableHighDetailLogs( shouldEnable );
	}


	[DllImport("__Internal")]
	private static extern void _storeKitRequestProductData( string productIdentifier );

	// Accepts an array of product identifiers. All of the products you have for sale should be requested in one call.
    public static void requestProductData( string[] productIdentifiers )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitRequestProductData( string.Join( ",", productIdentifiers ) );
    }


    [DllImport("__Internal")]
    private static extern void _storeKitPurchaseProduct( string productIdentifier, int quantity );

	// Purchases the given product and quantity
    public static void purchaseProduct( string productIdentifier, int quantity )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitPurchaseProduct( productIdentifier, quantity );
    }


    [DllImport("__Internal")]
    private static extern void _storeKitFinishPendingTransactions();

	// Finishes any pending transactions that were being tracked
    public static void finishPendingTransactions()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitFinishPendingTransactions();
    }


	[DllImport("__Internal")]
	private static extern void _storeKitForceFinishPendingTransactions();

	// Force finishes any and all pending transactions including those being tracked and any random transactions in Apple's queue
	public static void forceFinishPendingTransactions()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitFinishPendingTransactions();
	}


    [DllImport("__Internal")]
    private static extern void _storeKitFinishPendingTransaction( string transactionIdentifier );

	// Finishes the pending transaction identified by the transactionIdentifier
    public static void finishPendingTransaction( string transactionIdentifier )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitFinishPendingTransaction( transactionIdentifier );
    }


    [DllImport("__Internal")]
    private static extern void _storeKitPauseDownloads();

	// Pauses any pending downloads
    public static void pauseDownloads()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitPauseDownloads();
    }


    [DllImport("__Internal")]
    private static extern void _storeKitResumeDownloads();

	// Resumes any pending paused downloads
    public static void resumeDownloads()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitResumeDownloads();
    }


    [DllImport("__Internal")]
    private static extern void _storeKitCancelDownloads();

	// Cancels any pending downloads
    public static void cancelDownloads()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitCancelDownloads();
    }


    [DllImport("__Internal")]
    private static extern void _storeKitRestoreCompletedTransactions();

	// Restores all previous transactions.  This is used when a user gets a new device and they need to restore their old purchases.
	// DO NOT call this on every launch.  It will prompt the user for their password. Each transaction that is restored will have the normal
	// purchaseSuccessfulEvent fire for when restoration is complete.
    public static void restoreCompletedTransactions()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitRestoreCompletedTransactions();
    }


    [DllImport("__Internal")]
    private static extern string _storeKitGetAllSavedTransactions();

	// Returns a list of all the transactions that occured on this device.  They are stored in the Document directory.
    public static List<StoreKitTransaction> getAllSavedTransactions()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// Grab the transactions and parse them out
			var json = _storeKitGetAllSavedTransactions();
			return StoreKitTransaction.transactionsFromJson( json );
		}

		return new List<StoreKitTransaction>();
    }


	[DllImport("__Internal")]
	private static extern void _storeKitDisplayStoreWithProductId( string productId, string affiliateToken );

	// iOS 6+ only! Displays the App Store with the given productId in app. The affiliateToken parameter will only work on iOS 8+.
	public static void displayStoreWithProductId( string productId, string affiliateToken = null )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_storeKitDisplayStoreWithProductId( productId, affiliateToken );
	}

}
#endif
