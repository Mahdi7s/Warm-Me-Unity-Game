using UnityEngine;
using System.Collections;

public class ChLockStateScript : MonoBehaviour
{
    public static int LastSelectedChapter = 1;

    private GoToStateScript _goToStateScr = null;

    public int Chapter = 1;
    public GameObject LockObject = null;
    public Animator UnlockboardAnim = null;
    public Color FadeColor;
    public GotoChapterScript gotochapterscr;

    private Color _sprColor;
    private SpriteRenderer spr;

	public int UnlockPrice = 10000;

    public bool IsLocked
    {
        get { return GameState.IsChapterLocked(Chapter); }
    }

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        if (!GameState.IsChapterLocked(Chapter))
        {
            if (gotochapterscr!=null)
            {  
                gotochapterscr.GetComponent<SpriteRenderer>().color=new Color(1,1,1,1);
                gotochapterscr.canGo=true;
            }
            if (LockObject != null)
            {
                LockObject.SetActive(false);
            }
        }
        else
        {
            _sprColor = spr.color;
            spr.color = FadeColor;
        }
    }

    public void Unlock()
    {
        GameState.UnlockChapter(Chapter);
        if (LockObject != null)
        {
            LockObject.SetActive(false);
            spr.color = _sprColor;
        }
    }

    public void ShowBoard()
    {
        if (GameState.IsChapterLocked(Chapter))
        {
            LastSelectedChapter = Chapter;
            UnlockEnabilityScript unlockScr = UnlockboardAnim.GetComponent<UnlockEnabilityScript>();
            unlockScr.DisableSlide();
            unlockScr.SetGuiNumber(UnlockPrice);
            UnlockboardAnim.SetTrigger("In");
            var btnUnlock = UnlockboardAnim.GetComponentInChildren<BtnUnlockScript>();
            btnUnlock.LockStateScr = this;
        }
    }
}
