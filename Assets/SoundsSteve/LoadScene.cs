using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

    public void Load(string Level)
   {

        SafeLoad(Level);
    }
    public static void SafeLoad(string level)
    {
        AudioManager.OnChange();
        Application.LoadLevel(level);

    }
    public void Exit()
    {
        Application.Quit();
       
    }
	
}
