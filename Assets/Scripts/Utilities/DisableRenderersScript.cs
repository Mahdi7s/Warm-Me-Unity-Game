using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class DisableRenderersScript : MonoBehaviour {

    public bool Disable = false;
    public bool DisableByDefault = true;
    public string TriggerName = string.Empty;

    private bool _renderersDisabled = false;
    private Renderer[] _renderers = null;
    private Transform[] _children = null;
    private Animator _anim = null;

	// Use this for initialization
	void Start () {
        _renderers = GetComponentsInChildren<Renderer>();
        _children = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        _anim = GetComponent<Animator>();
        _renderersDisabled = !Disable;

        if (Disable) DisableRenderers(DisableByDefault);
	}
	
	// Update is called once per frame
	void Update () {
	    if (_renderersDisabled != Disable)
        {
            if(_anim.GetBool(TriggerName)) {
                DisableRenderers(Disable);
                _renderersDisabled = Disable;
            }
        }
	}

    private void DisableRenderers(bool disable)
    {
        /*foreach (var rend in _children)
        {
            rend.gameObject.SetActive(!disable);
        }*/
    }
}
