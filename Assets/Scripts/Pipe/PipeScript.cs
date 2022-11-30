using UnityEngine;
using System.Collections;

public class PipeScript : PausableBehaviour
{
    public GameObject breakWater;
    public Vector2 relativePosition;

    private ParticleSystem breakWaterParticle;
    private PipesParentScript pipesParentScript;
    private Animator anim;
    private float breakWaterStep;
    private bool notDamaged;
    private int damage, lastDamage, pipeNo;

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    void Start()
    {
        notDamaged = true;
        damage = lastDamage = 0;        // Specifies damage degree
        pipeNo = int.Parse(name.Substring(1));

        anim = GetComponent<Animator>();
        pipesParentScript = GetComponentInParent<PipesParentScript>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 position = transform.position, rotation = transform.rotation.eulerAngles;

        rotation.x = rotation.y = 0f;
        position.x += relativePosition.x;
        position.y += relativePosition.y;

        GameObject waterParticle = (GameObject)Instantiate(breakWater, position, Quaternion.Euler(rotation));   // Instantiate break water game object that shows splashing water from damaged pipes.
        waterParticle.transform.parent = transform;
        waterParticle.SetActive(true);
        breakWaterParticle = waterParticle.GetComponentInChildren<ParticleSystem>();

        //breakWaterParticle.renderer.sortingLayerName = "Fire";
        //breakWaterParticle.renderer.sortingOrder = 10;

        breakWaterStep = breakWaterParticle.emissionRate / 4f; // Each pipe has 4 damage triggers (25-50-75-100). So break water particle should have 4 diffrent values.

        breakWaterParticle.emissionRate = 0f;                   // at first pipes are not damaged, so we have no water splashing from them.
    }

    protected override void PUpdate()
    {
        if (lastDamage < damage)                                // If the animation of breaking pipe is not compeletely played (See pipes animator and its transition conditions)
        {
            lastDamage += 25;                                   // damage the pipe one step further
            anim.SetInteger("Damage", lastDamage);              // play its animation
            breakWaterParticle.emissionRate += breakWaterStep;  // set the splashing water size according to damage
            ++pipesParentScript.DamagesCount;                   // update total damage counts (see Pipes Parent Script for more info)
        }
    }

    /// <summary>
    /// This method simulates pipes reaction to an item hit event.
    /// </summary>
    public void PipeReaction(int hatNo, int temperature, int explosionPower)
    {
        if (pipeNo == hatNo)             // If this is the pipe of hat
        {
            if (temperature > 0)
            {
                anim.SetTrigger("IsUp");
            }
            else if (temperature < 0)
            {
                anim.SetTrigger("IsDown");
            }
            lastDamage = damage;
            damage += explosionPower;
            if (notDamaged && damage > 0)
            {
                GameState.EnqueueDamagedPipe(this);     // Add this to damaged pipes (this queue will use by wrench later)
                notDamaged = false;
            }
            if (damage > GameSettings.maxPipeDamage)
            {
                damage = GameSettings.maxPipeDamage;
            }
        }
    }

    /// <summary>
    /// This method will repair a pipe compeletely.
    /// </summary>
    public void RepairPipe()
    {
        notDamaged = true;
        pipesParentScript.DamagesCount -= damage / 25;
        damage = lastDamage = 0;
        anim.SetInteger("Damage", 0);
        breakWaterParticle.emissionRate = 0;
    }

    protected override void PFixedUpdate() { }
    protected override void POnGUI() { }
}