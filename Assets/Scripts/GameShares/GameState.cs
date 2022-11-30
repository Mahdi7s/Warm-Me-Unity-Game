using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class GameState
{
    private static int totalCoins, levelCoins;
    private static float temperature;
    private static string temp;
    private static Queue<PipeScript> damagedPipeScriptsQueue = new Queue<PipeScript>();
    private static Dictionary<string, string> dictionary = new Dictionary<string, string>();
    private static List<LevelModel> levelModelList = null;
    private static Dictionary<PurchasedItems, int> lastFailPurchasedItems = new Dictionary<PurchasedItems, int>();
    private static bool _audioMusic = true, _audioFx = true;
    public static Transform staticProgressBarPrefab;
    public static Vector3 progressBarPosition;
    public static int Combo = 1;
    public static PauseScript pauseScript;
    public static Animator backgroundFade;
    public static float itemNormalSeconds{ set; get; }
    private static bool isPausedWithAnim;
    public static int CurrentChapter = 1;

    private static string GetPropertyValue(string propertyName)
    {
        if (dictionary.TryGetValue(propertyName, out temp))
        {
            return temp;
        } else
        {
            string propertyNameEncrypted = CypherScript.Encrypt(propertyName);
            temp = PlayerPrefs.GetString(propertyNameEncrypted);
            if (temp != "")
            {
                temp = CypherScript.Decrypt(temp);
                dictionary.Add(propertyName, temp);
            }
            return temp;
        }
    }
    
    private static void SetPropertyValue(string propertyName, string value)
    {
        if (propertyName != "" && value != "")
        {
            if (dictionary.TryGetValue(propertyName, out temp))
            {
                if (temp != value)
                {
                    dictionary [propertyName] = value;
                    propertyName = CypherScript.Encrypt(propertyName);
                    value = CypherScript.Encrypt(value);
                    PlayerPrefs.SetString(propertyName, value);
                    PlayerPrefs.Save();
                }
            } else
            {
                dictionary.Add(propertyName, value);
                propertyName = CypherScript.Encrypt(propertyName);
                value = CypherScript.Encrypt(value);
                PlayerPrefs.SetString(propertyName, value);
                PlayerPrefs.Save();
            }
        }
    }

    public static void IntializeProperties()
    {
        if (GetPropertyValue("IsFirstLoad") == "" || GetPropertyValue("IsFirstLoad") == "true")
        {
            SetPropertyValue("Chapter", "1");
            SetPropertyValue("Level", "1");
            SetPropertyValue("AdMobEnabled", "true");
            SetPropertyValue("TotalCoins", "0");
            SetPropertyValue("LevelModels", (new LevelModel()).ToString());
            InitLockStates();

            SetPropertyValue("IsFirstLoad", "false");
        }
        //AudioMusic = AudioFx = true;
    }

    #region Levels Data Codes

    public static List<LevelModel> LevelModelList
    {
        get
        {
            if (levelModelList == null || !levelModelList.Any())
            {
                levelModelList = new List<LevelModel>();
                string levelModels = GetPropertyValue("LevelModels");
                if (!string.IsNullOrEmpty(levelModels))
                {
                    LevelModel temp;
                    string[] chapterLevelCoinArray = levelModels.Split('|');
                    for (int cntr = 0; cntr < chapterLevelCoinArray.Length; cntr++)
                    {
                        temp = new LevelModel(chapterLevelCoinArray [cntr]);
                        levelModelList.Add(temp);
                    }
                }
            }
            if (levelModelList == null || !levelModelList.Any())
            {
                levelModelList.Add(new LevelModel());
                LevelModelList = levelModelList;
            }
            return levelModelList;
            #region Level Model Definitions
            /*
            return new List<LevelModel>(new[]{
                new LevelModel{ Chapter=1, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=1, Level=9, Win=true,LevelCoins=100},

                new LevelModel{ Chapter=2, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=2, Level=9, Win=true,LevelCoins=100},

                new LevelModel{ Chapter=3, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=3, Level=9, Win=true,LevelCoins=100},

                new LevelModel{ Chapter=4, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=4, Level=9, Win=true,LevelCoins=100},

                new LevelModel{ Chapter=5, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=5, Level=9, Win=true,LevelCoins=100},

                new LevelModel{ Chapter=6, Level=1, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=2, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=3, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=4, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=5, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=6, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=7, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=8, Win=true,LevelCoins=100},
                new LevelModel{ Chapter=6, Level=9, Win=true,LevelCoins=100}
            });*/
            #endregion
        }
        set
        {
            if (value != null && value.Any())
            {
                String levelModels = string.Join("|", value.GroupBy(x => x.Chapter + "-" + x.Level).Select(x => x.First().ToString()).ToArray());
                SetPropertyValue("LevelModels", levelModels);
                levelModelList = value;
            } else
            {
                levelModelList.Clear();
            }
        }
    }

    public static LevelModel GetLastPlayedLevel(bool lastOfCurrent = false)
    {
        LevelModel retval = null;
        if (!lastOfCurrent)
        {
            if (GameState.LevelModelList != null && GameState.LevelModelList.Any())
            {
                retval = GameState.LevelModelList.OrderBy(x => (x.Chapter * 100) + x.Level).LastOrDefault();
            }
        } else
        {
            retval = GameState.LevelModelList.FirstOrDefault(x => x.Chapter == Chapter && x.Level == Level);
        }

        if (retval == null)
        {
            retval = new LevelModel(); // the 1st chapter & 1st level !
        }

        return retval;
    }

    public static bool IsChapterLocked(int chapter)
    {
        var locked = GetPropertyValue(string.Format("ChLocked#{0}", chapter));
        var retval = locked == "Yes";
        if (retval)
        {
            var lastLevel = GetLastPlayedLevel();
            if (lastLevel.Level == 9 && lastLevel.Chapter + 1 >= chapter)
            {
                UnlockChapter(chapter);
                retval = false;
            }
        }
        return retval;
    }
    
    public static void UnlockChapter(int chapter)
    {
        SetPropertyValue(string.Format("ChLocked#{0}", chapter), "No");
    }

    private static void InitLockStates()
    {
        SetPropertyValue(string.Format("ChLocked#{0}", 1), "No");
        SetPropertyValue(string.Format("ChLocked#{0}", 2), "Yes");
        SetPropertyValue(string.Format("ChLocked#{0}", 3), "Yes");
        SetPropertyValue(string.Format("ChLocked#{0}", 4), "Yes");
        SetPropertyValue(string.Format("ChLocked#{0}", 5), "Yes");
        SetPropertyValue(string.Format("ChLocked#{0}", 6), "Yes");
    }

    public static void UpdateLevel(LevelModel model)
    {
        var levels = LevelModelList;
        var removedCount = levels.RemoveAll(x => x.Chapter == model.Chapter && x.Level == model.Level);
        levels.Add(model);

        LevelModelList = levels;
    }

    public static void OpenLevelIfNotOpened(int chapter, int level)
    {
        if (!LevelModelList.Any(x => x.Chapter == chapter && x.Level == level))
        {
            var model = LevelModelList;
            model.Add(new LevelModel{ Chapter=chapter, Level=level, Win=false});
            LevelModelList = model;
        }
    }

    public static void DeleteLevel(LevelModel model)
    {
        var levels = LevelModelList;
        levels.RemoveAll(x => x.Chapter == model.Chapter && x.Level == model.Level);
    }   

    #endregion

    public static Dictionary<PurchasedItems, int> CurrentLevelPurchases
    {
        get;
        set;
    }

    public static Dictionary<PurchasedItems, int> LastFailPurchasedItems
    {
        get
        {
            if (lastFailPurchasedItems == null || !lastFailPurchasedItems.Any())
            {
                if (GetPropertyValue("LastFailPurchasedItems") == string.Empty)
                {
                    SetPropertyValue("LastFailPurchasedItems", string.Empty);
                }
                var str = GetPropertyValue("LastFailPurchasedItems");
                var keyvalues = str.Split(new[]{"|"}, StringSplitOptions.RemoveEmptyEntries);
                lastFailPurchasedItems = keyvalues.Select(x => x.Split('-')).ToDictionary(x => (PurchasedItems)Enum.Parse(typeof(PurchasedItems), x [0]), x => int.Parse(x [1]));
            }
            return lastFailPurchasedItems;
        }
        set
        {
            //string toSave = string.Empty;
            if (value == null || !value.Any())
            {
                lastFailPurchasedItems.Clear();
            } else
            {
                //var items = value.Aggregate(string.Empty, (curr, item) => curr + string.Format("{0}-{1}|", item.Key.ToString(), item.Value));
                lastFailPurchasedItems = value;
            }
            SetPropertyValue("LastFailPurchasedItems", string.Empty);
        }
    }

    public static int Chapter
    {
        get
        {
            return int.Parse(GetPropertyValue("Chapter"));
        }
        set
        {
            SetPropertyValue("Chapter", value.ToString());
        }
    }

    public static int Level
    {
        get
        {
            return int.Parse(GetPropertyValue("Level"));
        }
        set
        {
            SetPropertyValue("Level", value.ToString());
            //Time.timeScale = Mathf.Pow(1.1f, value - 1);
        }
    }

    public static void SetChapterBonus(int chapter, int bonus)
    {
        SetPropertyValue(string.Format("Bonus#{0}", chapter), bonus.ToString());
    }

    public static int GetChapterBonus(int chapter)
    {
        var key = string.Format("Bonus#{0}", chapter);
        if (PlayerPrefs.HasKey(CypherScript.Encrypt(key)))
        {
            return int.Parse(GetPropertyValue(string.Format("Bonus#{0}", chapter)));
        }
        return 0;
    }

    public static DateTime LastFreeCoinChanceTime
    {
        get
        { 
            var freeCoinChanceTimeStr = GetPropertyValue("LastFreeCoinChanceTime");
            if (!string.IsNullOrEmpty(freeCoinChanceTimeStr))
            {
                return DateTime.Parse(freeCoinChanceTimeStr);
            }
            return DateTime.MinValue;
        }
        set
        {
            string dateTime = value.ToString();
            SetPropertyValue("LastFreeCoinChanceTime", dateTime);
        }
    }

    public static ScoreScript scoreScript
    {
        get;
        set;
    }

    public static PipeScript[] PipeScripts
    {
        get;
        set;
    }

    public static bool AdMobEnabled
    {
        get
        {
            return bool.Parse(GetPropertyValue("AdMobEnabled"));
        }
        set
        {
            SetPropertyValue("AdMobEnabled", value.ToString());
        }
    }

    public static bool AudioFx
    {
        get
        {
            return _audioFx;
        }       
        set
        {
            _audioFx = value;
        }
    }

    public static bool AudioMusic
    {
        get
        {
            return _audioMusic;
        }       
        set
        {
            _audioMusic = value;
        }
    }

    public static void ChangeTemperature(float tempChange)
    {
        var chapterScr = Camera.main.GetComponent<ChapterLevelScript>();
        temperature = Mathf.Clamp(temperature + tempChange, 0, chapterScr.MaxTemprature);
    }

    public static float GetTemperature()
    {
        return temperature;
    }

    public static void ChangeStoreCoins(int coins)
    {
        if (totalCoins >= 0)
        {
            SetPropertyValue("TotalCoins", coins.ToString());
        }
    }

    public static int GetStoreCoins()
    {
        var tStr = GetPropertyValue("TotalCoins");
        int retval = 0;
        if (int.TryParse(tStr, out retval))
        {

        }
        return retval;
    }

    public static int GetAllCoins()
    {
        return GetStoreCoins() + GetAllCoinsWithoutStore();
    }

    public static int GetAllCoinsWithoutStore()
    {
        return LevelModelList.Sum(x => x.LevelCoins);
    }

    public static int LevelCoins
    {
        set
        {
            levelCoins = value;
            scoreScript.UpdateScore();
        }
        get
        {
            return levelCoins;
        }
    }

    public static void EnqueueDamagedPipe(PipeScript ps)
    {
        damagedPipeScriptsQueue.Enqueue(ps);
    }

    public static PipeScript DequeueDamagedPipe()
    {
        if (damagedPipeScriptsQueue.Count > 0)
        {
            PipeScript ps = damagedPipeScriptsQueue.OrderByDescending(x => x.Damage).First();
            return ps;
        }
        return null;
    }

    public static bool Pause(bool withMenuAnim = false)
    {
        if (pauseScript != null && !pauseScript.IsGamePaused())
        {
            pauseScript.PauseAll();
            if (withMenuAnim)
            {
                pauseScript.InGameMenuAnim.SetBool("Out", false);
                pauseScript.InGameMenuAnim.SetTrigger("In");
                isPausedWithAnim = true;
            }
            if (backgroundFade != null)
            {
                backgroundFade.SetBool("FadeOut", false);
                backgroundFade.SetTrigger("FadeIn");
            }
            return true;
        }
        return false;
    }

    public static bool Resume(bool withMenuAnim = false)
    {
        if (pauseScript != null && pauseScript.IsGamePaused())
        {
            pauseScript.ResumeAll();
            if (withMenuAnim)
            {
                pauseScript.InGameMenuAnim.SetBool("In", false);
                pauseScript.InGameMenuAnim.SetTrigger("Out");
                isPausedWithAnim = false;
            }
            if (backgroundFade != null)
            {
                backgroundFade.SetBool("FadeIn", false);
                backgroundFade.SetTrigger("FadeOut");
            }
            return true;
        }
        return false;
    }

    public static bool IsGamePausedByUser()
    {
        if (pauseScript != null)
        {
            return pauseScript.IsGamePaused() && isPausedWithAnim;
        }
        return false;
    }

    public static void Reset()
    {
        isPausedWithAnim = false;
        levelCoins = 0;
        temperature = 0.0f;
        damagedPipeScriptsQueue.Clear();
        Combo = 1;
    }

    public static void OpenLevel(int chapter, int level)
    {
        ShowLoadingMessage();
        GameState.Chapter = chapter;
        GameState.Level = level;
        GameState.OpenLevelIfNotOpened(chapter, level);
        Application.LoadLevel("Chapter" + chapter.ToString());
    }

    public static void RefreshLevel()
    {
        ShowLoadingMessage();
        Application.LoadLevel(Application.loadedLevel);                     // Refresh current level.
    }

    public static void LoadMenu(string name)
    {
        if (name.StartsWith("Levels"))
        {
            CurrentChapter = int.Parse(name.Replace("Levels", ""));
        }
        ShowLoadingMessage();
        IsPlaying = false;
        Application.LoadLevel(name);                                        // Load requested menu.

    }

    private static void ShowLoadingMessage()
    {
        if (staticProgressBarPrefab != null)
        {
            GameObject.Instantiate(staticProgressBarPrefab, progressBarPosition, Quaternion.identity);
        }
        GameState.Reset();
    }

    public static bool IsPlaying { get; set; }
}