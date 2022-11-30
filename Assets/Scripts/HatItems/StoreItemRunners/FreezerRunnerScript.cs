using UnityEngine;
using System.Collections;

public class FreezerRunnerScript : MonoBehaviour
{
    public float effectiveTime;
    public float timeScale;
    private bool timerOn = false;

    void Update()
    {
        if (timerOn)
        {
            effectiveTime -= Time.deltaTime;
            if (effectiveTime <= 0.0f)
            {
                Time.timeScale = 1.0f;
                timerOn = false;
                Destroy(gameObject);
            }
        }
    }

    public void Run()
    {
        if (effectiveTime > 0f)
        {
            timerOn = true;
            Time.timeScale = timeScale;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}