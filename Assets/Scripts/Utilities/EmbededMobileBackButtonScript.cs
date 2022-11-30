using UnityEngine;
using System.Collections;

public class EmbededMobileBackButtonScript : MonoBehaviour
{
    public static string lastScene = null;
    public StoreManager storeManager;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))               // if escape or android back button has been pressed
        {
            if (lastScene != null)
            {
                if (GameState.IsGamePausedByUser())
                {
                    GameState.Resume(true);
                }
                else if (GameState.IsPlaying)
                {
                    GameState.Pause(true);
                }
                else
                {
                    GameState.LoadMenu(lastScene);
                    if (storeManager != null)
                    {
                        storeManager.ResetStorePurchases();
                    }
                }
            }
        }
	}
}
