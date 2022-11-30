using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// 
/// All Must be used in OnGUI method
/// </summary>
public static class GUIUtility
{
    public static Transform[] digit;

    // Use this for initialization
    public static void ShowText(GUIText gtext, string text, Vector2 margin)
    {
        GUI.skin.font = gtext.font;
        GUI.skin.label.fontSize = gtext.fontSize;
        GUI.contentColor = Color.black;

        var pos2 = Camera.main.WorldToScreenPoint(new Vector3(gtext.transform.localPosition.x, gtext.transform.localPosition.y, 0));
        var pos = new Rect(pos2.x + margin.x, pos2.y + margin.y, 1000, 5000);
        GUI.Label(pos, text);
    }

    public static void DrawGUITextureAsText(Transform parent, int number, string sortingLayerName, int sortingOrder, float space)
    {
        DrawGUITextureAsText(digit, parent, number, sortingLayerName, sortingOrder, space);
    }

    public static void DrawGUITextureAsText(Transform[] digits, Transform parent, int number, string sortingLayerName, int sortingOrder, float space)
    {
        Vector3 position = parent.position;                     // position of text
        Vector3 scale = parent.localScale;                      // text scale
        int cntr;
        Transform temp;
        char[] print = number.ToString().ToCharArray();         // print: Character array that should be drawn on display

        for (cntr = 0; cntr < parent.childCount; cntr++)            // remove all the things that printed (instantiated) before.
        {
            GameObject.Destroy(parent.GetChild(cntr).gameObject);
        }

        for (cntr = 0; cntr < print.Length; cntr++)                 // for each character in characters that should be displayed
        {
            int index = print[cntr] - '0';                          // calculate index of the character sprite
            //Debug.Log("null: " + (digits == null).ToString() + ", count: " + digits.Length.ToString());
            temp = (Transform)Object.Instantiate(digits[index], position, Quaternion.identity); // instatiate the character sprite (game object)
            temp.renderer.sortingLayerName = sortingLayerName;      // set its sorting parameters
            temp.renderer.sortingOrder = sortingOrder;
            temp.parent = parent;                                   // make instantiated characters child of the specified parent
            temp.localScale = scale;                                // scale the text according to its parent scale
            position.x += temp.renderer.bounds.size.x + space;      // simulate moving cursor in display
        }
    }

    // this method works same as above method, for more info, see above method documentation.
    public static void DrawGUITextureAsText(Transform[] letters, Transform parent, string str, string sortingLayerName, int sortingOrder, float space)
    {
        Vector3 position = parent.position;
        Vector3 scale = parent.localScale;
        int cntr;
        char[] print = str.ToCharArray();
        space = space * parent.transform.localScale.x;
        for (cntr = 0; cntr < parent.childCount; cntr++)
        {
            GameObject.Destroy(parent.GetChild(cntr).gameObject);
        }
        for (cntr = 0; cntr < print.Length; cntr++)
        {
            if (print[cntr] == ' ')
            {
                position.x += 50 * space * parent.transform.localScale.x;                                       // simulate space character, by moving cursor forward
            }
            else if (('A' <= print[cntr] && print[cntr] <= 'Z') || ('a' <= print[cntr] && print[cntr] <= 'z'))  // if the character was a regular english character
            {
                Transform temp = DrawGUITextureAsCharacter(letters, parent, print[cntr], sortingLayerName, sortingOrder, position);
                position.x += temp.renderer.bounds.size.x + space;
            }
        }
    }

    public static Transform DrawGUITextureAsCharacter(Transform[] letters, Transform parent, char character, string sortingLayerName, int sortingOrder, Vector3 position)
    {
        int index = 0;
        Transform temp;
        Vector3 scale = parent.localScale;
        if ('A' <= character && character <= 'Z')
        {
            index = character - 'A';
        }
        else if ('a' <= character && character <= 'z')
        {
            index = character - 'a';
        }
        temp = (Transform)Object.Instantiate(letters[index], position, Quaternion.identity);
        temp.renderer.sortingLayerName = sortingLayerName;
        temp.renderer.sortingOrder = sortingOrder;
        SpriteRenderer spriteRenderer = temp.GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        spriteRenderer.sprite = Sprite.Create(sprite.texture, sprite.textureRect, Vector2.zero);    // make the sprite pivot up-left.
        temp.parent = parent;
        temp.localScale = scale;
        return temp;
    }

    public static Vector2 GetViewSize()
    {
        var cam = Camera.main;
        var height = cam.orthographicSize * 2.0f;
        var width = height * cam.aspect;

        return new Vector2(width, height);
    }

    public static Vector2 GetSpriteSize(Sprite spr)
    {
        return new Vector2(spr.bounds.size.x, spr.bounds.size.y);
    }

    public static Vector3 ToWorld(Vector3 v3)
    {
        var viewSize = GetViewSize();
        return new Vector3(v3.x - viewSize.x / 2, v3.y - viewSize.y / 2, v3.z);
    }
}