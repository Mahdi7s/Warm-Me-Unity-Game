using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SlideScript : MonoBehaviour
{
    public static int defaultChapterNumber = 0;
    private int _currentSlideIndex = 0;
    private Vector2 _slideSize = Vector2.zero;
    private Vector3 _maxDiff = Vector3.zero;
    private float _minWidthToSlide = 0.0f;
    private Vector3? startDrag = null;
    private Vector3? diffSum = null;
    private Vector3? remaningDiff = null;
    private Vector3? to = null;
    private Vector3 diff = Vector3.zero;
    private int dir = 0;

    private float delaySecs = 0.0f;
    private float slideDelta = 0.2f;

    private Transform[] _objectsToSlide = null;
    private Transform[] _slideCircles = null;

    public float bgSpeed = 0.3f;
    public float speed = 1.1f;

    public GameObject Background = null;
    public GameObject SlidesContainer = null;

    public GameObject SlideCirclesContainer = null;
    public GameObject FullCircle = null;

    void Start()
    {
        EmbededMobileBackButtonScript.lastScene = "Main";
        var scH = Screen.height;
        var scW = Screen.width;

        _maxDiff = Camera.main.ScreenToWorldPoint(new Vector3(scW, scH, 10));
        _maxDiff.y = _maxDiff.z = 0;

        _minWidthToSlide = _maxDiff.x * 0.60f;

        _objectsToSlide = SlidesContainer.GetComponentsInChildren<ChLockStateScript>().Select(x => x.transform).ToArray();
        if (SlideCirclesContainer != null)
        {
            _slideCircles = SlideCirclesContainer.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }

        var spr = _objectsToSlide.First().GetComponent<SpriteRenderer>().sprite;
        _slideSize = GUIUtility.GetSpriteSize(spr);

        /*
        var bgSpr = Background.GetComponent<SpriteRenderer>().sprite;
        var bgSize = GUIUtility.GetSpriteSize(bgSpr);
        bgSpeed = bgSize.x / 2 / _objectsToSlide.Length / Camera.main.ScreenToWorldPoint(new Vector3(scW, scH, 10.0f)).x;
        */

        for (int i = 0; i < _objectsToSlide.Length; i++)
        {
            var obj = _objectsToSlide[i];
            obj.position = Camera.main.ScreenToWorldPoint(new Vector3((i * scW) + scW / 2, scH / 2, 10.0f));
            obj.parent = SlidesContainer.transform;
        }

        if (defaultChapterNumber > 0)
        {
            speed = 1000.0f;
            _currentSlideIndex = defaultChapterNumber - 2;
            startDrag = Vector3.one;
            diffSum = new Vector3(_minWidthToSlide, 0, 0);
        }
    }

    private Vector3? backTo = null;
    void Update()
    {
        if (!startDrag.HasValue && Input.GetMouseButtonDown(0))
        {
            //if((delaySecs += Time.deltaTime) >= 1.5f)
            {
                startDrag = GetMousePosition();
                diffSum = Vector3.zero;

                delaySecs = 0.0f;

            }
        }
        else if (startDrag.HasValue && Input.GetMouseButtonUp(0))
        {
            startDrag = null;
            //dir = 0;
        }

        if (startDrag.HasValue)
        {
            var currDrag = GetMousePosition();
            diff = currDrag - startDrag.Value;
            if (Mathf.Abs(diff.x) <= slideDelta)
                return;
            dir = (int)Mathf.Sign(Camera.main.WorldToScreenPoint(currDrag).x - Camera.main.WorldToScreenPoint(startDrag.Value).x) * -1;//(int)Mathf.Sign(currDrag.x - startDrag.Value.x) * -1;           

            startDrag = currDrag;
        }

        if ((_currentSlideIndex == 0 && dir == -1) || (_currentSlideIndex == _objectsToSlide.Length - 1 && dir == 1))
            return;

        if (diffSum.HasValue && (!startDrag.HasValue || diffSum.Value.x >= _minWidthToSlide)) // slide to next or prev
        {
            startDrag = null;

            var from = SlidesContainer.transform.position;
            if (!to.HasValue)
            {
                remaningDiff = _objectsToSlide[_currentSlideIndex + dir].position;
                to = SlidesContainer.transform.position - remaningDiff;
            }

            if (remaningDiff.HasValue && to.HasValue)
            {
                SlidesContainer.transform.position = Vector3.MoveTowards(from, to.Value, speed * (Time.deltaTime * 10));
            }

            if (Mathf.Abs(_objectsToSlide[_currentSlideIndex + dir].position.x) <= slideDelta)
            {

                SlidesContainer.transform.position = to.Value;

                remaningDiff = null;
                diffSum = null;
                startDrag = null;
                to = null;
                ChangeSlide(dir);
                dir = 0;

                //Debug.Log("Luck: " + _currentSlideIndex.ToString());

                //return;
            }
        }
        else if (diffSum.HasValue) // dont slide, only drag
        {
            var tFrom = SlidesContainer.transform.position;
            var tTo = SlidesContainer.transform.position + diff;
            //Debug.LogError("diff: " + Vector3.Distance(tFrom, tTo).ToString());
            if (MathUtility.XDiff(tFrom.x, tTo.x) > slideDelta)
            {
                SlidesContainer.transform.position = Vector3.MoveTowards(tFrom, tTo, speed);
                diffSum += diff;
            }
        }


        var sPos = SlidesContainer.transform.position;
        var bgPos = Background.transform.position;
        Background.transform.position = new Vector3(sPos.x * bgSpeed, bgPos.y, bgPos.z);
    }

    void OnDisable()
    {
        remaningDiff = null;
        diffSum = null;
        startDrag = null;
        to = null;
        dir = 0;
    }

    /*
    void OnGUI()
    {
        var i = 0;
        foreach (var obj in _objectsToSlide)
        {
            GUI.Button(new Rect(10, 50 * (i + 1), 100, 40), obj.position.ToString() + "-" + i.ToString());
            ++i;
        }

        GUI.Button(new Rect(10, 200, 100, 40), "s idx: " + _currentSlideIndex.ToString());
        GUI.Button(new Rect(10, 250, 100, 40), "dir: " + dir.ToString());
    }
    */


    // ------------------------------------------------------------------------------------------

    private void ChangeSlide(int direction)
    {
        _currentSlideIndex += direction;
        FullCircle.transform.localPosition = _slideCircles[_currentSlideIndex].transform.localPosition;
        FullCircle.transform.parent = _slideCircles[_currentSlideIndex].transform;
        FullCircle.transform.position = _slideCircles[_currentSlideIndex].transform.position;

        // on slide complete
        speed = 1.1f;
    }

    private Vector3 GetMousePosition()
    {
        var mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPos.y = 0;
        mPos.z = 0;
        return mPos;
    }
}