using UnityEngine;
using System.Collections;


#if UNITY_IPHONE || UNITY_ANDROID
public class IAPProduct
{
    public string productId { get; private set; }
    public string title { get; private set; }
    public string price { get; private set; }
	public string description { get; private set; }
	public string currencyCode { get; private set; }


#if UNITY_IPHONE
	public IAPProduct( StoreKitProduct prod )
	{
		productId = prod.productIdentifier;
		title = prod.title;
		price = prod.price;
		description = prod.description;
		currencyCode = prod.currencyCode;
	}
#elif UNITY_ANDROID
	public IAPProduct( GoogleSkuInfo prod )
	{
		productId = prod.productId;
		title = prod.title;
		price = prod.price;
		description = prod.description;
		currencyCode = prod.priceCurrencyCode;
	}
#endif


	public override string ToString()
	{
		return string.Format( "[IAPProduct[: productId: {0}, title: {1}, price: {2}, description: {3}, priceCurrencyCode: {4}]",
			productId, title, price, description, currencyCode );
	}

}
#endif