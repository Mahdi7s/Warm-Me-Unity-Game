using UnityEngine;
using System.Collections;

public class TabItemScript : MonoBehaviour
{  
    private bool _isSelected = false;
    public bool isLocked;
    public PurchasedItems Kind;
    public Sprite Title = null;
    public Sprite Description = null;
    public GameObject SelectedObject = null;
    public GameObject DeselectedObject = null;
    public SpriteRenderer itemRenderer = null;
    public Color itemRendererColor;
    public SpriteRenderer lockSprite = null;
    public GUINumberScript Count = null;

    public int MaxItemsCount = 10;

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                if (_isSelected)
                {
                    SelectedObject.SetActive(true);
                    DeselectedObject.SetActive(false);
                } else
                {
                    SelectedObject.SetActive(false);
                    DeselectedObject.SetActive(true);
                }
            }
        }
    }

    void Start()
    {
        var cls = Camera.main.GetComponent<ChapterLevelScript>();
        isLocked = cls.GetLockStatus(Kind);
        Count = GetComponentInChildren<GUINumberScript>();
        lockSprite.enabled = isLocked;
        if (isLocked)
        {
            itemRenderer.color = itemRendererColor;
        }
    }
}