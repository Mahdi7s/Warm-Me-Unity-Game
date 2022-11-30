using UnityEngine;
using System.Collections;
using System.Linq;

public class SoundScript : MonoBehaviour
{
    public float Volume = 1.0f;
    private bool muted;
    public AudioClip[] Clips;
    public int[] Repeats;

    void Awake()
    {
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        audio.volume = Volume;
    }

    IEnumerator Start()
    {
        for (int i = 0; i < Clips.Length; i++)
        {
            var repeat = Repeats == null || Repeats.Length <= i ? 1 : Repeats[i];
            for (int j = 0; j < repeat; j++)
            {
                audio.clip = Clips[i];
                audio.Play();
                yield return new WaitForSeconds(Clips[i].length);
            }
        }
        yield break;
    }

    void Update()
    {
        if (!muted)                             // If the sound of this script was not disabled in code
        {
            audio.mute = !GameState.AudioFx;
        }
    }

    public void Mute()                          // This function will be called from Store Manager, because the SFX of store is diffrent form level.
    {
        audio.mute = muted = true;
    }

    public void Unmute()                        // This function will be called from Store Manager, because the SFX of store is diffrent form level.
    {
        audio.mute = muted = false;
    }
}
