using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    public Transform[] digit = new Transform[10];
    public float space;
    public string sortingLayer;
    public int sortingOrder;

    void Start()
    {
        GameState.scoreScript = this;
        GUIUtility.digit = digit;
        GUIUtility.DrawGUITextureAsText(transform, 0, sortingLayer, sortingOrder, space);
    }

    // This function will be called every time GameState.LevelCoins property updated.
    public void UpdateScore()
    {
        GUIUtility.DrawGUITextureAsText(transform, GameState.LevelCoins, sortingLayer, sortingOrder, space);
    }
}