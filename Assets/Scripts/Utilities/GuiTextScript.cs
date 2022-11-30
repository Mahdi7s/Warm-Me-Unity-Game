using UnityEngine;
using System.Collections;

public class GuiTextScript : MonoBehaviour {
    public int Size = 4;
    public Font Font = null;
    public Color Color = Color.black;
    public string Text = string.Empty;

    void OnGUI()
    {
        GUI.skin.font = Font;
        GUI.skin.label.fontSize = Size;
        GUI.contentColor = Color;

        var p = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(p.x, Screen.height - p.y, 1000, 5000), Text);
    }
}
