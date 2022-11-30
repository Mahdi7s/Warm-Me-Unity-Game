using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDelayScript : MonoBehaviour
{
   
    public float MinDelay = 1.0f;
    public float MaxDelay = 6.0f;
    // Use this for initialization
    IEnumerator Start()
    {
        particleSystem.Stop();
        yield return new WaitForSeconds(Random.Range(MinDelay, MaxDelay));
        particleSystem.Play();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
