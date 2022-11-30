using UnityEngine;
using System.Collections;
using System;

public class FreeCoinScript : MonoBehaviour
{
    public Animator bushAnimator;
    [Range(0, 16)]
    public float HoursToNextChance = 8.0f;

    void Awake()
    {
        DateTime temp = GameState.LastFreeCoinChanceTime;
        temp = temp.AddHours(HoursToNextChance);
        if (temp > DateTime.Now)
        {
            gameObject.SetActive(false);
            bushAnimator.enabled = false;
            bushAnimator.GetComponent<AnimDelayScript>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
