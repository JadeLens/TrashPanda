using UnityEngine;
using System.Collections;

public class GameOverButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MainMenu()
    {
        Application.LoadLevel("MainMenu");

    }

    public void Quit()
    {
        Application.Quit();
    }
}
