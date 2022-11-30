using UnityEngine;
using System.Collections;

public class TotalCoinsScript : MonoBehaviour {

    public GUINumberScript Number = null;

	// Use this for initialization
	void Start () {
        OnTotalCoinsChange();
	}

    public void OnTotalCoinsChange()
    {
        Number.Number = GameState.GetAllCoins();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
