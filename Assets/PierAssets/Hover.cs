using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hover : MonoBehaviour {


    [SerializeField]
    GameObject M_Button;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HoverTrue(GameObject Obj)
    {
        Obj.SetActive(true);
    }
    public void HoverFalse(GameObject Obj)
    {
        Obj.SetActive(false);

    }

    public void CoonButton(GameObject Obj)
    {

        if (M_Button.GetComponentInChildren<Text>().text == "Raccoon")
        {
            Obj.GetComponentInChildren<Text>().text = "Cost: Racoon 75 Trash 30 Water";
        }
        Obj.SetActive(true);
    }
}
