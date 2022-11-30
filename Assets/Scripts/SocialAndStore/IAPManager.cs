using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class IAPItemExtensions
{
    public static string ToItemString(this IAPItem item, bool forAndroid)
    {
        var retval = RemovePrice(item);
        return retval;
    }

    public static int GetPrice(this IAPItem item)
    {
        var dollarWithC = item.ToString().Split(new[]{"x"}, StringSplitOptions.RemoveEmptyEntries) [0];
        //var cents = int.Parse(dollarWithC.Substring(0, dollarWithC.Length - 1));
        var cents = int.Parse(dollarWithC.Replace("CoinPack", ""));

        return cents;
    }

    public static string RemovePrice(IAPItem item)
    {
        string str = item.ToString();
        str = str.Substring(0, str.IndexOf('x')).ToLower();
        return str.ToLower();
    }
}

public static class IAPManager
{
    private static bool _initialized = false;

    public static void Init()
    {
        if (!_initialized)
        {
            IAP.init(GameSettings.PublicKey);
            _initialized = true;
        }
    }

    public static void RequestItemData(Action<List<IAPProduct>> completionHandler)
    {
        var androidSkus = IAPShares.GetListOfItems(true);
        var iosProductIds = IAPShares.GetListOfItems(false);
        
        IAP.requestProductData(iosProductIds, androidSkus, completionHandler);
    }

    /// <summary>
    /// this is just for ios
    /// </summary>
    public static void Ios_RestoreTransactions()
    {
        IAP.restoreCompletedTransactions(productId => { /* hey product "productid" restored */ });
    }

    public static void PurchaseConsumableItem(IAPItem item, Action<bool, string> completionHandler)
    {
        //InitializeScript.debugger += "PurchaseConsumableItem method called.\n";
        var forAndroid = true;
        #if UNITY_IOS
                forAndroid = false;
        #endif
        //InitializeScript.debugger += "Item identifier: " + item.ToItemString(forAndroid) + "\nThe request sent to prime 31 plugin.\n";
        IAP.purchaseConsumableProduct(item.ToItemString(forAndroid), completionHandler);
    }

    /// <summary>
    /// for removing AdMob , this is good
    /// </summary>
    /// <param name="item">Item.</param>
    public static void PurchaseNonconsumableItem(IAPItem item, Action<bool, string> completionHandler)
    {
        var forAndroid = true;
        #if UNITY_IOS
            forAndroid = false;
        #endif
        IAP.purchaseNonconsumableProduct(item.ToItemString(forAndroid), completionHandler);
    }

    // ------------------- Helpers ---------------

}