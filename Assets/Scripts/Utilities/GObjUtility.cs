using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public static class GObjUtility
{
    public static void DisableContent(GameObject gobj, bool disable)
    {
        var anims = gobj.GetComponentsInChildren<Animator>();
        var particles = gobj.GetComponentsInChildren<ParticleSystem>();
        foreach (var anim in anims)
        {
            anim.enabled = !disable;
        }
        foreach (var part in particles)
        {
            part.enableEmission = !disable;
        }
    }

    /// <summary>
    /// A recursive method that returns progeny of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns>Progeny of a Transform</returns>
    public static List<Transform> GetTransformProgeny(Transform transform)
    {
        List<Transform> transformsList= new List<Transform>();
        transformsList.Add(transform);
        for (int cntr = 0; cntr < transform.childCount; cntr++)
        {
            transformsList.AddRange(GetTransformProgeny(transform.GetChild(cntr)));
        }
        return transformsList;
    }
}
