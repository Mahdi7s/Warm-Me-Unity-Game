using UnityEngine;
using System.Collections;

public class SandTimeScript : PausableBehaviour
{
    public BoardScript boardScript;
    public Collider2D menuIconCollider;
    public Sprite[] sandWatchSprites;
    //[Tooltip("This is the delay after instantiating last item.")]
    private float delay = 4;

    private int levelInstantiatedItems, numberOfSandWatchSamples, index;
    private bool initialized, timer;
    private ChapterLevelScript chapterLevelScript;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    private Animator anim;
    // This function will be called from ChapterLevelScript.StartLevel() function.
    public void Initialize(ChapterLevelScript chapterLevelScript)
    {
        this.chapterLevelScript = chapterLevelScript;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        particleSystem.renderer.sortingLayerName = spriteRenderer.sortingLayerName;
        particleSystem.renderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        particleSystem.Play();
        numberOfSandWatchSamples = sandWatchSprites.Length;
        index = 0;
        anim = GetComponent<Animator>();
        spriteRenderer.sprite = sandWatchSprites [0];
        initialized = true;
    }

    protected override void PUpdate()
    {
        if (initialized)
        {
            if (timer)                  // This is for last item, after instantiating last item, we should wait some time, then
            {                           // finish the game.
                delay -= Time.fixedDeltaTime;// Delay after instantiating last item
                if (delay <= 0f)
                {
                    particleSystem.Stop();
                    menuIconCollider.enabled = false;
                    boardScript.ShowScore();
                    initialized = false;
                    //InitializeScript.Debugger = levelInstantiatedItems.ToString() + '/' + levelMaxItems.ToString();
                }
            }
            else
            {
                levelInstantiatedItems = chapterLevelScript.InstantiatedItems;
                index = (numberOfSandWatchSamples * levelInstantiatedItems) / chapterLevelScript.ItemsCount;
                if (index < numberOfSandWatchSamples)
                {
                    if (index == numberOfSandWatchSamples - 1)
                    {
                        anim.SetTrigger("End");
                    }
                    spriteRenderer.sprite = sandWatchSprites[index];
                }

                if (levelInstantiatedItems == chapterLevelScript.ItemsCount)
                {
                    timer = true;
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
}