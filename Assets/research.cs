using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class research : MonoBehaviour {
    GameObject timebar;
    public GameObject rabbitDen;
    private GameObject m_caller;

    public int m_trashCost;
    public int m_waterCost;
    public AudioClip wrong;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startResearch(GameObject caller)
    {
        if (rabbitDen.GetComponent<UnitBuilder>().owner.myResources.getTrash() >= m_trashCost && rabbitDen.GetComponent<UnitBuilder>().owner.myResources.getWater() >= m_waterCost)
        {

            rabbitDen.GetComponent<UnitBuilder>().owner.myResources.IncrementTrash(-m_trashCost);
            rabbitDen.GetComponent<UnitBuilder>().owner.myResources.IncrementWater(-m_waterCost);
            timebar = caller.transform.GetChild(1).gameObject;
            timebar.GetComponentInChildren<timer>().StartTimer(caller);
            m_caller = caller;
        }
        else
        {
            AudioManager.PlaySoundClip(wrong);
        }
    }

    public void finished()
    {
        m_caller.GetComponentInChildren<Text>().text = "Raccoon";
        timebar.SetActive(false);

        //GetComponentInChildren<Text>().text = "Raccoon";
        m_caller.GetComponent<Button>().onClick.RemoveAllListeners();
        m_caller.GetComponent<Button>().onClick.AddListener(rabbitDen.GetComponent<UnitBuilder>().spawnRacoonForUiButton);
        //this.GetComponent<research>().enabled = false;
        //this.GetComponent<Button>()

    }

    
}
