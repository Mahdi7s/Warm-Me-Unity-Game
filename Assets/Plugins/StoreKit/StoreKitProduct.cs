using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_IPHONE
public class StoreKitProduct
{
    public string productIdentifier;
    public string title;
    public string description;
    public string price;
	public string currencySymbol;
	public string currencyCode;
	public string formattedPrice;

	public string countryCode;
	public string downloadContentVersion;
	public bool downloadable;
	public List<Int64> downloadContentLengths = new List<Int64>();


	public static List<StoreKitProduct> productsFromJson( string json )
	{
		var productList = new List<StoreKitProduct>();

		var products = json.listFromJson();
		foreach( Dictionary<string,object> ht in products )
			productList.Add( productFromDictionary( ht ) );

		return productList;
	}


    public static StoreKitProduct productFromDictionary( Dictionary<string,object> ht )
    {
        StoreKitProduct product = new StoreKitProduct();

		if( ht.ContainsKey( "productIdentifier" ) )
        	product.productIdentifier = ht["productIdentifier"].ToString();

		if( ht.ContainsKey( "localizedTitle" ) )
        	product.title = ht["localizedTitle"].ToString();

		if( ht.ContainsKey( "localizedDescription" ) )
        	product.description = ht["localizedDescription"].ToString();

		if( ht.ContainsKey( "price" ) )
        	product.price = ht["price"].ToString();

		if( ht.ContainsKey( "currencySymbol" ) )
			product.currencySymbol = ht["currencySymbol"].ToString();

		if( ht.ContainsKey( "currencyCode" ) )
			product.currencyCode = ht["currencyCode"].ToString();

		if( ht.ContainsKey( "formattedPrice" ) )
			product.formattedPrice = ht["formattedPrice"].ToString();

		if( ht.ContainsKey( "countryCode" ) )
			product.countryCode = ht["countryCode"].ToString();

		if( ht.ContainsKey( "downloadContentVersion" ) )
			product.downloadContentVersion = ht["downloadContentVersion"].ToString();

		if( ht.ContainsKey( "downloadable" ) )
			product.downloadable = bool.Parse( ht["downloadable"].ToString() );

		if( ht.ContainsKey( "downloadContentLengths" ) && ht["downloadContentLengths"] is IList )
		{
			var tempLengths = ht["downloadContentLengths"] as List<object>;
			foreach( var dlLength in tempLengths )
				product.downloadContentLengths.Add( System.Convert.ToInt64( dlLength ) );
		}

        return product;
    }


	public override string ToString()
	{
		return String.Format( "<StoreKitProduct>\nID: {0}\ntitle: {1}\ndescription: {2}\nprice: {3}\ncurrencysymbol: {4}\nformattedPrice: {5}\ncurrencyCode: {6}\ncountryCode: {7}\ndownloadContentVersion: {8}\ndownloadable: {9}",
			productIdentifier, title, description, price, currencySymbol, formattedPrice, currencyCode, countryCode, downloadContentVersion, downloadable );
	}

}
#endif
