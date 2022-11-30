using UnityEngine;
using System.Collections;

public class DragEffectScript : MonoBehaviour
{
    private TrailRenderer _trailRenderer = null;

    void Start()
    {
        renderer.sortingLayerName = "StoreLayer";
        renderer.sortingOrder = -1;
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.enabled = false;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _trailRenderer.enabled = true;
        }
        if (Input.GetMouseButton(0))
        {
            var pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
            transform.position = new Vector3(pos.x, pos.y, -1);
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            _trailRenderer.enabled = false;
        }
    }
}
