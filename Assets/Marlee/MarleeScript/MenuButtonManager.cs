using UnityEngine;
using System.Collections;

public class MenuButtonManager : MonoBehaviour {
    public GameObject menuCanvas;
    public GameObject helpCanvas;

	// Use this for initialization
	void Start () {

        menuCanvas.SetActive(true);
        helpCanvas.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        Application.LoadLevel(1);
    }

    public void Help()
    {
        menuCanvas.SetActive(false);
        helpCanvas.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Back()
    {
        menuCanvas.SetActive(true);
        helpCanvas.SetActive(false);
    }

    public void Again()
    {
       
    }
}

