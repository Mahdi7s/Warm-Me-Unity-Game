using UnityEngine;
using System.Collections;



#if UNITY_ANDROID
public class GoogleIAB
{
	private static AndroidJavaObject _plugin;

	static GoogleIAB()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.GoogleIABPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}


	// Toggles high detail logging on/off
	public static void enableLogging( bool shouldEnable )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		if( shouldEnable )
			Debug.LogWarning( "YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!" );
		_plugin.Call( "enableLogging", shouldEnable );
	}


	// Toggles automatic signature verification on/off
	public static void setAutoVerifySignatures( bool shouldVerify )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setAutoVerifySignatures", shouldVerify );
	}


	// Initializes the billing system
	public static void init( string publicKey )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "init", publicKey );
	}


	// Unbinds and shuts down the billing service
	public static void unbindService()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "unbindService" );
	}


	// Returns whether subscriptions are supported on the current device
	public static bool areSubscriptionsSupported()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;

		return _plugin.Call<bool>( "areSubscriptionsSupported" );
	}


	// Sends a request to get all completed purchases and product information as setup in the Play dashboard about the provided skus
	public static void queryInventory( string[] skus )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "queryInventory", new object[] { skus } );

		//var method = AndroidJNI.GetMethodID( _plugin.GetRawClass(), "queryInventory", "([Ljava/lang/String;)V" );
		//AndroidJNI.CallVoidMethod( _plugin.GetRawObject(), method, AndroidJNIHelper.CreateJNIArgArray( new object[] { skus } ) );
	}


	// Sends our a request to purchase the product
	public static void purchaseProduct( string sku )
	{
		purchaseProduct( sku, string.Empty );
	}

	public static void purchaseProduct( string sku, string developerPayload )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "purchaseProduct", sku, developerPayload );
	}


	// Sends out a request to consume the product
	public static void consumeProduct( string sku )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "consumeProduct", sku );
	}


	// Sends out a request to consume all of the provided products
	public static void consumeProducts( string[] skus )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "consumeProducts", new object[] { skus } );
	}

}
#endif