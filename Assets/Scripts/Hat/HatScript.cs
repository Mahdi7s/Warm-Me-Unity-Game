using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum HatState
{
    Normal,
    Outing,
    Special,
    Inning
}

public class HatScript : PausableBehaviour
{
    public ChapterLevelScript chapterLevelScript = null;
    public GameObject HatChild = null;
    public ParticleSystem OutParticle = null;
    public Transform ItemTransform, hatLightTransform;
    public Vector2 hatLightRelPos;
    public float scoreShowTime;                     // Every time that an egg hitted, it will get user some score. This variable specifies the time that the score
    // should be displayed.
    private Animator _hatAnim = null;
    private GameObject _currentItem = null;
    private HatItemScript _currentItemScript = null;
    private Animator _currentItemAnim = null;
    private GameObject _nextItem = null;
    private bool _shouldFall = false, isFalling = false, magicWand = false; // _shouldFall: determine whether if the mouse has been arrived to the hat, and wants to
    // fall it.
    // isFalling: determine if the hat is falling bacuase of a mouse. In this situation the hat should not
    // instantiate any new item.
    private GUINumberScript gUINumberScript;        // GUI Script that will show the score that is earned by hitting eggs.
    private Queue<int> coins;                       // This is a queue of coins that is earned by hitting eggs. Every time that a egg hitted, the score will enqueue
    // in this variable.
    private float elapsedShowTime;                  // Specifies the time has been elapsed from score show time.
    private ParticleSystem hatLightParticle;

    public HatState State { get; private set; }
    public bool BoxOfMatches { get; set; }          // Specifies whether a match box is hitted.

    public float TestSpeed = 1.0f;

    // This bool property will determine if the magic wand item has been hitted for this hat.
    public bool MagicWand
    {
        get
        {
            return magicWand;
        }
        set
        {
            magicWand = value;
            if (value)
            {
                hatLightParticle.Play();
                //Debug.Log("Played.");
            }
            else
            {
                hatLightParticle.Stop();
                //Debug.Log("Stoped.");
            }
        }
    }

    // If a match box hitted for this hat, this will specify match box runner script.
    public MatchBoxRunnerScript MatchBoxRunner
    {
        get;
        set;
    }

    public GameObject NextItem
    {
        get
        {
            return _nextItem;
        }
        set
        {
            if (_nextItem == null)
            {
                _nextItem = value;
                if (MagicWand)
                {
                    var item = _nextItem.GetComponent<HatItemScript>();
                    if (!item.Kind.IsPositiveItem())
                    {
                        var randPositiveKind = HatItemKind.None.GetRandomPositive();
                        var newItem = chapterLevelScript.HatItems.FirstOrDefault(x => x.Kind == randPositiveKind);
                        _nextItem = newItem.InstantiateNew(this, chapterLevelScript.ItemNormalSeconds());
                    }
                }
            }
        }
    }

    public bool CanShowNewItem(string newKind)
    {
        bool check = (_nextItem == null && _currentItem == null && !_shouldFall && _hatAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        if (check && !string.IsNullOrEmpty(newKind))
        {
            if (MagicWand)
            {
                if (newKind.Contains(PurchasedItems.MagicWand.ToString())) return false;

                /*
                HatItemKind hiKind;
                PurchasedItems piKind;
                if (HatItemKind.None.GetItemEnum(newKind, out hiKind, out piKind))
                {
                    if (hiKind.IsPositiveItem()) return true;
                }
                 */
            }
            if (BoxOfMatches)
            {
                if (newKind.Contains(PurchasedItems.BoxOfMatches.ToString())) return false;
            }
            return true;
        }
        else return false;
    }

    // Specifies number of a hat.
    public int HatNo
    {
        get;
        private set;
    }

    void Start()
    {
        TestSpeed = GetComponentInChildren<Animator>().speed;
        Vector3 position = transform.position;
        position.x += hatLightRelPos.x;
        position.y += hatLightRelPos.y;
        var hatLight = (Transform)(Instantiate(hatLightTransform, position, Quaternion.identity));
        hatLight.parent = transform;
        hatLight.Rotate(new Vector3(0, 0, 1) * transform.rotation.eulerAngles.z);
        hatLightParticle = hatLight.GetComponentInChildren<ParticleSystem>();
        hatLight.transform.position = transform.position;
        //hatLightParticle.transform.parent = transform;
        hatLightParticle.renderer.sortingLayerName = "Match";
        hatLightParticle.renderer.sortingOrder = -1;
        coins = new Queue<int>();
        gUINumberScript = GetComponentInChildren<GUINumberScript>();
        gUINumberScript.Hide();
        _hatAnim = HatChild.GetComponent<Animator>();
        HatNo = int.Parse(gameObject.name.Substring(3));

        if(chapterLevelScript == null)
            chapterLevelScript = FindObjectOfType<ChapterLevelScript>();
    }

    protected override void PUpdate()
    {
        if (!isFalling)
        {
            if (_currentItem == null)
            {
                if (_shouldFall)
                {
                    // Kill every thing under the game object, but the hat itself.
                    for (int cntr = 0; cntr < transform.childCount; cntr++)
                    {
                        Transform trnsfrm = transform.GetChild(cntr);
                        if (trnsfrm.gameObject.name != "hat")
                        {
                            KillTransformAndItsChildren(trnsfrm);
                        }
                    }
                    _hatAnim.SetTrigger("Fall");            // Play falling animation of hat.

                    transform.rotation = Quaternion.identity;

                    Destroy(gameObject, 5.0f);
                    isFalling = true;
                }
                else if (_nextItem != null)
                {
                    _currentItem = _nextItem;
                    _currentItem.transform.localScale = Vector3.one;
                    _currentItemScript = _currentItem.GetComponent<HatItemScript>();
                    SetCurrentItemAnimator();
                    _currentItemAnim.SetBool("Out", true);
                    _hatAnim.SetBool("In", false);
                    _hatAnim.SetBool("Out", true);
                    OutParticle.Play();
                    _nextItem = null;
                }
            }

            if (_currentItem != null)
            {
                var animState = _currentItemAnim.GetCurrentAnimatorStateInfo(0);

                if (animState.IsName("Out"))
                {
                    State = HatState.Outing;
                }
                else if (animState.IsName("Normal"))
                {
                    State = HatState.Normal;
                }
                else if (animState.IsName("In"))
                {
                    State = HatState.Inning;
                }
                else if (animState.IsName("End"))
                {
                    GameObject.Destroy(_currentItem);
                    if (_currentItemScript.PlayInAnimAfterSpecial)
                    {
                        _hatAnim.SetBool("In", true);
                        _hatAnim.SetBool("Out", false);
                    }
                    _currentItem = null;
                    _currentItemAnim = null;
                    _currentItemScript = null;
                    if (_shouldFall)
                    {
                        // Kill every thing under the game object, but the hat itself.
                        for (int cntr = 0; cntr < transform.childCount; cntr++)
                        {
                            if (transform.GetChild(cntr).gameObject.name != "hat")
                            {
                                KillTransformAndItsChildren(transform.GetChild(cntr));
                            }
                        }
                        _hatAnim.SetTrigger("Fall");            // Play falling animation of hat.
                        Destroy(gameObject, 5.0f);
                        isFalling = true;
                    }
                }
            }
        }
        if (elapsedShowTime > 0f)                               // if the score should be show
        {
            elapsedShowTime -= Time.deltaTime;
            if (elapsedShowTime <= 0f)                          // if the score should not be shown
            {
                gUINumberScript.Number = 0;
                gUINumberScript.Hide();
                elapsedShowTime = 0f;
            }
        }
        if (coins.Count > 0)                                    // if there is a score in score queue (coins queue)
        {
            gUINumberScript.Number += coins.Dequeue();          // Get the first score
            gUINumberScript.Show();                             // show it
            elapsedShowTime += scoreShowTime;                   // Set the show time
        }
    }

    protected override void PFixedUpdate()
    {

    }

    protected override void POnGUI()
    {

    }

    private void SetCurrentItemAnimator()
    {
        _currentItemAnim = _currentItemScript.AnimationChild.GetComponent<Animator>();
    }

    // This function will be called by mouse. The hat will be fall when the item in that get back to hat.
    public void Fall()          
    {
        if (!_shouldFall)
        {
            _shouldFall = true;
            chapterLevelScript.HatIsFalling();
        }
    }

    // Specifies that a hat is already faliing or not. Used by mouses, to find out their target is already falling or not.
    public bool IsFalling()
    {
        return isFalling;
    }

    // A generic function that will destroy all child of a transfrom recursively, and the transform it self. This function has been used in many diffrent
    // scripts. You can move it to one of utility scripts.
    public static void KillTransformAndItsChildren(Transform transform, float time = 0.0f)
    {
        if (transform.childCount <= 0)
        {
            if (time > 0.0f)
            {
                Destroy(transform.gameObject, time);
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
        else
        {
            for (int cntr = 0; cntr < transform.childCount; cntr++)
            {
                if (time > 0.0f)
                {
                    KillTransformAndItsChildren(transform.GetChild(cntr), time);
                }
                else
                {
                    KillTransformAndItsChildren(transform.GetChild(cntr));
                }
            }
        }
    }

    // Enqueue a score in the score queue.
    public void EnqueueCoin(int score)
    {
        coins.Enqueue(score);
    }
}