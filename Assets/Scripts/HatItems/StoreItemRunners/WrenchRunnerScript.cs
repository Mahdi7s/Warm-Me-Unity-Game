using UnityEngine;
using System.Collections;

public class WrenchRunnerScript : MonoBehaviour
{
    public void Run(PipeScript pipeScript)
    {
        transform.GetChild(0).transform.position = Vector3.zero;
        transform.parent = pipeScript.transform.parent;
        transform.rotation = pipeScript.transform.rotation;
        transform.position = pipeScript.transform.position;
        transform.localScale = Vector3.one;
        pipeScript.RepairPipe();
        audio.Play();
        Destroy(gameObject, 2.0f);
    }
    void Update()
    {
        audio.mute = !GameState.AudioFx;
        audio.pitch = Time.timeScale;
    }
}
