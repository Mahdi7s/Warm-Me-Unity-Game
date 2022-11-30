using UnityEngine;
using System.Collections;

public class GotoChapterScript : MonoBehaviour {

    public string GoToScene="";
    public bool canGo=false;
    public ChLockStateScript chlockscr=null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (TouchUtility.GetTouchedCollider() == collider2D ) 
        {
            if(canGo ){
            SlideScript.defaultChapterNumber = int.Parse(GoToScene.Substring(GoToScene.Length - 1, 1));
            GameState.LoadMenu(GoToScene);
            }
            else if(chlockscr!=null && chlockscr.IsLocked){
                chlockscr.ShowBoard();
            }
        }
	}
}
