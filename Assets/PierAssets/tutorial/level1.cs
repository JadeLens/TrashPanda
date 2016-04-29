using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class level1 : MonoBehaviour {

    public string levelToChangeTo;
    PointOfInterest[] pointsToCapture;
    // Use this for initialization
    int currentMessage = 0;
    public string[] messages;
    public Text messageBox;
    void Start()
    {
        pointsToCapture = FindObjectsOfType<PointOfInterest>();
        messageBox.text = messages[currentMessage];
    }
    public void checkRequirement()
    {
        int capturedCount = 0;
        foreach(PointOfInterest poi in pointsToCapture)
        {
            poi.owningFaction = faction.faction1;
            capturedCount++;
        }
        if(capturedCount >= pointsToCapture.Length)
        {
            Debug.Log("level completed");

        }
    }

	
	// Update is called once per frame
	void Update () {
        checkRequirement();
    }
}
