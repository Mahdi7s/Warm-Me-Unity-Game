using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class EndChapterScript : MonoBehaviour
{
    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer;                                   // Scene background image renderer
    public Sprite[] chapter = new Sprite[GameSettings.maxNumberOfChapters];     // Scene background images for each chapter
    [Header("Qoate Settings")]
    // All renderers of all words, for each scentence 9 sprites renderers should be add to this array.
    public SpriteRenderer[] wordsRenderers = new SpriteRenderer[GameSettings.maxNumberOfChapters * GameSettings.maxLevelsPerChapter];
    // All empty sprites of all words, for each scentence 9 sprites should be add to this array.
    public Sprite[] emptyQuotes = new Sprite[GameSettings.maxNumberOfChapters * GameSettings.maxLevelsPerChapter];
    public Transform quotes;
    public Transform levelsCup;
    private List<LevelModel> lvls = null;

	public int ScoreOnWordsComplete = 1000;
	public GameObject WordsCompleteObject = null;
    public GameObject WordsNotCompleteObject = null;

    public GUINumberScript BonusScr = null;

    void Start()
    {
        backgroundRenderer.sprite = chapter[GameState.Chapter - 1]; // set the background
        GameObject quote;                                           // a temp variable
        for (int cntr = 0; cntr < quotes.childCount; cntr++)
        {
            quote = quotes.GetChild(cntr).gameObject;
            if (int.Parse(quote.name.Substring(6, 1)) != GameState.Chapter)  // if quote game object is not this chapter quote
            {
                quote.SetActive(false);                                     // deactive it
            }
        }
        lvls = GameState.LevelModelList.Where(x => x.Chapter == GameState.Chapter).ToList(); // And I used LINQ at last. FUCK.
        foreach (LevelModel lm in lvls)
        {
            if (lm != null)
            {
                int level = lm.Level - 1; // array index is start from 0, so we should decrease the level
                if (!lm.WordCompleted)
                {
                    HideWord(level);
                }
                Transform temp = levelsCup.GetChild(level).GetChild(0);
                switch (lm.LevelCup)
                {
                    case LevelCup.Bronze:
                        temp.GetChild(0).gameObject.SetActive(false);
                        temp.GetChild(1).gameObject.SetActive(false);
                        temp.GetChild(2).gameObject.SetActive(true);
                        break;
                    case LevelCup.Silver:
                        temp.GetChild(0).gameObject.SetActive(false);
                        temp.GetChild(1).gameObject.SetActive(true);
                        temp.GetChild(2).gameObject.SetActive(false);
                        break;
                    case LevelCup.Golden:
                        temp.GetChild(0).gameObject.SetActive(true);
                        temp.GetChild(1).gameObject.SetActive(false);
                        temp.GetChild(2).gameObject.SetActive(false);
                        break;
                }
            }
        }

		// show and plus score if filled all words
        if (lvls.Any(x => x.WordCompleted))
        {
            var bonus = lvls.Sum(x => x.WordCompleted ? 100 : 0);
            var savedBonus = GameState.GetChapterBonus(GameState.Chapter);
            if (bonus != savedBonus)
            {
                GameState.SetChapterBonus(GameState.Chapter, bonus);
                GameState.ChangeStoreCoins(GameState.GetStoreCoins() + (bonus - savedBonus));
            }
            var bb = (bonus - savedBonus);
            BonusScr.Number = bb;
            BonusScr.EditorNum = bb;
            WordsCompleteObject.SetActive(true);
        }
        else
        {
            WordsNotCompleteObject.SetActive(true);
        }
    }

    public void HideWord(int index)
    {
        index = (GameState.Chapter - 1) * GameSettings.maxLevelsPerChapter + index;     // calculate index of word renderer and empty quote
        wordsRenderers[index].sprite = emptyQuotes[index];
    }
}