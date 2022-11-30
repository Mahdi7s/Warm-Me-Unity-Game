using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chapter6LevelScript : ChapterLevelScript
{
    void Awake()
    {
        GameState.Chapter = Chapter = 6;
        EmbededMobileBackButtonScript.lastScene = "Levels6";
        lockDic.Add(PurchasedItems.MolotovCocktails, false);
        lockDic.Add(PurchasedItems.MouseTrap, false);
        lockDic.Add(PurchasedItems.MagicWand, false);
        lockDic.Add(PurchasedItems.Freezer, false);
        lockDic.Add(PurchasedItems.Combo, false);
        lockDic.Add(PurchasedItems.BoxOfMatches, false);
        lockDic.Add(PurchasedItems.GoldenEgg, false);
        lockDic.Add(PurchasedItems.MutipleMatch, false);
        lockDic.Add(PurchasedItems.Wrench, false);
        hatItemsLockDic.Add(HatItemKind.CigaretteBlue, false);
        hatItemsLockDic.Add(HatItemKind.CigaretteGreen, false);
        hatItemsLockDic.Add(HatItemKind.CigaretteYellow, false);
        hatItemsLockDic.Add(HatItemKind.EggBronze, false);
        hatItemsLockDic.Add(HatItemKind.EggGold, false);
        hatItemsLockDic.Add(HatItemKind.IceGreen, false);
        hatItemsLockDic.Add(HatItemKind.IceRed, false);
        hatItemsLockDic.Add(HatItemKind.IceYellow, false);
        hatItemsLockDic.Add(HatItemKind.MatchBlue, false);
        hatItemsLockDic.Add(HatItemKind.MatchGreen, false);
        hatItemsLockDic.Add(HatItemKind.MatchPink, false);
        hatItemsLockDic.Add(HatItemKind.MatchRed, false);
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