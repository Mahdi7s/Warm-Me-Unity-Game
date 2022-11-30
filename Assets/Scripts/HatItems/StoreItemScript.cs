using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreItemScript : HatItemScript
{
    #region Definitions
    public static MouseHouseContainerScript mhcs;
    public static Transform mouseTrap;

    [Header("Purchasable Only")]
    public PurchasedItems piKind;
    public GameObject runner;
    [Tooltip("Wrench Only")]
    public Vector2 itemRelativePosition;    // Works only for wrench item.
    [Tooltip("MagicWand, MatchBox, Freezer Only")]
    public float itemEffectivenessTime;     // Works for changer, box of matches and freezer items.
    [Tooltip("Freezer Only")]
    public float timeScale = 1.00f;         // Works only for freezer item.
    public int comboCoefficient;
    public GameObject[] destroyableParticles;
    public int[] goldenEggScoreArray;
    public static Dictionary<PurchasedItems, int> dict=new Dictionary<PurchasedItems,int>(); // just for debugging, you can remove it, but be ware of usages

    private bool isHitted;                  // Works only for, box of matches changer and freezer items.
    private bool isReady;                   // Works only for wrench and mouse items.
    private PipeScript wrenchPipe;          // Works only for wrench item.
    #endregion

    protected override void Start()
    {
        base.Start();
        if (piKind == PurchasedItems.GoldenEgg)
        {
            elapsedTime = 0.5f/animatorSpeedChanger.speed;
            //guiScript = GetComponentInChildren<GUINumberScript>();
            //guiScript.Hide();
            //customAnim = guiScript.GetComponentInChildren<Animator>();
        }
        isHitted = false;
        if (guiScript != null)
        {
            guiScript.Hide();
        }

        if (dict.ContainsKey(piKind))
        {
            dict[piKind]++;
        }
        else
        {
            dict.Add(piKind, 0);
        }
    }
    protected override void PUpdate()
    {
        base.PUpdate();
        if (reaction == 3)                  // if item is a golden egg
        {                                   // same procedure in HatItemScript, see there for more info.
            eggsReactionDelay -= deltaTime;
            if (eggsReactionDelay < 0.0f)
            {
                guiScript.Number = goldenEggScoreArray[Random.Range(0, goldenEggScoreArray.Length)];
                //guiScript.Show();
                hat.EnqueueCoin(guiScript.Number);
                GameState.LevelCoins += (guiScript.Number * GameState.Combo);
                reaction = 0;
            }
        }
    }
    public override void Hit()
    {
        base.Hit();
        if (_playSpecial && !isHitted)
        {
            switch (piKind)
            {
                case PurchasedItems.Wrench:
                    PipeScript ps = GameState.DequeueDamagedPipe();
                    if (ps != null)
                    {
                        GameObject WrenchRunner = (GameObject)Instantiate(runner, Vector3.zero, Quaternion.identity);
                        WrenchRunnerScript wrs = WrenchRunner.GetComponent<WrenchRunnerScript>();
                        wrs.Run(ps);
                    }
                    break;
                case PurchasedItems.Freezer:
                    foreach (GameObject gobj in destroyableParticles)
                    {
                        gobj.GetComponent<ParticleSystem>().Stop();
                    }
                    GameObject freezerRunner = (GameObject)Instantiate(runner, Vector3.zero, Quaternion.identity);
                    FreezerRunnerScript frs = freezerRunner.GetComponent<FreezerRunnerScript>();
                    frs.Run();
                    break;
                case PurchasedItems.MagicWand:
                    GameObject magicWandRunner = (GameObject)Instantiate(runner, Vector3.zero, Quaternion.identity);
                    MagicWandRunnerScript mwrs = magicWandRunner.GetComponent<MagicWandRunnerScript>();
                    mwrs.Run(hat);
                    break;
                case PurchasedItems.MouseTrap:
                    foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
                    {
                        particleSystem.Play();
                    }
                    mhcs.KillAMouse();
                    break;
                case PurchasedItems.MolotovCocktails:
                    foreach (GameObject go in destroyableParticles)
                    {
                        go.GetComponent<ParticleSystem>().Stop();
                    }
                    break;
                case PurchasedItems.BoxOfMatches:
                    GameObject matchBoxRunner = (GameObject)Instantiate(runner, Vector3.zero, Quaternion.identity);
                    MatchBoxRunnerScript mbrs = matchBoxRunner.GetComponent<MatchBoxRunnerScript>();
                    mbrs.Run(hat);
                    break;
                case PurchasedItems.Combo:
                    GameObject comboRunner = (GameObject)Instantiate(runner, Vector3.zero, Quaternion.identity);
                    ComboRunnerScript crs = comboRunner.GetComponent<ComboRunnerScript>();
                    crs.Run();
                    break;
                case PurchasedItems.GoldenEgg:
                    reaction = 3;
                    //guiScript.gameObject.SetActive(true);
                    guiScript.Number = goldenEggScoreArray [Random.Range(0, goldenEggScoreArray.Length)];
                    //guiScript.Show();
                    //guiScript.transform.parent = Camera.main.transform;
                    break;
            }
            isHitted = true;
        }
    }
}