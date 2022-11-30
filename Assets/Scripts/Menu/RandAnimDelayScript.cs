using UnityEngine;
using System.Collections;

public class RandAnimDelayScript : AnimDelayScript {

    public float MinDelay = 1.0f;
    public float MaxDelay = 8.0f;   

	// Use this for initialization
	void Start () {
        DelaySeconds = Random.Range(MinDelay, MaxDelay);
        base.Start();
	}
}
