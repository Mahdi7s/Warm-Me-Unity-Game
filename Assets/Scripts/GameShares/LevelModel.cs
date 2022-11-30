using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum LevelCup
{
    Bronze,
    Silver,
    Golden,
    None
}

public class LevelModel
{
    private const int MinSilverPercent = 95;
    private const int MinGoldenPercent = 98;

    private bool _wordCompleted = false;

    public int Level { set; get; }
    public int Chapter { set; get; }
    public int LevelCoins { set; get; }
    public bool Win { get; set; }
    public bool WordCompleted 
    { 
        get { return _wordCompleted; }
        set
        {
            if (!_wordCompleted && value)
                _wordCompleted = value;
        }
    }
    public int Skill { get; set; }

    public LevelCup LevelCup
    {
        get
        {
            return LevelCoins <= 0 ? LevelCup.None : Skill >= MinGoldenPercent ? 
                LevelCup.Golden : Skill >= MinSilverPercent ? LevelCup.Silver : LevelCup.Bronze;
        }
    }

    public LevelModel(string str = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            Init();
        } else
        {
            string[] array = str.Split(',');
            Chapter = int.Parse(array [0]);
            Level = int.Parse(array [1]);
            LevelCoins = int.Parse(array [2]);
            Win = bool.Parse(array [3]);
            _wordCompleted = bool.Parse(array [4]);
            Skill = int.Parse(array [5]);
        }
    }

    private void Init()
    {
        Chapter = 1;
        Level = 1;
        LevelCoins = 0;
        _wordCompleted = Win = false;
        Skill = 0;
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5}", Chapter, Level, LevelCoins, Win.ToString(), WordCompleted.ToString(), Skill);
    }
}