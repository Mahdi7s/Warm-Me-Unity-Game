using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ChangeSceneScript : MonoBehaviour
{
    public string GoToScene = string.Empty;
    public bool CanChangeScene = true;
    public Vector3 OkPlace = Vector3.zero;
    private Vector3? touchPos = null;
    private float secsDelay = 0.0f;
    private ChLockStateScript _lockStateScr = null;

    private float clickDelta = 0.2f;
    private float timeDelta = 1.0f;

    void Start()
    {
        _lockStateScr = GetComponent<ChLockStateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!touchPos.HasValue && Input.GetMouseButtonDown(0))
        {
            var coll = TouchUtility.GetCollider2D(Input.mousePosition);
            //Debug.Log("Slide Yes: " + (coll.gameObject.GetInstanceID() == gameObject.GetInstanceID()).ToString() + ", Pos: " + transform.position.ToString());
            if ((coll != null && coll.gameObject == gameObject) && MathUtility.XDiff(transform.position.x, OkPlace.x) <= clickDelta)
            {
                touchPos = Input.mousePosition;
            }
        }

        if (touchPos.HasValue && Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Touch Yes: " + (touchPos.HasValue && touchPos.Value == Input.mousePosition).ToString() + ", Secs Delay: " + secsDelay.ToString());
            if (touchPos.HasValue && (MathUtility.XDiff(Input.mousePosition.x, touchPos.Value.x) <=clickDelta) && secsDelay <= timeDelta)
            {
                if (_lockStateScr != null && _lockStateScr.IsLocked)
                {
                    _lockStateScr.ShowBoard();
                }
                else
                {
                    SlideScript.defaultChapterNumber = int.Parse(GoToScene.Substring(GoToScene.Length - 1, 1));
                    GameState.LoadMenu(GoToScene);
                }
            }
            touchPos = null;
            secsDelay = 0.0f;
        }

        if (touchPos.HasValue)
        {
            secsDelay += Time.fixedDeltaTime;
            Debug.Log(MathUtility.XDiff(Input.mousePosition.x, touchPos.Value.x).ToString());
        }

        if (secsDelay > timeDelta || (touchPos.HasValue && MathUtility.XDiff(touchPos.Value.x, Input.mousePosition.x) > clickDelta))
        {
            secsDelay = 0.0f;
            touchPos = null;
        }
    }
}