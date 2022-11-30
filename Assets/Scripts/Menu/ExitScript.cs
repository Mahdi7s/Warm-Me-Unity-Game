using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{
    public Animator quitBoardAnim;

    // Use this for initialization
    void Start()
    {
        EmbededMobileBackButtonScript.lastScene = null;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitBoardAnim.SetTrigger("In");
        }
    }
}
