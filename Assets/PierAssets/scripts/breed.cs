using UnityEngine;
using System.Collections;

public class breed : MonoBehaviour {
    public baseRtsAI unit;
    public bool isMale = true;
    public bool isMature = true;
    // Use this for initialization
    void Start () {
        unit = this.gameObject.GetComponent<baseRtsAI>();
    }
	
	// Update is called once per fra
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {

            UnitBuilder.cloneUnit(unit);
        }
	}
}
