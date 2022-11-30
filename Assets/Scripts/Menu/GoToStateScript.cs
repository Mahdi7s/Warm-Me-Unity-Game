using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class GoToStateScript : MonoBehaviour
{
    private bool doClick = false;
    private float _disablePeriod;

    [Header("Animator")]
    public Animator anim;
    public string transitionCondition;
    public bool isTrigger;

    [Header("Action")]
    public Behaviour ActionScript = null;
    public string ActionName = string.Empty;
    public GameObject ObjectToSend = null;
    public GameObject Object2ToSend = null;
    public bool DisableSelfOnAction = false;


    public float DisablePeriod = 2.5f;

    void Start()
    {
        _disablePeriod = DisablePeriod;
    }

    void Update()
    {
        if (!doClick)
        {
            var touchedColl = TouchUtility.GetTouchedCollider();
            if (touchedColl != null)
            {
                if (touchedColl == collider2D)
                {
                    doClick = true;

                    if (!string.IsNullOrEmpty(transitionCondition))
                    {
                        if (isTrigger)
                        {
                            anim.SetTrigger(transitionCondition);
                        } else
                        {
                            anim.SetBool(transitionCondition, true);
                        }
                    }

                    if (ActionScript != null && !string.IsNullOrEmpty(ActionName))
                    {
                        if(ObjectToSend == null)
                            ActionScript.SendMessage(ActionName);
                        else 
                            ActionScript.SendMessage(ActionName, new[]{ObjectToSend, Object2ToSend});
                    }
                    if(DisableSelfOnAction)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        } else
        {
            if ((_disablePeriod -= Time.deltaTime) <= 0.0f)
            {
                _disablePeriod = DisablePeriod;
                doClick = false;
            }
        }
    }
}