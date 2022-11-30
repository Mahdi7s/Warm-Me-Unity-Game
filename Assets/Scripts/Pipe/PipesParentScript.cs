using UnityEngine;
using System.Collections;

public class PipesParentScript : PausableBehaviour
{
    public BoardScript boardScript;
    public Transform showerSpriteTransform;
    public int maximumDamages = 5;
    public float delay = 3.0f;
    private Vector3 showerSpriteScale = Vector3.zero;
    private float initialXofShowerSpriteTransform = 0, delayTimer;
    private int damagesCount;
    private bool timer;

    public int DamagesCount
    {
        get
        {
            return damagesCount;
        }
        set
        {
            damagesCount = value;
            showerSpriteScale.x = (1f - ((float)damagesCount / maximumDamages)) * initialXofShowerSpriteTransform;    // Set the shower sprite size, according to total damage
            if (showerSpriteScale.x <= 0f)                  // if the game was over
            {
                timer = true;                               // This timer implement a delay before finishing the game (So cigarette has enough time to explode!)
                showerSpriteScale.x = 0f;                   // Resets shower sprite scale
            }
            showerSpriteTransform.localScale = showerSpriteScale;   // Apply changes to the shower sprite
        }
    }

    void Awake()
    {
        delayTimer = delay;
        GameState.PipeScripts = gameObject.GetComponentsInChildren<PipeScript>();
        showerSpriteScale = showerSpriteTransform.localScale;
        initialXofShowerSpriteTransform = showerSpriteScale.x;      // This is maximum size of shower sprite that it can be
    }

    protected override void PUpdate()
    {
        if (timer)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
            {
                if (showerSpriteScale.x <= 0f)
                {
                    boardScript.ShowScore(true);        // Finish the game!
                }
                else
                {
                    timer = false;
                    delayTimer = delay;
                }
            }
        }
    }

    protected override void PFixedUpdate() { }
    protected override void POnGUI() { }
}