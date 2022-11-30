using UnityEngine;
using System.Collections;

public class ComboRunnerScript : MonoBehaviour
{
    public float effectiveTime;
    public int comboCoefficient;
    public ParticleSystem comboParticle;
    private bool timerOn = false;
    private int coefficient = 1;

    void Update()
    {
        if (timerOn)
        {
            effectiveTime -= Time.deltaTime;
            if (effectiveTime > 0)
            {
                if (!comboParticle.isPlaying)
                {
                    comboParticle.Play();
                }
            }
            else
            {
                comboParticle.Stop();
                GameState.Combo /= coefficient;
                timerOn = false;
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void Run()
    {
        if (comboCoefficient > 1 && effectiveTime > 0.0f)
        {
            timerOn = true;
            coefficient = comboCoefficient;
            GameState.Combo *= coefficient;
        } else
        {
            Destroy(gameObject);
        }
    }
}