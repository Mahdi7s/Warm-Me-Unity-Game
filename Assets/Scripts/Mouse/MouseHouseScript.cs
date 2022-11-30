using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseHouseScript : PausableBehaviour
{
    #region Definitions
    MousePathScript[] paths;
    List<MouseObjectsData> modl = new List<MouseObjectsData>();
    MousePathScript path;
    bool isGone;
    float elapsedTime;
    int pathIndex;

    public MouseHouseContainerScript MHCScript  // Stands for Mouse House Container Script
    { set; private get; }

    public float TimeWindow                     // This will be calculate and set by Mouse House Container Script.
    { set; private get; }

    public int NumberOfMouses                   // This will be set by Mouse House Container Script.
    { set; private get; }

    public Transform MousePrefab                // This will be set by Mouse House Container Script.
    { set; private get; }

    public Transform MouseTrapPlaced            // This will be set by Mouse House Container Script.
    { set; private get; }
    #endregion

    void Start()
    {
        paths = GetComponentsInChildren<MousePathScript>();
        if (TimeWindow > 0f)
        {
            float triggerTime;
            MousePathScript mps;    // mps= Mouse path script
            MouseObjectsData mod;   // mps= Mouse Object Data
            for (int cntr=0; cntr<NumberOfMouses; cntr++)
            {
                triggerTime = cntr * TimeWindow + Random.value * TimeWindow;
                mps = paths [Random.Range(0, paths.Length)];
                mod = new MouseObjectsData(triggerTime, mps);
                modl.Add(mod);      // modl= Mouse Objects Data List
            }
        }
        if (MHCScript == null)
        {
            MHCScript = GetComponentInParent<MouseHouseContainerScript>();
        }
        MHCScript.AddToMouseObjectDataList(modl);   // Send the list for mouse house container script.
    }

    protected override void PUpdate()
    {
       
    }

    protected override void PFixedUpdate()
    {
        
    }

    protected override void POnGUI()
    {
       
    }
}