using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuManager : MonoBehaviour
{

    public bool pauseGame = false;
    public GameObject player;
    public GameObject pausemenuCanvas;
    public GameObject optionsCanvas;



    void Start()
    {
        //canvas = GetComponent<Canvas>();
        //canvas.enabled = false;
        pausemenuCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame = !pauseGame;
            if (pauseGame == true)
            {

                //canvas.enabled = !canvas.enabled;

                pausemenuCanvas.SetActive(true);

                Time.timeScale = 0;
                pauseGame = true;
                // kitten.GetComponent<CustomMouseLook>().enabled = false;

            }

        }
        if (pauseGame == false)
        {
            //canvas.enabled = !canvas.enabled;
            pausemenuCanvas.SetActive(false);
            optionsCanvas.SetActive(false);
            Time.timeScale = 1;
            pauseGame = false;
         
            //player.GetComponent<CustomMouseLook>().enabled = true;


        }
    }


    public void Back()
    {
        pausemenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
    public void Options()
    {
        optionsCanvas.SetActive(true);
        pausemenuCanvas.SetActive(false);
    }

    public void MainMenu()
    {
        pauseGame = false;
        Application.LoadLevel("MainMenu");
       
    }

    public void Resume()
    {
        pausemenuCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        pauseGame = false;
        
    }

}
