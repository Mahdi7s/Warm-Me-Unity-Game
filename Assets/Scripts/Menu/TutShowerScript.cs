using UnityEngine;
using System.Collections;

public class TutShowerScript : MonoBehaviour {
    public Animator anim = null;
	// Use this for initialization
	IEnumerator Start () {
       yield return new WaitForSeconds(0.5f);
       var lvlModel =  GameState.GetLastPlayedLevel();
       if (lvlModel.Chapter == 1 && lvlModel.Level <= 1)
       {
           anim.SetTrigger("In");
       }
       yield return new WaitForSeconds(0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
