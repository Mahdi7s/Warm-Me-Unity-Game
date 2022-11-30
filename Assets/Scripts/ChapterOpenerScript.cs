using UnityEngine;
using System.Collections;
using System.Linq;

public class ChapterOpenerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (var i = 0; i < 6; i++)
        {
            for (var j = 0; j < 9; j++)
            {
                GameState.OpenLevelIfNotOpened(i + 1, j + 1);
            }
        }
        GameState.LevelModelList.Last().LevelCoins = 1000;
        GameState.LevelModelList.Last().Win = true ;
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
