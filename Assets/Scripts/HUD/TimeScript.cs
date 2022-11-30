using UnityEngine;
using System.Collections;

public class TimeScript : PausableBehaviour
{
    public GameObject timeStep;
    public GameObject board;
    public Collider2D menuIconCollider;
    public int numberOfStepsInACircle;
    public float animationLength;

    public float LevelTime
    {
        get;
        private set;
    }

    public float Speed
    {
        get;
        set;
    }
	
    private int cntr, numberOfInstances;
    private float elapsedTime;
    private bool initialized;
    private GameObject temp;
    private ChapterLevelScript cls;
    private Animator stopWatchAnim;

    private Vector3 position, rotation, scale;

    void Awake()
    {
    }

    public void Initialize(ChapterLevelScript chapterLevelScript)
    {
        cls = chapterLevelScript;
        //rotation = new Vector3(0f, 0f, 0f);
        //scale = timeStep.transform.localScale;
        //position = timeStep.transform.position;
        //cntr = numberOfInstances = 1;
        //LevelTime = cls.LevelDurationSecs + 30;
        LevelTime = cls.ItemsCount;
        //LevelTime = 5.0f;
        stopWatchAnim = gameObject.GetComponent<Animator>();
        //Speed = animationLength / LevelTime;
        elapsedTime = 0.0f;
        //stopWatchAnim.speed = 0;
        stopWatchAnim.playbackTime = 0;
        //stopWatchAnim.SetBool("Run", true);
        initialized = true;
    }

    protected override void PFixedUpdate()
    {
        /*if (LevelTime <= 0)
        {
            LevelTime = cls.ItemsCount;
            //LevelTime = 5.0f;
            stopWatchAnim = gameObject.GetComponent<Animator>();
            Speed = animationLength / LevelTime;
            elapsedTime = 0.0f;
            stopWatchAnim.speed = Speed;
            stopWatchAnim.SetBool("Run", true);
        }*/
        if (initialized)
        {
            if (LevelTime > elapsedTime)
            {
                //elapsedTime += Time.deltaTime;
                elapsedTime = (float)cls.InstantiatedItems;
                stopWatchAnim.playbackTime = (float)((float)elapsedTime / (float)LevelTime);
                /*cntr = (int)(elapsedTime * numberOfStepsInACircle / LevelTime);
                while (numberOfInstances < cntr)
                {
                    rotation.z -= (360.0f / numberOfStepsInACircle);
                    temp = (GameObject)Instantiate(timeStep, position, Quaternion.Euler(rotation));
                    temp.transform.parent = transform.parent;
                    temp.transform.localScale = scale;
                    numberOfInstances++;
                }
                stopWatchAnim.speed = Speed;*/
            }
            else if (LevelTime != 0.0f)
            {
                menuIconCollider.enabled = false;
                LevelTime = 0.0f;
                stopWatchAnim.SetBool("Run", false);
                stopWatchAnim.speed = 0.0f;
                board.GetComponent<BoardScript>().ShowScore();
            }
        }
    }

    public string GetTime()
    {
        PFixedUpdate();
        return elapsedTime.ToString();
    }

    protected override void PUpdate()
    {
    }
    
    protected override void POnGUI()
    {
    }
}