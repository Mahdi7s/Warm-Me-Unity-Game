using UnityEngine;
using System.Collections;

public class LikeUsBoardDeactivator : MonoBehaviour
{
    void Awake()
    {
        if (ButtonScript.isGoneToFacebook && ButtonScript.isGoneToTwitter)
        {
            gameObject.SetActive(false);
        }
    }
}
