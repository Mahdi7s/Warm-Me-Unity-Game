using UnityEngine;
using System.Collections;
using System.Linq;

public class GUINumberScript : MonoBehaviour
{

    public string SortingLayer = "GuiMenu";
    public int SortingOrder = 7;
    public int DigitSpace = 0;

    public Transform[] _digits = new Transform[10];
    private int _number = 0;
    private Transform PlaceHolder = null;
    internal bool isVisible = true;
    private bool timerOn;

    public int Number
    {
        get{ return _number; }
        set
        {
            _number = value;
            if (isVisible)
            {
                if (PlaceHolder != null)
                {
                    GUIUtility.DrawGUITextureAsText(_digits, PlaceHolder, _number, SortingLayer, SortingOrder, DigitSpace);
                }
            }
        }
    }

    public int EditorNum = 0;

    void Awake()
    {
        PlaceHolder = transform.GetChild(0);
        Number = EditorNum;
    }

    void Start()
    {
        
    }

    public void Hide()
    {
        isVisible = false;
        DestroyInstantiatedDigits();
    }

    public void Show()
    {
        isVisible = true;
        if (PlaceHolder != null)
        {
            GUIUtility.DrawGUITextureAsText(_digits, PlaceHolder, _number, SortingLayer, SortingOrder, DigitSpace);
        }
    }

    public void DestroyInstantiatedDigits()
    {
        if (PlaceHolder != null)
        {
            for (int cntr = 0; cntr < PlaceHolder.childCount; cntr++)
            {
                Destroy(PlaceHolder.GetChild(cntr).gameObject);
            }
        }
    }
}