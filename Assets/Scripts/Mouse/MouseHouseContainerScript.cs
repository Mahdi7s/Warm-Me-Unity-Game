// This is the script that instantiates mouses, and trap them if any mouse trap item hitted by the user.
// Logic:
// At the function Start, the script decide about the time that each mouse should be instantiad, and assign
// every mouse a path. every path is binded to a hat, so the mouse will fall it if hat reached by the mouse
// and no trap is in his way; if any trap is existed in his path, it ignores the hat, and goes for the trap.

// mod= Mouse Object Data
// mps= Mouse Path Script
// mhs= Mouse House Script

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseHouseContainerScript : PausableBehaviour
{
    #region Definitions
    public int numberOfMousesPerHouse;
    public Transform mousePrefab;
    public Transform mouseTrapPlaced;
    public GameObject mouseContainer;
    public float timeToExit = 30f;
    List<MouseObjectsData> mouseDieList = new List<MouseObjectsData>();
    List<MouseObjectsData> mouseInstantiateList = new List<MouseObjectsData>();
    float elapsedTime;
    #endregion

    void Awake()
    {
        if (StoreItemScript.mhcs == null) // if MouseContainerScript is not referenced in StoreItemScript
        {
            MouseObjectsData.mhcs = StoreItemScript.mhcs = this; // reference it everywhere that is needed
            StoreItemScript.mouseTrap = mouseTrapPlaced;
            MouseScript.mouseTrap = mouseTrapPlaced;
        }
        if (numberOfMousesPerHouse > 0) // if this level should has a mouse
        {
            float timeWindow = timeToExit / numberOfMousesPerHouse;
            for (int cntr=0; cntr<transform.childCount; cntr++)
            {
                MouseHouseScript mhs = transform.GetChild(cntr).gameObject.GetComponent<MouseHouseScript>();
                mhs.TimeWindow = timeWindow;                    // references every property in any
                mhs.NumberOfMouses = numberOfMousesPerHouse;    // MouseHouseScript in its children
                mhs.MousePrefab = mousePrefab;
                mhs.MouseTrapPlaced = mouseTrapPlaced;
                mhs.MHCScript = this;
            }
        } else
        {
            gameObject.SetActive(false);             // Deactive this object
        }
    }

    protected override void PUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (mouseInstantiateList.Count > 0)
        {
            MouseObjectsData temp = mouseInstantiateList.First();
            if (temp != null)
            {
                if (temp.triggerTime <= elapsedTime)    // if the mouse should go
                {
                    bool containInDieList = mouseDieList.Contains(temp);
                    if (containInDieList)   // if the mouse is already in die list
                    {
                        mouseDieList.Remove(temp);  // remove it from die list
                    }
                    mouseInstantiateList.Remove(temp); // remove current mouse from the mouse instantiate list, then instantiate it
                    MouseScript ms = ((Transform)Instantiate(mousePrefab, temp.path.GetPath() [0], Quaternion.identity)).gameObject.GetComponent<MouseScript>();
                    ms.transform.parent = mouseContainer.transform;
                    ms.SetPathAndHome(temp.path, mouseContainer, temp.trap); // Initialize instantiated mouse
                    ms.mod = temp;                                           // bind the mouse to its mouse object data
                    temp.mouseScript = ms;                                   // update the info in its object data
                    temp.isGone = true;                                      // Set the mouse status to gone.
                    if (containInDieList)                                    // if the mouse was in die list before
                    {
                        mouseDieList.Add(temp);                              // add it again to die list
                        mouseDieList = mouseDieList.OrderBy(element => element.triggerTime).ToList();   // Sort the list
                    }
                }
            }
        }
    }

    protected override void PFixedUpdate()
    {
        
    }
    
    protected override void POnGUI()
    {
        
    }

    public void AddToMouseObjectDataList(List<MouseObjectsData> modl)
    {
        mouseInstantiateList.AddRange(modl);    // get all mouses in all houses and update the lists accordingly
        mouseInstantiateList = mouseInstantiateList.OrderBy(element => element.triggerTime).ToList();
        DublicateList(mouseInstantiateList, mouseDieList);
    }
    
    public void KillAMouse()
    {
        if (mouseDieList.Count > 0)             // is any mouse existed in die list
        {
            mouseDieList = mouseDieList.OrderBy(element => element.triggerTime).ToList();
            MouseObjectsData temp = mouseDieList.First();   // same as instantiate procedure
            if (temp != null)
            {
                bool containInInstantiateList = mouseInstantiateList.Contains(temp);
                if (containInInstantiateList)
                {
                    mouseInstantiateList.Remove(temp);
                    int diePoint = Random.Range(1, temp.path.GetPathLength() - 1);
                    temp.trap = (Transform)(Instantiate(mouseTrapPlaced, temp.path.GetPath() [diePoint], temp.GetOriention(diePoint)));
                    foreach (ParticleSystem particleSystem in temp.trap.GetComponentsInChildren<ParticleSystem>())
                    {
                        particleSystem.Play();
                    }
                    mouseInstantiateList.Add(temp);
                    mouseInstantiateList = mouseInstantiateList.OrderBy(element => element.triggerTime).ToList();

                } else
                {
                    temp.mouseScript.FallInTrap();
                } 
                mouseDieList.Remove(temp);
            }
        }
    }
    // This method will remove all mouses that their target is the same, and they are not instantiated yet.
    public void RemoveAllMousesInPath(MousePathScript mps)
    {
        List<MouseObjectsData> mouseObjectDataList = new List<MouseObjectsData>();
        foreach (MouseObjectsData mod in mouseInstantiateList)
        {
            if (mod.path.GetPath() != mps.GetPath())
            {
                mouseObjectDataList.Add(mod);
            }
        }
        DublicateList(mouseObjectDataList, mouseInstantiateList);
        mouseObjectDataList.Clear();
        foreach (MouseObjectsData mod in mouseDieList)
        {
            if (mod.path.GetPath() != mps.GetPath())
            {
                mouseObjectDataList.Add(mod);
            }
        }
        DublicateList(mouseObjectDataList, mouseDieList);
    }

    // this method will remove a mouse.
    public void Remove(MouseObjectsData mod)
    {
        mouseInstantiateList.Remove(mod);
        mouseDieList.Remove(mod);
    }
    // this method will dublicate a list: create a copy of a list.
    public void DublicateList<T>(List<T> source, List<T> destination)
    {
        destination.Clear();
        foreach (T src in source)
        {
            destination.Add(src);
        }
    }
}