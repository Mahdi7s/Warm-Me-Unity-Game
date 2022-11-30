using UnityEngine;
using System.Collections;

public class AnimatorSpeedChanger : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float speed;

    void Awake()
    {
        foreach (GameObject gObj in gameObjects)
        {
            ChangeAnimatorSpeedInAllChildren(gObj.transform, speed);
            if (gObj.name.StartsWith("Hats Container"))
            {
                ChangeParticleSystemSpeedInAllChildren(gObj.transform, speed);
            }
        }
    }

    // This method will call ba chapter level script
    public void SetSpeed(float speed)
    {
        this.speed = speed;
        Awake();
    }

    /// <summary>
    /// Change all animators speed in childs of a transform to the specified speed, and the transform itself.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="speed"></param>
    public static void ChangeAnimatorSpeedInAllChildren(Transform transform, float speed)
    {
        Animator anim = transform.GetComponent<Animator>();
        if (anim != null)
        {
            anim.speed = speed;
        }
        if (transform.childCount > 0)
        {
            for (int cntr = 0; cntr < transform.childCount; cntr++)
            {
                ChangeAnimatorSpeedInAllChildren(transform.GetChild(cntr), speed);
            }
        }
    }

    /// <summary>
    /// Change all particles speed in childs of a transform to the specified speed, and the transform itself.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="speed"></param>
    public static void ChangeParticleSystemSpeedInAllChildren(Transform transform, float speed)
    {
        ParticleSystem ps = transform.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.startDelay /= speed;
        }
        if (transform.childCount > 0)
        {
            for (int cntr = 0; cntr < transform.childCount; cntr++)
            {
                ChangeParticleSystemSpeedInAllChildren(transform.GetChild(cntr), speed);
            }
        }
    }
}
