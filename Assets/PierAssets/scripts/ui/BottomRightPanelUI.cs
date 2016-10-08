using UnityEngine;
using System.Collections;

public class BottomRightPanelUI : MonoBehaviour {
    public Transform BuildingUi;
    public Transform UnitUi;
    private basePlayer player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UnitUi.gameObject.SetActive((player.mySelection.Count >= 1));
 BuildingUi.gameObject.SetActive(!(player.mySelection.Count >= 1));

    }
}
