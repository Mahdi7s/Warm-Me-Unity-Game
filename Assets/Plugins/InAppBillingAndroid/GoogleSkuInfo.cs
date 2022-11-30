using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



#if UNITY_ANDROID
public class GoogleSkuInfo
{
	public string title { get; private set; }
	public string price { get; private set; }
	public string type { get; private set; }
	public string description { get; private set; }
	public string productId { get; private set; }
	public string priceCurrencyCode { get; private set; }
	public long priceAmountMicros { get; private set; }


	public static List<GoogleSkuInfo> fromList( List<object> items )
	{
		var skuInfos = new List<GoogleSkuInfo>();

		foreach( Dictionary<string,object> i in items )
			skuInfos.Add( new GoogleSkuInfo( i ) );

		return skuInfos;
	}


	public GoogleSkuInfo( Dictionary<string,object> dict )
	{
		if( dict.ContainsKey( "title" ) )
			title = dict["title"] as string;

		if( dict.ContainsKey( "price" ) )
			price = dict["price"] as string;

		if( dict.ContainsKey( "type" ) )
			type = dict["type"] as string;

		if( dict.ContainsKey( "description" ) )
			description = dict["description"] as string;

		if( dict.ContainsKey( "productId" ) )
			productId = dict["productId"] as string;

		if( dict.ContainsKey( "price_currency_code" ) )
			priceCurrencyCode = dict["price_currency_code"] as string;

		if( dict.ContainsKey( "price_amount_micros" ) )
		{
			var temp = dict["price_amount_micros"] as long?;
			if( temp != null )
				priceAmountMicros = temp.Value;
		}
	}


	public override string ToString()
	{
		 return string.Format( "<GoogleSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}, priceCurrencyCode: {5}",
			 title, price, type, description, productId, priceCurrencyCode );
	}

}
#endif