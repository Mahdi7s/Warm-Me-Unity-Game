using UnityEngine;
using System.Collections;

public class FireLayer : MonoBehaviour
{
    void Start()
    {
        particleSystem.renderer.sortingLayerName = "Fire";
    }
}