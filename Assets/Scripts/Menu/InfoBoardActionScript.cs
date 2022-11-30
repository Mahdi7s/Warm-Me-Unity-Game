﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class InfoBoardActionScript : MonoBehaviour
{
    public float AutoHideDelay = 4.0f;

    private AnimDelayScript[] _delayScripts = null;
    private float _timer = 0.0f;
    private bool show = false;
    private int _index = 0;
    private float _autoHideDelay = 0.0f;

    // Use this for initialization
    void Start()
    {
        _autoHideDelay = AutoHideDelay;
        _delayScripts = GetComponentsInChildren<AnimDelayScript>().OrderBy(x => x.DelaySeconds).ToArray();
        /*foreach (var delayScr in _delayScripts)
        {
            delayScr.enabled = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            _timer += Time.deltaTime;

            if (_timer >= _delayScripts[_index].DelaySeconds)
            {
                var anim = _delayScripts[_index].GetComponent<Animator>();
                if (_index + 1 < _delayScripts.Length)
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        anim.SetTrigger("start");
                    }
                    ++_index;
                }
                else
                {
                    if ((_autoHideDelay -= Time.deltaTime) <= 0.0f)
                    {
                        GetComponent<Animator>().SetTrigger("click");
                        OnBoardHide();
                    }
                }
            }
        }
    }

    public void OnBoardShow()
    {
        if (!show)
        {
            _autoHideDelay = AutoHideDelay;
            show = true;
            _timer = 0.0f;
            _index = 0;
        }
    }

    public void OnBoardHide()
    {
        if (show)
        {
            show = false;
            EnableDelayScripts(show);
        }
    }

    private void EnableDelayScripts(bool enable)
    {
        foreach (var delayScr in _delayScripts)
        {
            var anim = delayScr.GetComponent<Animator>();
            if (enable)
            {
                delayScr.GetComponent<Animator>().SetTrigger("start");
            }
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("TextFade"))
                {
                    anim.SetTrigger("end");
                }
            }
        }
    }
}