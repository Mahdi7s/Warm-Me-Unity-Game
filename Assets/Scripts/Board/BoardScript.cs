using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BoardScript : MonoBehaviour
{
    public AudioClip winAudio, loseAudio;
    public GameObject loseButtons, winButtons;
    public ParticleSystem intersectParticle, loserParticle;
    public Sprite loserSprite, winnerSprite, loserBoardSprite, winnerBoardSprite, loseTempratureSprite, losePipeBreakSprite;
    public SpriteRenderer textRenderer, loseImageRenderer, winImageRenderer;
    public Animator medalAnimator, winnerParticleAnimator;

    public SpriteRenderer Bronze, Silver, Gold;

    private GUINumberScript guiNumber;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Transform hud;
    private int _score = -1;
    private ChapterLevelScript chapterLevelScript;

    void Start()
    {
        chapterLevelScript = Camera.main.GetComponent<ChapterLevelScript>();
        guiNumber = GetComponentInChildren<GUINumberScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        hud = Camera.main.transform.FindChild("HUD");
        if (hud == null)
        {
            Debug.LogError("can not find HUD object");
        }

        Bronze.enabled = Silver.enabled = Gold.enabled = false;
    }

    void Update()
    {
        audio.mute = !GameState.AudioFx;
        if (_score > 0)
        {
            if (guiNumber.Number < _score)
            {
                guiNumber.Number += Mathf.Clamp(_score - guiNumber.Number, 0, _score / 150);
                if (guiNumber.Number == _score)
                {
                    winImageRenderer.gameObject.SetActive(true);
                    var pss = winImageRenderer.GetComponentInChildren<ParticleSystem>();
                    pss.renderer.sortingLayerName = winImageRenderer.renderer.sortingLayerName;
                    pss.renderer.sortingOrder = winImageRenderer.renderer.sortingOrder - 1;
                    medalAnimator.enabled = true;
                    _score = -1;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameState.ChangeTemperature(chapterLevelScript.MinWinTemprature);
            ShowScore();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameState.ChangeTemperature(-1 * chapterLevelScript.MinWinTemprature);
            ShowScore();
        }
    }

    public void ShowScore(bool lostBecauseOfPipes = false)
    {
		if (Camera.main.audio != null && Camera.main.audio.clip != null) {
			Camera.main.audio.Stop();
		}

        GameState.Pause();
        var minTemp = chapterLevelScript.MinWinTemprature;
        if (lostBecauseOfPipes)
        {
            OnLose(true);
        } else if (GameState.GetTemperature() < minTemp)
        {
            OnLose(false);
        } else
        {
            OnWin();
        }
        intersectParticle.Play();
        anim.SetTrigger("In");
        /*InitializeScript.Debugger = "Chapter= " + GameState.Chapter.ToString();
        InitializeScript.Debugger = "Level= " + GameState.Level.ToString();
        foreach (PurchasedItems pi in Enum.GetValues(typeof(PurchasedItems)))
        {
            if (StoreItemScript.dict.ContainsKey(pi))
            {
                InitializeScript.Debugger = pi.ToString() + "= " + StoreItemScript.dict[pi].ToString();
            }
        }
        InitializeScript.Debugger = "Level Time= "+cls.LevelDurationSecs+" + 30";
        InitializeScript.Debugger = "Last item time= " + HatItemScript.startTime;
        InitializeScript.Debugger = "------------------------------------------";
        InitializeScript.Save();*/
        GameState.Reset();
    }

    private void OnLose(bool lostBecauseOfPipes)
    {
        audio.clip = loseAudio;
        audio.Play();
        winImageRenderer.gameObject.SetActive(false);
        loseImageRenderer.gameObject.SetActive(true);
        if (lostBecauseOfPipes)
        {
            loseImageRenderer.sprite = losePipeBreakSprite;
        } else
        {
            loseImageRenderer.sprite = loseTempratureSprite;
        }
        guiNumber.gameObject.SetActive(false);
        spriteRenderer.sprite = loserBoardSprite;
        winButtons.SetActive(false);
        loseButtons.SetActive(true);
        loserParticle.Play();
        textRenderer.sprite = loserSprite;
        SaveScore(false, 0);
    }

    private void OnWin()
    {
        audio.clip = winAudio;
        audio.Play();
        
        loseImageRenderer.gameObject.SetActive(false);
        winImageRenderer.gameObject.SetActive(true);
        guiNumber.gameObject.SetActive(true);
        spriteRenderer.sprite = winnerBoardSprite;
        winButtons.SetActive(true);
        loseButtons.SetActive(false);
        winnerParticleAnimator.enabled = true;
        textRenderer.sprite = winnerSprite;
        if (chapterLevelScript.IsWordCompleted())
        {
            GameState.LevelCoins += 50;
        }
        _score = GameState.LevelCoins;
        SaveScore(true, _score);
    }

    private void SaveScore(bool win, int lvlCoins)
    {
        hud.gameObject.SetActive(false);
        LevelModel lastLevel = GameState.GetLastPlayedLevel(true);
        if (win)
        {
            var skill = (chapterLevelScript.HitPositiveCount * 100 / chapterLevelScript.PositiveItemsCount);
            var cup = new LevelModel{ Skill = skill, LevelCoins=lvlCoins}.LevelCup;
            switch (cup)
            {
                case LevelCup.Bronze:
                    Bronze.enabled = true;
                    break;
                case LevelCup.Silver:
                    Silver.enabled = true;
                    break;
                case LevelCup.Golden:
                    Gold.enabled = true;
                    break;
            }

            if ((!lastLevel.Win) || (skill >= lastLevel.Skill))
            {
                lastLevel.Win = true;
                lastLevel.LevelCoins = lvlCoins;
                lastLevel.WordCompleted = chapterLevelScript.IsWordCompleted();
                lastLevel.Skill = skill;

                GameState.CurrentLevelPurchases = null;
                GameState.LastFailPurchasedItems = null;
                GameState.UpdateLevel(lastLevel);
            }
        } else
        {
            if (!lastLevel.Win)
            {
                lastLevel.WordCompleted = lastLevel.Win = false;
                lastLevel.LevelCoins = 0;
                GameState.UpdateLevel(lastLevel);
            }
            GameState.LastFailPurchasedItems = GameState.CurrentLevelPurchases;
        }
    }
}