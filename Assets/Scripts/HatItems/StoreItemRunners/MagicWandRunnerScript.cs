using UnityEngine;
using System.Collections;

public class MagicWandRunnerScript : MonoBehaviour
{
    public float effectiveTime;
    public Vector2 relativePosition;
    public GameObject[] particle;

    private bool timerOn = false;
    private ParticleSystem p;
    private HatScript HS { get; set; }

    void Update()
    {
        if (timerOn)
        {
            effectiveTime -= Time.deltaTime;
            if (effectiveTime <= 0.0f)
            {
                HS.MagicWand = timerOn = false;
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void Run(HatScript hatScript)
    {
        HS = hatScript;
        if (effectiveTime > 0f)
        {
            Vector3 position = HS.transform.position;
            position.x += relativePosition.x;
            position.y += relativePosition.y;
            Quaternion rotation = HS.transform.rotation;
            transform.position = position;
            transform.rotation = rotation;
            foreach (GameObject go in particle)
            {
                go.GetComponent<ParticleSystem>().Play();
            }
            HS.MagicWand = timerOn = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}