using UnityEngine;
using System.Collections;

public class QuoteController : MonoBehaviour
{
    public Transform[] fillLetters = new Transform[52];
    Transform filledTextPlaceHolder;
    GUILetterScript gUILetterScript;
    string[,] qoats;
    string sortingLayerName;
    int sortingOrder;
    char[] qoat;

    private string _quoteStr = string.Empty;

    void Start()
    {
        gUILetterScript = GetComponentInChildren<GUILetterScript>();
        filledTextPlaceHolder = transform.GetChild(1);
        filledTextPlaceHolder.position = gUILetterScript.GetPlaceHolderPosition();  // sets place holder positions the same
        sortingLayerName = gUILetterScript.sortingLayer;
        sortingOrder = gUILetterScript.sortingOrder;
        if (fillLetters [26] == null)                           // if fill letters are just lower case
        {
            for (int cntr = 0; cntr < 26; cntr++)               // use lower case as upper case
            {
                fillLetters [cntr + 26] = fillLetters [cntr];
            }
        }
        qoats = new string[6, 9];
        #region Quote Definitions
        qoats [0, 0] = "sometime";
        qoats [0, 1] = "smallest";
        qoats [0, 2] = "thing";
        qoats [0, 3] = "take";
        qoats [0, 4] = "most";
        qoats [0, 5] = "room";
        qoats [0, 6] = "heart";
        qoats [0, 7] = "winnie";
        qoats [0, 8] = "pooh";
        qoats [1, 0] = "best";
        qoats [1, 1] = "most";
        qoats [1, 2] = "beautiful";
        qoats [1, 3] = "world";
        qoats [1, 4] = "see";
        qoats [1, 5] = "touch";
        qoats [1, 6] = "must";
        qoats [1, 7] = "felt";
        qoats [1, 8] = "heart";
        qoats [2, 0] = "even";
        qoats [2, 1] = "knew";
        qoats [2, 2] = "tomorrow";
        qoats [2, 3] = "world";
        qoats [2, 4] = "pieces";
        qoats [2, 5] = "still";
        qoats [2, 6] = "plant";
        qoats [2, 7] = "apple";
        qoats [2, 8] = "tree";
        qoats [3, 0] = "greatest";
        qoats [3, 1] = "weakness";
        qoats [3, 2] = "giving";
        qoats [3, 3] = "certain";
        qoats [3, 4] = "succeed";
        qoats [3, 5] = "always";
        qoats [3, 6] = "try";
        qoats [3, 7] = "more";
        qoats [3, 8] = "time";
        qoats [4, 0] = "journy";
        qoats [4, 1] = "thousend";
        qoats [4, 2] = "mile";
        qoats [4, 3] = "must";
        qoats [4, 4] = "begin";
        qoats [4, 5] = "with";
        qoats [4, 6] = "single";
        qoats [4, 7] = "step";
        qoats [4, 8] = "laotzu";
        qoats [5, 0] = "ones";
        qoats [5, 1] = "who";
        qoats [5, 2] = "crazy";
        qoats [5, 3] = "enough";
        qoats [5, 4] = "think";
        qoats [5, 5] = "can";
        qoats [5, 6] = "change";
        qoats [5, 7] = "world";
        qoats [5, 8] = "ones";
        #endregion
        if (GameState.Level <= 0)
        {
            GameState.Level = 1;
        }
        _quoteStr = gUILetterScript.str = qoats[GameState.Chapter - 1, GameState.Level - 1];
        qoat = gUILetterScript.str.ToCharArray();
    }

    public string GetQuote()
    {
        return _quoteStr;
    }

    public void FillCharacter(int index)
    {
        //Debug.Log(index);
        if (0 <= index && index <= _quoteStr.Length - 1)                // if index is valid
        {
            GUIUtility.DrawGUITextureAsCharacter(fillLetters, filledTextPlaceHolder, qoat[index], sortingLayerName, sortingOrder, gUILetterScript.GetLetterPosition(index));
        }
    }
}