using UnityEngine;
using System.Collections;
using System.Collections.Generic;



#if UNITY_ANDROID
public class GooglePurchase
{
	public enum GooglePurchaseState
	{
		Purchased,
		Canceled,
		Refunded
	}

	public string packageName { get; private set; }
	public string orderId { get; private set; }
	public string productId { get; private set; }
	public string developerPayload { get; private set; }
	public string type { get; private set; }
	public long purchaseTime { get; private set; }
	public GooglePurchaseState purchaseState { get; private set; }
	public string purchaseToken { get; private set; }
	public string signature { get; private set; }
	public string originalJson { get; private set; }


	public static List<GooglePurchase> fromList( List<object> items )
	{
		var purchases = new List<GooglePurchase>();

		foreach( Dictionary<string,object> i in items )
			purchases.Add( new GooglePurchase( i ) );

		return purchases;
	}


	public GooglePurchase( Dictionary<string,object> dict )
	{
		if( dict.ContainsKey( "packageName" ) )
			packageName = dict["packageName"].ToString();

		if( dict.ContainsKey( "orderId" ) )
			orderId = dict["orderId"].ToString();

		if( dict.ContainsKey( "productId" ) )
			productId = dict["productId"].ToString();

		if( dict.ContainsKey( "developerPayload" ) )
			developerPayload = dict["developerPayload"].ToString();
		
		if( dict.ContainsKey( "type" ) )
			type = dict["type"] as string;

		if( dict.ContainsKey( "purchaseTime" ) )
			purchaseTime = long.Parse( dict["purchaseTime"].ToString() );

		if( dict.ContainsKey( "purchaseState" ) )
			purchaseState = (GooglePurchaseState)int.Parse( dict["purchaseState"].ToString() );

		if( dict.ContainsKey( "purchaseToken" ) )
			purchaseToken = dict["purchaseToken"].ToString();

		if( dict.ContainsKey( "signature" ) )
			signature = dict["signature"].ToString();

		if( dict.ContainsKey( "originalJson" ) )
			originalJson = dict["originalJson"].ToString();
	}


	public override string ToString()
	{
		return string.Format( "<GooglePurchase> packageName: {0}, orderId: {1}, productId: {2}, developerPayload: {3}, purchaseToken: {4}, purchaseState: {5}, signature: {6}, type: {7}, json: {8}",
			packageName, orderId, productId, developerPayload, purchaseToken, purchaseState, signature, type, originalJson );
	}
}
#endif