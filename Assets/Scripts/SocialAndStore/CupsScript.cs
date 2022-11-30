using UnityEngine;
using System.Collections;

public class CupsScript : MonoBehaviour {
    public GameObject Bronze = null;
    public GameObject Silver = null;
    public GameObject Golden = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Animator a;

	}

    public void ShowCup(LevelModel lvlModel)
    {
        var cup = lvlModel.LevelCup == LevelCup.Bronze ? Bronze : 
            (lvlModel.LevelCup == LevelCup.Silver ? Silver : Golden);
        // animator & show cup...
    }
}
