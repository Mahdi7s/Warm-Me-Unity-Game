using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FreeCoinBoardScript : MonoBehaviour
{
    private bool _isClicked = false;

    public FreeCoinScript FreeCoinScr = null;
    public GameObject[] Pigs = new GameObject[5];
    public int[] CoinsAmount = new []{0, 25, 50, 100, 200};
    public float DelayToBreakAll = 2.5f;
    public GUINumberScript Hours = null;
    public GUINumberScript Coins = null;

    void Start()
    {

    }
	
    void Update()
    {
        if (!_isClicked)
        {
            StartCoroutine("CheckClick");
        }
    }

    IEnumerator CheckClick()
    {
        var collider = TouchUtility.GetTouchedCollider();
        if (collider != null)
        {
            var pig = Pigs.FirstOrDefault(x => x.collider2D == collider);
            if (pig != null)
            {
                _isClicked = true;
                var idx = -1;
                var idxList = Enumerable.Range(0, CoinsAmount.Count()).OrderBy(x => Guid.NewGuid()).ToArray();
                Action<GameObject> setCoin = gObj => {
                    var gNum = gObj.GetComponentInChildren<GUINumberScript>();
                    gNum.Number = CoinsAmount [idxList [++idx]];
                    gNum.gameObject.SetActive(true);
                };

                setCoin(pig);
                var pigAnim = pig.transform.parent.GetComponent<Animator>();
                pigAnim.SetTrigger("Click");
                yield return new WaitForSeconds(DelayToBreakAll);
                Coins.transform.parent.gameObject.SetActive(true);
                Coins.Number = CoinsAmount [idxList [idx]];
                GameState.ChangeStoreCoins(GameState.GetStoreCoins() + Coins.Number);
                Hours.Number = (int)FreeCoinScr.HoursToNextChance;
                GameState.LastFreeCoinChanceTime = DateTime.Now;



                foreach (var pitem in Pigs)
                {
                    if (pitem != pig)
                    {
                        yield return new WaitForSeconds(0.2f);
                        setCoin(pitem);
                        var panim = pitem.transform.parent.GetComponent<Animator>();
                        panim.SetTrigger("Click");
                    }
                }
            }
        }
        yield break;
    }
}
