using UnityEngine;
using System.Collections;

public enum PurchasedItems
{
    MouseTrap,
    MolotovCocktails,
    GoldenEgg,
    Wrench,
    Freezer,
    MagicWand,
    MutipleMatch,
    BoxOfMatches,
    Combo,
    None
}

public class TVScript : MonoBehaviour
{
    public GameObject tVMouseTrap, tVMolotovCocktails, tVGoldenEgg, tVWrench, tVFreezer, tVMagicWand, tVMutipleMatch, tVBoxOfMatches, tVCombo;
    private GameObject lastSelectedGameObject = null;

    void Awake()
    {
        SetParticlesLayer(tVBoxOfMatches);
        SetParticlesLayer(tVCombo);
        SetParticlesLayer(tVFreezer);
        SetParticlesLayer(tVGoldenEgg);
        SetParticlesLayer(tVMagicWand);
        SetParticlesLayer(tVMolotovCocktails);
        SetParticlesLayer(tVMouseTrap);
        SetParticlesLayer(tVMutipleMatch);
        SetParticlesLayer(tVWrench);

        SetItemActive(tVCombo, false);
        SetItemActive(tVFreezer, false);
        SetItemActive(tVGoldenEgg, false);
        SetItemActive(tVMagicWand, false);
        SetItemActive(tVMolotovCocktails, false);
        SetItemActive(tVMouseTrap, false);
        SetItemActive(tVMutipleMatch, false);
        SetItemActive(tVWrench, false);
    }

    public void ShowItem(PurchasedItems purchaseItem)
    {
        if (lastSelectedGameObject != null)
        {
            SetItemActive(lastSelectedGameObject, false);
        }
        switch (purchaseItem)
        {
            case PurchasedItems.BoxOfMatches:
                lastSelectedGameObject = tVBoxOfMatches;
                break;
            case PurchasedItems.Combo:
                lastSelectedGameObject = tVCombo;
                break;
            case PurchasedItems.Freezer:
                lastSelectedGameObject = tVFreezer;
                break;
            case PurchasedItems.GoldenEgg:
                lastSelectedGameObject = tVGoldenEgg;
                break;
            case PurchasedItems.MagicWand:
                lastSelectedGameObject = tVMagicWand;
                break;
            case PurchasedItems.MolotovCocktails:
                lastSelectedGameObject = tVMolotovCocktails;
                break;
            case PurchasedItems.MouseTrap:
                lastSelectedGameObject = tVMouseTrap;
                break;
            case PurchasedItems.MutipleMatch:
                lastSelectedGameObject = tVMutipleMatch;
                break;
            case PurchasedItems.Wrench:
                lastSelectedGameObject = tVWrench;
                break;
        }
        SetItemActive(lastSelectedGameObject, true);
    }

    private void SetParticlesLayer(GameObject gObj)
    {
        GameObject gobj = gObj.transform.GetChild(0).gameObject;
        foreach (ParticleSystem ps in gobj.GetComponentsInChildren<ParticleSystem>())
        {
            ps.renderer.sortingLayerName = gobj.renderer.sortingLayerName;
            ps.renderer.sortingOrder = gobj.renderer.sortingOrder + 1;
            //Debug.Log(ps.renderer.sortingLayerName + ps.renderer.sortingOrder);
        }
        SetItemActive(gobj, false);
    }

    private void SetItemActive(GameObject gobj, bool active)
    {
        var pos = gobj.transform.position;
        if (active)
        {
            gobj.transform.position = new Vector3(pos.x, pos.y, 0); 
        }
        else
        {
            gobj.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z - 1);
        }
        foreach (var ps in gobj.GetComponentsInChildren<ParticleSystem>())
        {
            if (active)
            {
                ps.Play();
            }
            else
            {
                ps.Stop();
            }
        }
    }
}