using UnityEngine;
using System.Collections;

public class ItemHelpScript : MonoBehaviour
{
    public SpriteRenderer Image = null;
    public SpriteRenderer Negative = null;
    public SpriteRenderer Positive = null;
    public SpriteRenderer Name = null;

    public GUINumberScript Heat = null;
    public GUINumberScript Score = null;
    public GUINumberScript Damage = null;
    public GUINumberScript Chapter = null;

    public GameObject Percent1 = null;
    public GameObject Percent2 = null;

    public GameObject EggScore = null;


    private Animator _anim = null;


    void Start()
    {
        _anim = GetComponent<Animator>();
        Reset();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public IEnumerator ShowBoard(GameObject[] objs)
    {
        var gobj = objs [0];
        var itemSpr = objs [1];

        Reset();
        EnableValues(false);
        yield return new WaitForSeconds(1.7f);

        var hatItemScr = gobj.GetComponent<HatItemScript>();
        if (hatItemScr != null)
        {
            Image.sprite = itemSpr.GetComponent<SpriteRenderer>().sprite;
            Name.sprite = hatItemScr.name;
            if (hatItemScr.Kind == HatItemKind.EggBronze)
            {
                Score.Hide();
                EggScore.SetActive(true);
            }
            else
            {
                Score.Number = hatItemScr.Score;
                Score.Show();
            }
            Damage.Number = hatItemScr.ExplosionPower;
            if (hatItemScr.Temperature != 0)
            {
                var posChar = hatItemScr.Temperature >= 0;
                Negative.enabled = !posChar;
                Positive.enabled = posChar;
            }
            EnableValues(true);
            Heat.Number = Mathf.Abs(hatItemScr.Temperature);
            var perc1Ena = Mathf.Abs(hatItemScr.ExplosionPower) < 10;
            Percent1.SetActive(perc1Ena);
            Percent2.SetActive(!perc1Ena);
            Chapter.Number = hatItemScr.ShowFromChapter;
        }
    }

    private void Reset()
    {
        Negative.enabled = false;
        Positive.enabled = false;
        Image.sprite = null;
        Heat.Number = 0;
        Score.Number = 0;
        Damage.Number = 0;
        Chapter.Number = 0;

        Percent1.SetActive(false);
        Percent2.SetActive(false);
        if (EggScore != null)
        {
            EggScore.SetActive(false);
        }
    }

    private void EnableValues(bool enable)
    {
        Heat.gameObject.SetActive(enable);
        Score.gameObject.SetActive(enable);
        Damage.gameObject.SetActive(enable);
        Chapter.gameObject.SetActive(enable);

        Negative.gameObject.SetActive(enable);
        Positive.gameObject.SetActive(enable);
        Percent1.gameObject.SetActive(enable);
        Percent2.gameObject.SetActive(enable);
    }
}
