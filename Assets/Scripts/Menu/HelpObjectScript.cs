using UnityEngine;
using System.Collections;

public class HelpObjectScript : MonoBehaviour
{
    public Sprite helpHitItems, helpThermometer, helpTime;
    public Collider2D cancel, resume;

    private int item;
    private bool clicked;
    private SpriteRenderer spriteRenderer;
    private Animator resumeAnim;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        resumeAnim = resume.GetComponent<Animator>();
    }

    void Update()
    {
        Collider2D temp = TouchUtility.GetTouchedCollider(false);
        if (temp == resume)
        {
            
            clicked = true;
            item++;
            item %= 3;
            switch (item)
            {
                case 0:
                    spriteRenderer.sprite = helpHitItems;
                    break;
                case 1:
                    spriteRenderer.sprite = helpThermometer;
                    break;
                case 2:
                    spriteRenderer.sprite = helpTime;
                    break;
            }
        }
    }
}
