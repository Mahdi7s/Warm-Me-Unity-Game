using UnityEngine;
using System.Collections;

public class Shop2ndScript : MonoBehaviour {
    public GameObject CoinPack1 = null;
    public GameObject CoinPack2 = null;
    public GameObject CoinPack3 = null;
    public GameObject CoinPack4 = null;

    public TotalCoinsScript TotalCoins = null;
    public bool IsTest = true;

    private Animator _anim = null;

    private PublishDestination publishDestination;

	void Start () {
        publishDestination = GameSettings.publishDestination;
        _anim = GetComponent<Animator>();
        IAPManager.Init();
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D overlapColl = null;
        if (Input.GetMouseButtonUp(0))
        {
            overlapColl = GetCollObject(Input.mousePosition);
        }

        if (overlapColl != null)
        {
            if (overlapColl.gameObject.name.StartsWith("CoinShopButton"))
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
        }
	}

    public void DoPurchase(IAPItem itemToPurchase)
    {
        // show indicator ...
        if (IsTest)
        {
            GameState.ChangeStoreCoins(GameState.GetStoreCoins() + 1000);
        }
        else
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
                        }
                        else
                        {
                            GameState.ChangeStoreCoins(GameState.GetStoreCoins() + itemToPurchase.GetPrice());
                        }
                        _anim.SetTrigger("Hide");
                    });
                    break;
            }
        }
        TotalCoins.OnTotalCoinsChange();
    }

    private Collider2D GetCollObject(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);

        var overlapColl = Physics2D.OverlapPoint(touchPos);

        return overlapColl;
    }
}
