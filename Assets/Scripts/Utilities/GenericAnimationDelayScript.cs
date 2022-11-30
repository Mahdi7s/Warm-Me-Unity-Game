using UnityEngine;
using System.Collections;

public class GenericAnimationDelayScript : MonoBehaviour
{
    public float animationDelay;
    private Animator[] animators;
    private void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.enabled = false;
        }
    }

	void Update ()
    {
        animationDelay -= Time.deltaTime;
        if (animationDelay <= 0.0f)
        {
            foreach (Animator anim in animators)
            {
                anim.enabled = true;
                enabled = false;
            }
        }
	}
}