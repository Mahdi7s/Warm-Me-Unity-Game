using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour
{
    
    public bool particleBoard = false;
    public bool particleBallon = false;
    public bool particleCoin = false;
    public bool particleHelpBoard = false;
    public bool particlePig = false;
    public bool particleFireworks = false;
    public bool particleCombo = false;
    
    // Use this for initialization
    void Start()
    {

        //renderer.sortingLayerName = "GuiParticle";
        if (particleBoard)
        {
            renderer.sortingLayerName = "GuiMenu";
            renderer.sortingOrder = 4;
        } else if (particleBallon)
        {
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = -4;
        } else if (particleCoin)
        {
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = -2;
        } else if (particleHelpBoard)
        {
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = 9;
        } else if (particlePig)
        {
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = 18;
        } else if (particleFireworks)
        {
            renderer.sortingLayerName = "GuiMenu";
            renderer.sortingOrder = 6;
        } else if (particleCombo)
        {
            renderer.sortingLayerName = "GuiMenu";
            renderer.sortingOrder = -1;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
