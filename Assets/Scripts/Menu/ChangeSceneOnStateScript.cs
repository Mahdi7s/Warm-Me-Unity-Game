using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class ChangeSceneOnStateScript : MonoBehaviour
{
    private Animator _anim = null;
    public string StateName = string.Empty;
    public string SceneName = string.Empty;
    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName(StateName))
        {
            GameState.LoadMenu(SceneName);
        }
    }
}