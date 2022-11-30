using UnityEngine;
using System.Collections;

public class BackButtonScript : MonoBehaviour
{
    public AudioClip hitSound;
    public string previousScene;

    private bool doClick = false;
    private Animator anim = null;
    public ParticleSystem ExplodeParticle = null;

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        audio.clip = hitSound;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        audio.mute = !GameState.AudioFx;
        var touchedColl = TouchUtility.GetTouchedCollider(false);
        if (touchedColl != null)
        {
            if (touchedColl == collider2D)
            {
                if (ExplodeParticle != null)
                {
                    ExplodeParticle.renderer.sortingLayerName = renderer.sortingLayerName;
                    ExplodeParticle.renderer.sortingOrder = renderer.sortingOrder;
                    ExplodeParticle.Play();
                }
                doClick = true;
                anim.SetTrigger("Click");
                audio.Play();
            }
        }

        var state = anim.GetCurrentAnimatorStateInfo(0);
        if (doClick && state.IsName("End"))
        {
            GameState.LoadMenu(previousScene);
            doClick = false;
        }
    }
}
