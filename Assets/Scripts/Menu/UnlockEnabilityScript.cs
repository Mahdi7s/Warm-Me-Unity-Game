using UnityEngine;
using System.Collections;

public class UnlockEnabilityScript : MonoBehaviour
{
    private SlideScript _slideScr = null;
    private GUINumberScript _guiNumber = null;

    void Start()
    {
        _slideScr = FindObjectOfType<SlideScript>();
        _guiNumber = GetComponentInChildren<GUINumberScript>();
    }


    public void EnableSlide()
    {
        StartCoroutine("EnableSlideRoutin");
    }
    bool enabling = false;
    private IEnumerator EnableSlideRoutin()
    {
        if (!enabling)
        {
            enabling = true;

            yield return new WaitForSeconds(0.5f);

            bool enability = true;
            if (_slideScr != null)
            {
                _slideScr.enabled = enability;
            }
            enabling = false;
        }
        yield break;
    }

    public void DisableSlide()
    {
        bool enability = false;
        if (_slideScr != null)
        {
            _slideScr.enabled = enability;
        }
    }

    public void SetGuiNumber(int num)
    {
        _guiNumber.EditorNum = num;
        _guiNumber.Number = num;

        _guiNumber.Show();
    }
}
