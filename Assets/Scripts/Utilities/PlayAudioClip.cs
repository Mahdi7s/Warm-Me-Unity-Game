using UnityEngine;
using System.Collections;

public class PlayAudioClip : MonoBehaviour
{
    public void PlaySFX(AudioClip src)
    {
        if (GameState.AudioFx)
            AudioSource.PlayClipAtPoint(src, Vector3.zero);
    }
}
