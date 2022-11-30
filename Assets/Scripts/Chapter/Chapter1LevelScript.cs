using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chapter1LevelScript : ChapterLevelScript
{
    void Awake()
    {
        GameState.Chapter = Chapter = 1;
        EmbededMobileBackButtonScript.lastScene = "Levels1";
        lockDic.Add(PurchasedItems.MolotovCocktails, true);
        lockDic.Add(PurchasedItems.MouseTrap, true);
        lockDic.Add(PurchasedItems.MagicWand, true);
        lockDic.Add(PurchasedItems.Freezer, true);
        lockDic.Add(PurchasedItems.Combo, true);
        lockDic.Add(PurchasedItems.BoxOfMatches, false);
        lockDic.Add(PurchasedItems.GoldenEgg, true);
        lockDic.Add(PurchasedItems.MutipleMatch, true);
        lockDic.Add(PurchasedItems.Wrench, true);
        hatItemsLockDic.Add(HatItemKind.CigaretteBlue, true);
        hatItemsLockDic.Add(HatItemKind.CigaretteGreen, true);
        hatItemsLockDic.Add(HatItemKind.CigaretteYellow, false);
        hatItemsLockDic.Add(HatItemKind.EggBronze, true);
        hatItemsLockDic.Add(HatItemKind.EggGold, true);
        hatItemsLockDic.Add(HatItemKind.IceGreen, true);
        hatItemsLockDic.Add(HatItemKind.IceRed, true);
        hatItemsLockDic.Add(HatItemKind.IceYellow, true);
        hatItemsLockDic.Add(HatItemKind.MatchBlue, false);
        hatItemsLockDic.Add(HatItemKind.MatchGreen, true);
        hatItemsLockDic.Add(HatItemKind.MatchPink, true);
        hatItemsLockDic.Add(HatItemKind.MatchRed, true);
        hatItemsLockDic.Add(HatItemKind.MatchYellow, false);
    }

    protected override void PUpdate()
    {
        base.PUpdate();
    }

    protected override void POnGUI()
    {
        base.POnGUI();
    }

    protected override void PFixedUpdate()
    {
        base.PFixedUpdate();
    }
}