using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public enum HatItemKind
{
    None,
    
    // Matches
    MatchYellow,
    MatchBlue,
    MatchGreen,
    MatchPink,
    MatchRed,
    
    // Ice Cream
    IceYellow,
    IceGreen,
    IceRed,
    
    // Cigarette
    CigaretteYellow,
    CigaretteGreen,
    CigaretteBlue,
    
    // Egg
    EggBronze,
    EggGold,
    
    StoreItem,
    Letter
}

public static class HatItemKindExtensions
{
    public static readonly HatItemKind[] PositiveItems = new HatItemKind[]
    {
        HatItemKind.StoreItem,
        HatItemKind.MatchYellow,
        HatItemKind.MatchBlue,
        HatItemKind.MatchGreen,
        HatItemKind.MatchPink,
        HatItemKind.MatchRed,
        HatItemKind.EggBronze,
        HatItemKind.Letter
    };

    public static readonly PurchasedItems[] RunnerItems = new PurchasedItems[]
    {
        PurchasedItems.BoxOfMatches,
        PurchasedItems.Combo,
        PurchasedItems.Freezer,
        PurchasedItems.MagicWand,
        PurchasedItems.Wrench
    };
    
    public static bool IsPositiveItem(this HatItemKind kind)
    {
        return PositiveItems.Any(x => x == kind);
    }

    public static bool IsRunnerItem(this PurchasedItems kind)
    {
        return RunnerItems.Any(x => x == kind);
    }

    public static HatItemKind GetRandomPositive(this HatItemKind kind)
    {
        return PositiveItems [Random.Range(1, PositiveItems.Length)];
    }

    public static bool GetItemEnum(this HatItemKind kind, string kindStr, out HatItemKind itemKind, out PurchasedItems piKind)
    {
        if (IsFreeItem(HatItemKind.None, kindStr))
        {
            piKind = PurchasedItems.None;
            itemKind = (HatItemKind)Enum.Parse(typeof(HatItemKind), kindStr.Replace(ChapterLevelScript.HatItemKindPref, string.Empty));
        } else if (IsStoreItem(HatItemKind.None, kindStr))
        {
            itemKind = HatItemKind.None;
            piKind = (PurchasedItems)Enum.Parse(typeof(PurchasedItems), kindStr.Replace(ChapterLevelScript.HatItemKindPref, string.Empty));
        } else
        {
            piKind = PurchasedItems.None;
            itemKind = HatItemKind.None;
            return false;
        }
        return true;
    }

    public static bool IsFreeItem(this HatItemKind kind, string strKind)
    {
        return strKind.StartsWith(ChapterLevelScript.HatItemKindPref);
    }

    public static bool IsStoreItem(this HatItemKind kind, string strKind)
    {
        return strKind.StartsWith(ChapterLevelScript.StoreItemKindPref);
    }
}
