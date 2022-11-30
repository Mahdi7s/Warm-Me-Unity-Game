using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chapter3LevelScript : ChapterLevelScript
{
    void Awake()
    {
        GameState.Chapter = Chapter = 3;
        EmbededMobileBackButtonScript.lastScene = "Levels3";
        lockDic.Add(PurchasedItems.MolotovCocktails, true);
        lockDic.Add(PurchasedItems.MouseTrap, true);
        lockDic.Add(PurchasedItems.MagicWand, true);
        lockDic.Add(PurchasedItems.Freezer, true);
        lockDic.Add(PurchasedItems.Combo, false);
        lockDic.Add(PurchasedItems.BoxOfMatches, false);
        lockDic.Add(PurchasedItems.GoldenEgg, false);
        lockDic.Add(PurchasedItems.MutipleMatch, true);
        lockDic.Add(PurchasedItems.Wrench, false);
        hatItemsLockDic.Add(HatItemKind.CigaretteBlue, true);
        hatItemsLockDic.Add(HatItemKind.CigaretteGreen, false);
        hatItemsLockDic.Add(HatItemKind.CigaretteYellow, false);
        hatItemsLockDic.Add(HatItemKind.EggBronze, true);
        hatItemsLockDic.Add(HatItemKind.EggGold, true);
        hatItemsLockDic.Add(HatItemKind.IceGreen, false);
        hatItemsLockDic.Add(HatItemKind.IceRed, false);
        hatItemsLockDic.Add(HatItemKind.IceYellow, true);
        hatItemsLockDic.Add(HatItemKind.MatchBlue, false);
        hatItemsLockDic.Add(HatItemKind.MatchGreen, false);
        hatItemsLockDic.Add(HatItemKind.MatchPink, false);
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