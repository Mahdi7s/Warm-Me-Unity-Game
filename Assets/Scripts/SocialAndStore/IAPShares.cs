using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum IAPItem
{
    RemoveAdsx99c, 
    CoinPack5000x99c,
    CoinPack12000x199c,
    CoinPack19000x299c,
    CoinPack35000x399c
}

public static class IAPShares
{
    public static string[] GetListOfItems(bool forAndroid)
    {
        var retval = new List<string>();
        foreach (IAPItem item in Enum.GetValues(typeof(IAPItem)))
        {
            retval.Add(item.ToItemString(forAndroid));
        }
        return retval.ToArray();
    }
}