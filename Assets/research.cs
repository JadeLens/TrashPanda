using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class research : MonoBehaviour {
    GameObject timebar;
    public GameObject rabbitDen;
    private GameObject m_caller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startResearch(GameObject caller)
    {
        timebar = caller.transform.GetChild(1).gameObject;
        timebar.GetComponentInChildren<timer>().StartTimer(caller);
        m_caller = caller;

    }

    public void finished()
    {
        m_caller.GetComponentInChildren<Text>().text = "Raccoon";
        timebar.SetActive(false);

        //GetComponentInChildren<Text>().text = "Raccoon";
        m_caller.GetComponent<Button>().onClick.RemoveAllListeners();
        m_caller.GetComponent<Button>().onClick.AddListener(rabbitDen.GetComponent<UnitBuilder>().spawnForUiButton);
        //this.GetComponent<research>().enabled = false;
        //this.GetComponent<Button>()

    }

    
}
