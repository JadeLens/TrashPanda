using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {

    public void Load(string Level)
   {

        SafeLoad(Level);
    }
    public static void SafeLoad(string level)
    {
        AudioManager.OnChange();
        SceneManager.LoadScene(level);
        

    }
    public void Exit()
    {
        Application.Quit();
       
    }
	
}
