using UnityEngine;
using System.Collections;
using System.Linq;

public class GUILetterScript : MonoBehaviour
{
    public string sortingLayer = "GuiMenu";
    public int sortingOrder = 7;
    public float letterSpace = 0;
    [Tooltip("You can just initilize first 28 items, then other items will be fill automatically.")]
    public Transform[] letters = new Transform[52];
    private string _string;
    private Transform placeHolder = null;
    internal bool isVisible = true;
    private bool timerOn;

    public string str
    {
        get { return _string; }
        set
        {
            _string = value;
            if (isVisible)
            {
                GUIUtility.DrawGUITextureAsText(letters, placeHolder, _string, sortingLayer, sortingOrder, letterSpace);
            }
        }
    }

    void Awake()
    {
        placeHolder = transform.GetChild(0);
        if (letters[26] == null)                    // if upper case letters was not initialized
        {
            for (int cntr = 0; cntr < 26; cntr++)   // Use lower case letters instead of upper
            {                                       // case letters.
                letters[cntr + 26] = letters[cntr];
            }
        }
    }

    public void Hide()
    {
        isVisible = false;
        DestroyInstantiatedLetters();
    }

    public void Show()
    {
        isVisible = true;
        GUIUtility.DrawGUITextureAsText(letters, placeHolder, str, sortingLayer, sortingOrder, letterSpace);
    }

    public void DestroyInstantiatedLetters()
    {
        for (int cntr=0; cntr<placeHolder.childCount; cntr++)
        {
            Destroy(placeHolder.GetChild(cntr).gameObject);
        }
    }

    public Vector3 GetPlaceHolderPosition()
    {
        if (placeHolder != null)
        {
            return placeHolder.position;
        }
        return Vector3.zero;
    }

    public Vector3 GetLetterPosition(int index)
    {
        if (placeHolder != null && isVisible)
        {
            return placeHolder.GetChild(index).position;
        }
        return Vector3.zero;
    }
}