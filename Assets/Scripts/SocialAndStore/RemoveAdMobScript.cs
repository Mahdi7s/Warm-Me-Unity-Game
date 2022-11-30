using UnityEngine;
using System.Collections;

public class RemoveAdMobScript : MonoBehaviour
{
    public bool isTest;
    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (GameState.AdMobEnabled)
        {
            IAPManager.Init();
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.AdMobEnabled)
        {
            var touchedColl = TouchUtility.GetTouchedCollider();
            if (touchedColl == collider2D)
            {
                if (isTest && GameState.AdMobEnabled)
                {
                    GameState.AdMobEnabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else if (GameState.AdMobEnabled)
                {
                    IAPManager.PurchaseNonconsumableItem(IAPItem.RemoveAdsx99c, (succeeded, error) =>
                    {
                        if (!succeeded)
                        {
                            Debug.LogError(error);
                        }
                        else
                        {
                            GameState.AdMobEnabled = false;
                            spriteRenderer.enabled = GameState.AdMobEnabled = false;
                            AdadPlugin.DestroyBanner();
                        }
                    });
                }
            }
        }
    }
}
