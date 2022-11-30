using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimDelayScript : PausableBehaviour
{
    public float DelaySeconds = 0.0f;
    public Animator[] OtherSyncAnimators = null;
    //public bool ContinueAfterDelayReached = false;
    //public bool ResetOnEnable = false;

    private Animator _anim = null;
    private float _startDelay = 0.0f;

    // Use this for initialization
    public void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = false;
        foreach (var anim in OtherSyncAnimators)
        {
            anim.enabled = false;
        }

        PauseAnimations = false;
        PauseParticles = false;

        _startDelay = DelaySeconds;
    }

    /*
    void OnEnable()
    {
        _startDelay = DelaySeconds;
    }

    void OnDisable()
    {
        _startDelay = 0.0f;
    }*/
	
    protected override void PUpdate()
    {
        if ((_startDelay -= Time.deltaTime) <= 0.0f)
        {
            _anim.enabled = true;
            foreach (var anim in OtherSyncAnimators)
            {
                anim.enabled = true;
            }

            //enabled = false;
            /*
            if (ResetOnEnable && _startDelay <= 0.0f)
            {
                _startDelay = DelaySeconds;
            }
            enabled = ContinueAfterDelayReached;
            */
        }
    }

    protected override void PFixedUpdate()
    {

    }

    protected override void POnGUI()
    {

    }
}
