using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class PurchaseItemExtensions
{
    public static int GetItemCoinPrice(this PurchasedItems item)
    {
        int coinPrice = 0;
        switch (item)
        {
            case PurchasedItems.BoxOfMatches:
                coinPrice = 150;
                break;
            case PurchasedItems.MagicWand:
                coinPrice = 300;//***
                break;
            case PurchasedItems.Freezer:
                coinPrice = 300;
                break;
            case PurchasedItems.GoldenEgg:
                coinPrice = 100;
                break;
            case PurchasedItems.MolotovCocktails:
                coinPrice = 400;
                break;
            case PurchasedItems.MouseTrap:
                coinPrice = 300; //***
                break;
            case PurchasedItems.MutipleMatch:
                coinPrice = 500;
                break;
            case PurchasedItems.Wrench:
                coinPrice = 500;
                break;
            case PurchasedItems.Combo:
                coinPrice = 200;
                break;
        }
        return coinPrice;
    }
}

public class StoreManager : MonoBehaviour
{
    public SpriteRenderer SelectedItemPicture = null;
    public SpriteRenderer SelectedItemDescription = null;
    public SpriteRenderer itemName;
    public GUINumberScript Coins = null;
    public GUINumberScript CoinsForPurchase = null;
    public GUINumberScript itemPrice = null;
    public TVScript tV;
    public GameObject tabsPanel;
    public GameObject TabItemsContainer = null;
    public GameObject ShopPanel = null;
    public GameObject BtnInc = null;
    public GameObject BtnDec = null;
    public GameObject BtnShop = null;
    public GameObject BtnPlay = null;
    public GameObject BtnBack = null;
    public GameObject CoinPack1 = null;
    public GameObject CoinPack2 = null;
    public GameObject CoinPack3 = null;
    public GameObject CoinPack4 = null;
    public ChapterLevelScript ChapterLevelScr = null;
    public SoundScript levelSound;
    public bool IsTest = true;

    private TabItemScript _selectedTab = null;
    private PublishDestination publishDestination;
    private List<TabItemScript> _tabItems = null;
    //private Animator _imageBoardAnim = null;
    private Animator _shopPanelAnim = null;
    private Animator _storePanelAnim = null;
    private SpriteRenderer descriptionRenderer;
    private bool _isFirstInAnim = true;

    private int _storeCoins = 0;

    private string _err = "No Errrorrr";

    void Start()
    {
        _storeCoins = GameState.GetStoreCoins();

        IAPManager.Init();

        descriptionRenderer = SelectedItemDescription.GetComponent<SpriteRenderer>();
        publishDestination = GameSettings.publishDestination;
        _tabItems = TabItemsContainer.GetComponentsInChildren<TabItemScript>().ToList();
        //_imageBoardAnim = SelectedItemPicture.GetComponentInParent<Animator>();
        _shopPanelAnim = ShopPanel.GetComponent<Animator>();
        _storePanelAnim = GetComponent<Animator>();

        OnSelectionChanged(_tabItems.First().gameObject);
        OnCoinChange();
        SetupStore();
        levelSound.Mute();
    }

    private void SetupStore()
    {
        /*GameObject temp;
        Vector2 tabSize = GUIUtility.GetSpriteSize(tabItem.GetComponentInChildren<SpriteRenderer>().sprite);
        Vector3 position;
        int cntr = 0;
        position = tabItem.transform.position;*/
        foreach (TabItemScript tis in _tabItems)
        {
            if (GameState.LastFailPurchasedItems != null && !GameState.LastFailPurchasedItems.Any())
            {
                var purchasedBefore = GameState.LastFailPurchasedItems.Select(x => x.Key).FirstOrDefault(x => x == tis.Kind);
                if (purchasedBefore != null && GameState.LastFailPurchasedItems.ContainsKey(purchasedBefore))
                {
                    tis.Count.Number = GameState.LastFailPurchasedItems [purchasedBefore];
                }
            } else
            {
                _tabItems.ForEach(x => x.Count.Number = 0);
            }
        }
    }

    void Update()
    {
        try
        {
            audio.mute = !GameState.AudioFx;
            if (_isFirstInAnim)
            {
                GameState.Pause();
                _storePanelAnim.SetTrigger("In");

                _isFirstInAnim = false;
            }

            Collider2D overlapColl = null;
            if (Input.GetMouseButtonUp(0))
            {
                overlapColl = GetCollObject(Input.mousePosition);
            }

            if (overlapColl != null)
            {
                if (overlapColl.gameObject == BtnInc && IsButtonEnabled(BtnInc))
                {
                    var itemCount = _selectedTab.Count.Number;
                    if (itemCount < _selectedTab.MaxItemsCount)
                    {
                        IncreaseSelectedCount(1);
                        ++itemCount;
                        if (itemCount >= _selectedTab.MaxItemsCount)
                        {
                            EnableButton(BtnInc, false);
                        }
                    }
                }
                else if (overlapColl.gameObject == BtnDec && IsButtonEnabled(BtnDec))
                {
                    var itemCount = _selectedTab.Count.Number;
                    if (itemCount > 0)
                    {
                        IncreaseSelectedCount(-1);
                    }
                    else
                    {
                        EnableButton(BtnDec, false);
                    }
                }
                else if (overlapColl.gameObject == BtnShop)
                {
                    ShowPurchasePopup();
                }
                else if (overlapColl.gameObject.transform.parent.gameObject == BtnPlay)
                {
                    StartGameLevel();
                    GameState.Resume();
                    audio.Stop();
                    levelSound.Unmute();
                }
                else if (overlapColl.gameObject.name.StartsWith("CoinShopButton"))
                {
                    if (overlapColl.gameObject == CoinPack1)
                    {
                        DoPurchase(IAPItem.CoinPack5000x99c);
                    }
                    else if (overlapColl.gameObject == CoinPack2)
                    {
                        DoPurchase(IAPItem.CoinPack12000x199c);
                    }
                    else if (overlapColl.gameObject == CoinPack3)
                    {
                        DoPurchase(IAPItem.CoinPack19000x299c);
                    }
                    else if (overlapColl.gameObject == CoinPack4)
                    {
                        DoPurchase(IAPItem.CoinPack35000x399c);
                    }
                }
                else if (overlapColl.gameObject.name.StartsWith("TabItem"))
                {
                    var tabItem = overlapColl.gameObject;
                    if (!tabItem.GetComponent<TabItemScript>().isLocked)
                    {
                        OnSelectionChanged(tabItem.gameObject);
                    }
                }
            }
        }
        catch (UnityException ex)
        {
            _err += ex.Message;
        }
    }

    //void OnGUI(){
    //    GUI.color = Color.black;
    //    GUI.contentColor = Color.black;
    //    GUI.skin.label.fontSize = 24;

    //    GUI.Label(new Rect(20, 20, 300, 400), _err);
   // }

    public void OnSelectionChanged(GameObject itemToSelect)
    {
        var selItem = itemToSelect.GetComponent<TabItemScript>();
        if (_selectedTab != selItem)
        {
            _selectedTab = selItem;
            itemPrice.Number = _selectedTab.Kind.GetItemCoinPrice();
            itemName.sprite = _selectedTab.Title;
            _tabItems.ForEach(x => x.IsSelected = (_selectedTab.GetInstanceID() == x.GetInstanceID()));
            descriptionRenderer.sprite = _selectedTab.Description;
            tV.ShowItem(_selectedTab.Kind);
            OnCoinChange();
        }
    }

    public void ShowPurchasePopup()
    {
        DisableAllColliders();
        GObjUtility.DisableContent(_shopPanelAnim.gameObject, false);
        _shopPanelAnim.SetTrigger("Show");
        BtnPlay.SetActive(false);
        BtnBack.SetActive(false);
    }

    public IEnumerator HidePurchasePopup(IAPItem purchasedItem)
    {
        var purchasedCoins = purchasedItem.GetPrice();
        var total = GameState.GetStoreCoins() + purchasedCoins;
        _storeCoins += purchasedCoins;
        GameState.ChangeStoreCoins(total);
        OnCoinChange();
        _shopPanelAnim.SetTrigger("Hide");
        yield return new WaitForSeconds(2.0f);
        EnableAllColliders();
        GObjUtility.DisableContent(_shopPanelAnim.gameObject, true);

        BtnPlay.SetActive(true);
        BtnBack.SetActive(true);
    }

    public void DoPurchase(IAPItem itemToPurchase)
    {
        // show indicator ...
        if (IsTest)
        {
            _storeCoins += 1000;
            OnCoinChange();
            //_shopPanelAnim.SetTrigger("Hide");
        } else
        {
            switch (publishDestination)
            {
                case (PublishDestination.GooglePlayAndAppStore):
                case (PublishDestination.Bazaar):
                    IAPManager.PurchaseConsumableItem(itemToPurchase, (succeeded, error) =>
                    {
                        if (!succeeded)
                        {
                            Debug.LogError(error);
                        } else
                        {
                            StartCoroutine("HidePurchasePopup", itemToPurchase);
                            //HidePurchasePopup(itemToPurchase);
                        }
                    });
                    break;
            }
        }
    }

    // -------------------------- Privates -------------------------

    private void StartGameLevel()
    {
        GameState.ChangeStoreCoins(_storeCoins);

        GameState.CurrentLevelPurchases = new Dictionary<PurchasedItems, int>();
        GameState.CurrentLevelPurchases = _tabItems.Where(x => x.Count.Number > 0).ToDictionary(x => x.Kind, x => x.Count.Number);

        //Start Level
        ChapterLevelScr.StartLevel();

        if (_shopPanelAnim.GetCurrentAnimatorStateInfo(0).IsName("CoinBoardIn"))
        {
            _shopPanelAnim.SetTrigger("Hide");
        }
        _storePanelAnim.SetTrigger("Out");

        FindObjectOfType<TutShowerScript>().anim.SetTrigger("Out");
        GameState.IsPlaying = true;
    }

    private void IncreaseSelectedCount(int inc)
    {
        var totalCoins = GameState.GetAllCoinsWithoutStore() + _storeCoins;
        var remaningCoins = 0;
        var itemCoinPrice = _selectedTab.Kind.GetItemCoinPrice();
        if ((remaningCoins = totalCoins - (int)(Mathf.Sign(inc) * itemCoinPrice)) >= 0)
        {
            _selectedTab.Count.Number = _selectedTab.Count.Number + inc;
            //var storeCoins = GameState.GetStoreCoins();
            //GameState.ChangeStoreCoins(storeCoins - (totalCoins - remaningCoins));
            _storeCoins = _storeCoins - (totalCoins - remaningCoins);
            OnCoinChange();
        } else
        {
            // play not enough money anim of coins button
        }
    }

    private void OnCoinChange() // use for tab change so
    {
        var remainingCoins = GameState.GetAllCoinsWithoutStore() + _storeCoins;
        var selItemCoinPrice = _selectedTab.Kind.GetItemCoinPrice();

        EnableButton(BtnInc, true);
        EnableButton(BtnDec, true);

        if (remainingCoins < selItemCoinPrice)
        {
            EnableButton(BtnInc, false);
        }

        if (_selectedTab != null && _selectedTab.Count != null && _selectedTab.Count.Number <= 0)
        {
            EnableButton(BtnDec, false);
        }

        Coins.Number = remainingCoins;
        CoinsForPurchase.Number = remainingCoins;
    }

    private void EnableButton(GameObject btn, bool enable)
    {
        var btnEnable = btn.transform.FindChild("BtnEnabled").gameObject;
        var btnDisable = btn.transform.FindChild("BtnDisabled").gameObject;

        btnEnable.SetActive(enable);
        btnDisable.SetActive(!enable);
    }

    private bool IsButtonEnabled(GameObject btn)
    {
        return btn.transform.FindChild("BtnEnabled").gameObject.activeSelf;
    }

    private Collider2D GetCollObject(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);

        var overlapColl = Physics2D.OverlapPoint(touchPos);

        return overlapColl;
    }

    private void DisableAllColliders()
    {
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
    }

    public void EnableAllColliders()
    {
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = true;
        }
    }

    public void ResetStorePurchases()
    {
        _storeCoins = 0;
    }

    public void DistroyObject()
    {
        List<Transform> allTransforms = GObjUtility.GetTransformProgeny(transform);
        foreach (Transform t in allTransforms)
        {
            Destroy(t.gameObject);
        }
    }
}