using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

    public void Load(string Level)
    {
        Application.LoadLevel(Level);
    }
    public void Exit()
    {
        Application.Quit();
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
	
}
