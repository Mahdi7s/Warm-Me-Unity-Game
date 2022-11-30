using UnityEngine;
using System.Collections;

public class TouchUtility
{   
    public static Collider2D GetTouchedCollider(bool onMouseUp = true)
    {
        if ((onMouseUp && Input.GetMouseButtonUp(0)) || (!onMouseUp && Input.GetMouseButtonDown(0)))
        {
            return GetCollider2D(Input.mousePosition);
        }
        return null;
    }

    public static Collider2D GetCollider2D(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        
        var overlapColl = Physics2D.OverlapPoint(touchPos);
        
        return overlapColl;
    }

    public static Vector2 GetTouchScroll()
    {
        Vector2 deltaPos = Vector2.zero;
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 deltaPosition = Vector3.zero;
            while (!Input.GetMouseButtonUp(0))
            {
                deltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPosition;
            }
            deltaPos.x = deltaPosition.x;
            deltaPos.y = deltaPosition.y;
        }
        return deltaPos;
    }
}