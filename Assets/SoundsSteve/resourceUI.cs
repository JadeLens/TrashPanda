using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class resourceUI : MonoBehaviour {

    public Text trashTxt;
    public Text waterTxt;
    public basePlayer player;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (player!= null)
        {
            trashTxt.text = "Trash: " + player.myResources.getTrash().ToString();
            waterTxt.text = "Water: " + player.myResources.getWater().ToString();
        }
	}
}
