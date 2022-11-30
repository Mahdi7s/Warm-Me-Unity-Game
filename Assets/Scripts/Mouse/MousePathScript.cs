using UnityEngine;
using System.Collections;

public class MousePathScript : MonoBehaviour
{
    public GameObject hat;
    private Vector3[] pointsArray;

    void Start()
    {
        pointsArray = new Vector3[transform.childCount];
        for (int cntr=0; cntr<transform.childCount; cntr++)
        {
            pointsArray [cntr] = transform.GetChild(cntr).position;
        }
    }
	
    public Vector3[] GetPath()
    {
        return pointsArray;
    }
	
    public Vector3 GetHatPosition()
    {
        return hat.transform.position;
    }

    public GameObject GetHat()
    {
        return hat;
    }

    public int GetPathLength()
    {
        return pointsArray.Length;
    }
}