using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelsMenuScript : MonoBehaviour
{
    private LvlMenuScript[] _levels = null;
    private List<LevelModel> _lvlModels = null;
    //-----------------------------------------------

    public GameObject LevelsContainer = null;
    public Transform medalContainer;

    public Sprite[] medals;

    void Start()
    {
        EmbededMobileBackButtonScript.lastScene = "Chapters";
        _levels = LevelsContainer.GetComponentsInChildren<LvlMenuScript>().OrderBy(x => (x.Chapter * 100) + x.Level).ToArray();
        _lvlModels = GameState.LevelModelList;

        var isLastActive = false;
        foreach (var lvl in _levels)
        {
            var model = _lvlModels.FirstOrDefault(x => x.Chapter == lvl.Chapter && x.Level == lvl.Level);
            if (model != null)
            {
                lvl.Initialize(model, true);
                isLastActive = model.Win;
            } else
            {
                if (!isLastActive)
                {
                    isLastActive = lvl.Level == 1 && (lvl.Chapter == 1 || !GameState.IsChapterLocked(lvl.Chapter) || (lvl.Chapter > 1 && _lvlModels.Any(x => x.Chapter == lvl.Chapter - 1 && x.Level == 9)));
                }
                lvl.Initialize(model, isLastActive);

                isLastActive = false;
            }
        }
    }
		
    void Update()
    {
        var tColl = TouchUtility.GetTouchedCollider();
        if (tColl != null)
        {
            var lvl = _levels.FirstOrDefault(x => x.gameObject == tColl.gameObject);
            if (lvl != null)
            {
                lvl.OnHit();
            }
        }
    }

    public SpriteRenderer GetMedalSpriteRenderer(int index)
    {
        return medalContainer.GetChild(index).GetComponent<SpriteRenderer>();
    }

    public Sprite GetSprite(LevelCup levelcup)
    {
        if (levelcup != LevelCup.None)
            return medals [(int)levelcup];
        return null;
    }
}