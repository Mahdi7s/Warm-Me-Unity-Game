using UnityEngine;
using System.Collections;

public enum SlothState
{
    Cold,
    Hot,
    Normal
}

public class SlothScript : PausableBehaviour
{
    public ParticleSystem splashBubble;
    public AudioClip slothJump, slothCold, slothHot;
    Animator anim;
    SlothState slothState;
    AnimatorStateInfo state, state2;

    [Range(0, 100)]
    public int ColdTempraturePercentage = 20, HotTempraturePercentage = 80;

    private int hotSlothTemperature, coldSlothTemperature;

    void Start()
    {
        slothState = SlothState.Cold;
        anim = GetComponent<Animator>();

        var chScr = Camera.main.GetComponent<ChapterLevelScript>();
        coldSlothTemperature = chScr.MaxTemprature * ColdTempraturePercentage / 100;
        hotSlothTemperature = chScr.MaxTemprature * HotTempraturePercentage / 100;
    }

    protected override void PUpdate()
    {
        audio.mute = !GameState.AudioFx;
        state = anim.GetCurrentAnimatorStateInfo(0);
        if (!(state.IsName("PreHot") || state.IsName("HotToNormal") || state.IsName("PreCold") || state.IsName("ColdToNormal")))
        {
            if (GameState.GetTemperature() >= hotSlothTemperature)
            {
                if (slothState == SlothState.Cold)
                {
                    anim.SetTrigger("Normal");
                    anim.ResetTrigger("Cold");
                    anim.ResetTrigger("Hot");
                    slothState = SlothState.Normal;
                }
                else
                {
                    anim.SetTrigger("Hot");
                    if (slothHot != null)
                    {
                        audio.clip = slothHot;
                        audio.Play();
                    }
                    anim.ResetTrigger("Normal");
                    anim.ResetTrigger("Cold");
                    slothState = SlothState.Hot;
                }
            }
            else if (GameState.GetTemperature() <= coldSlothTemperature)
            {
                if (slothState == SlothState.Hot)
                {
                    anim.SetTrigger("Normal");
                    anim.ResetTrigger("Cold");
                    anim.ResetTrigger("Hot");
                    slothState = SlothState.Normal;
                }
                else
                {
                    anim.SetTrigger("Cold");
                    if (slothCold != null)
                    {
                        audio.clip = slothCold;
                        audio.Play();
                    }
                    anim.ResetTrigger("Normal");
                    anim.ResetTrigger("Hot");
                    slothState = SlothState.Cold;
                }
            }
            else
            {
                anim.SetTrigger("Normal");
                anim.ResetTrigger("Cold");
                anim.ResetTrigger("Hot");
                slothState = SlothState.Normal;
            }
        }
    }

    protected override void PFixedUpdate()
    {
    }

    protected override void POnGUI()
    {

    }

    public void Upsidedown()
    {
        state2 = anim.GetCurrentAnimatorStateInfo(0);
        if (!(state2.IsName("Upsidedown") || state2.IsName("Hot - Upsidedown") || state2.IsName("Cold - Upsidedown") ||
            state2.IsName("PreHot") || state2.IsName("HotToNormal") || state2.IsName("PreCold") || state2.IsName("ColdToNormal")))
        {
            anim.SetTrigger("Upsidedown");
            splashBubble.Play();
            if (slothJump != null)
            {
                audio.clip = slothJump;
                audio.Play();
            }
        }
    }
}