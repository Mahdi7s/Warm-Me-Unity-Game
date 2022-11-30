using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PauseScript : MonoBehaviour
{
    private static List<PausableBehaviour> PausableObjects = null;
    //private static bool _pause  = false;
    private float timeScale;
    public Animator InGameMenuAnim = null;
    public bool shouldBePausedAtStart=true;
    private bool isPaused = false;

    public void PauseAll()
    {
        if (!isPaused)
        {
            isPaused = true;
            timeScale = Time.timeScale;
            Time.timeScale = 1.0f;
            Initialize();
            PausableObjects.ForEach(x => x.OnPause());
        }
    }
    
    public void ResumeAll()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = timeScale;
            PausableObjects.ForEach(x => x.OnResume());
        }
    }

    public void Initialize()
    {
        //if (PausableObjects == null)
        {
            PausableObjects = FindObjectsOfType<PausableBehaviour>().ToList();
            PausableObjects.ForEach(x => x.Initialize());
        }
    }

    /*
    void OnApplicationPause()
    {
        _pause = true;
    }
    
    void OnApplicationFocus()
    {
        if (_pause)
        {
            PauseAll();
            InGameMenuAnim.SetTrigger("In");
            _pause = false;
        }
    }
    */
    
    void Awake()
    {
        GameState.pauseScript = this;
        //Initialize()
    }
    
    void Start()
    {
        if (!shouldBePausedAtStart)
        {
            GameState.Resume();
        }
    }
    
    void Update()
    {
        /*
        PausableObjects.ForEach(x => {
            if (!x.isPaused)
            {
                x.
            }
        });
        */
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}
