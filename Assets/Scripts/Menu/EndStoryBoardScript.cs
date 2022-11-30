using UnityEngine;
using System.Collections;

public class EndStoryBoardScript : MonoBehaviour {

    private Animator anim;
	void Start ()
    {
        EmbededMobileBackButtonScript.lastScene = "Levels1";            // This is for android back button, do not remove it
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("End"))
        {
            anim.speed = 0f;
            anim.enabled = false;
            GameState.OpenLevel(1, 1);
        }
	}
}
