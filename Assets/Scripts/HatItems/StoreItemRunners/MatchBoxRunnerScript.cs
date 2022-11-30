using UnityEngine;
using System.Collections;

public class MatchBoxRunnerScript : MonoBehaviour
{
    public float effectiveTime;
    public Vector2 relativePosition;
    public GameObject SparkParticle;
    public GameObject DestroyParticle;
    private bool timerOn = false;
    private Animator anim;
    private ParticleSystem sp;
    private ParticleSystem dp;
    private HatScript HS{ get; set; }

    void Update()
    {
        if (timerOn)
        {
            effectiveTime -= Time.deltaTime;
            if (effectiveTime <= 0.0f)
            {
                HS.BoxOfMatches = timerOn = false;
                HS.MatchBoxRunner = null;
                dp.Play();
                GameObject.Destroy(gameObject, 0.3f);
            }
        }
    }

    public void Run(HatScript hatScript)
    {
        if (effectiveTime > 0f)
        {
            HS = hatScript;
            Vector3 position = HS.transform.position;
            position.x += relativePosition.x;
            position.y += relativePosition.y;
            Quaternion rotation = HS.transform.rotation;
            transform.position = position;
            transform.rotation = rotation;
            HS.MatchBoxRunner = this;
            anim = GetComponentInChildren<Animator>();
            sp = SparkParticle.GetComponent<ParticleSystem>();
            dp = DestroyParticle.GetComponent<ParticleSystem>();
            HS.BoxOfMatches = timerOn = true;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Fire()
    {
        sp.Play();
        anim.SetTrigger("Fire");
        //Debug.Log("Tr");
    }
}