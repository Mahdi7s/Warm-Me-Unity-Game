using System;
using UnityEngine;
using System.Collections;

public class HatItemsContainerScript : MonoBehaviour
{
    // Use this for initialization
    public float minimumItemsToHit;                 // Specifies minimum number of items that should be hiited by user, to play a praise animation.
    public float goodItemsByTime;                   // Minimum number of good items that should be hitted in 1*_timeRatio second(s) to play a
    // good praise animation.
    public float excellentItemsByTime;              // Minimum number of good items that should be hitted in 1*_timeRatio second(s) to play a
    // excellent praise animation.
    public Animator praiseAnim;
    public Sprite[] excellent;
    public Sprite[] good;
    public Sprite[] dreadful;
    public SpriteRenderer spriteRenderer;

    private int goodRate, badRate;
    private float t;                // t stands for time.
    private ParticleSystem ps;
    private const float _timeRatio = 3.0f;

    void Start()
    {
        // Sets the particle system into varibale, then specifies it's attributes.
        ps = praiseAnim.gameObject.GetComponentInChildren<ParticleSystem>();
        ps.renderer.sortingLayerName = praiseAnim.renderer.sortingLayerName;
        ps.renderer.sortingOrder = praiseAnim.renderer.sortingOrder;
        // Reset all variables
        goodRate = badRate = 0;
        t = 0.0f;
    }

    void Update()
    {
        t += Time.deltaTime;    // Record elapsed time
    }

    // This function will call every time that a hat item hitted.
    public void RecordItem(bool positivity)
    {
        if (positivity)
        {
            if (goodRate <= 0)      // If it is the first good item
            {
                t = 0.0f;           // Reset the time
            }
            goodRate++;             // Increase number of good items
            badRate = 0;            // Reset bad items.
        }
        else                        // same procedure as above, for bad items
        {
            if (badRate <= 0)
            {
                t = 0.0f;
            }
            badRate++;
            goodRate = 0;
        }
        if (goodRate >= minimumItemsToHit)                                  // If good items has been hitted for some time
        {
            AnimatorStateInfo state = praiseAnim.GetCurrentAnimatorStateInfo(0);
            if (t > 0.0f && state.IsName("Idle"))                           // Check whether a praise animation is already playing
            {
                float gbyt = (float)(goodRate / (_timeRatio * t));          // gbyt = Good rate BY Time
                if (gbyt > excellentItemsByTime)                            // check if the excellent animation should be play
                {
                    Sprite currentsp = excellent[UnityEngine.Random.Range(0, excellent.Length)];
                    spriteRenderer.sprite = currentsp;
                    praiseAnim.SetTrigger("In");
                    ps.Play();
                }
                else if (gbyt > goodItemsByTime)                            // else if good animation should be play
                {
                    Sprite currentsp = good[UnityEngine.Random.Range(0, good.Length)];
                    spriteRenderer.sprite = currentsp;
                    praiseAnim.SetTrigger("In");
                    ps.Play();
                }
            }
            t = 0.0f;                                                       // Reset the timer
            goodRate = 0;                                                   // Reset the rate
        }
        else if (badRate >= minimumItemsToHit)                              // Else if bad items has been hitted for some time
        {                                                                   // same procedure as above, check it for more info.
            AnimatorStateInfo state = praiseAnim.GetCurrentAnimatorStateInfo(0);
            if (t > 0.0f && state.IsName("Idle"))
            {
                float bbyt = (float)(badRate / (_timeRatio * t));          // bbyt = Bad rate BY Time
                if (bbyt > goodItemsByTime)
                {
                    Sprite currentsp = dreadful[UnityEngine.Random.Range(0, dreadful.Length)];
                    spriteRenderer.sprite = currentsp;
                    praiseAnim.SetTrigger("In");
                    ps.Play();
                }
            }
            t = 0.0f;
            badRate = 0;
        }
    }
}