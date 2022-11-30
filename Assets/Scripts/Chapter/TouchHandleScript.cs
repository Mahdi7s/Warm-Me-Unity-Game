using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TouchHandleScript : MonoBehaviour
{
    public string HatItemLayer = "HatItem";
    public int HatItemsZIndex = 0;

    private Vector3? _mousePos = null;
    private RaycastHit2D[] _hits = null;
    private int _hatItemsLayerMask = 0;
    
    void Start()
    {
        var chapterLevel = Camera.main.GetComponent<ChapterLevelScript>();
        _hits = new RaycastHit2D[chapterLevel.Hats.Count()];

        _hatItemsLayerMask = (1 << LayerMask.NameToLayer(HatItemLayer));
    }
	
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var camZ = 0 - Camera.main.transform.position.z;
            var from = _mousePos.HasValue ? _mousePos.Value : Input.mousePosition;
            from = Camera.main.ScreenToWorldPoint(new Vector3(from.x, from.y, camZ));

            var to = Input.mousePosition;
            to = Camera.main.ScreenToWorldPoint(new Vector3(to.x, to.y, camZ));

            var items = Physics2D.LinecastAll(from, to, _hatItemsLayerMask, HatItemsZIndex, HatItemsZIndex);
            foreach (var itm in items)
            {
                itm.transform.collider2D.enabled = false;
                OnItemHit(itm.transform.parent.gameObject);
            }

            _mousePos = Input.mousePosition;
        } 

        if (Input.GetMouseButtonUp(0))
        {
            _mousePos = null;
        }
    }

    private void OnItemHit(GameObject gObj)
    {
        gObj.SendMessage("Hit");
    }
}