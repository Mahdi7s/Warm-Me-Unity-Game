using UnityEngine;
using System.Collections;

public class BtnUnlockScript : MonoBehaviour {
    public Animator ShopAnim = null;
    private Animator _anim = null;    
    public GUINumberScript UnlockPrice = null;
    public TotalCoinsScript TotalCoins = null;

    public ChLockStateScript LockStateScr {get;set;}

	void Start () {
        _anim = transform.parent.GetComponent<Animator>();        
	}
	
	void Update () {
        if (TouchUtility.GetTouchedCollider() == collider2D)
        {
            var allCoins = GameState.GetAllCoins();
            if (UnlockPrice.Number > allCoins)
            {
                ShopAnim.SetTrigger("Show");
            }
            else
            {
                GameState.ChangeStoreCoins(GameState.GetStoreCoins() - UnlockPrice.Number);
                LockStateScr.Unlock();
                TotalCoins.OnTotalCoinsChange();
                FindObjectOfType<UnlockEnabilityScript>().EnableSlide();
                _anim.SetTrigger("Out");
            }
        }
	}
}
