using UnityEngine;
using System.Collections;

//Put this on the Enemy
public class FogOfWarVisibility : MonoBehaviour
{
    private bool observed = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (observed)
        {
            //this.gameObject.GetComponent<Renderer>().enabled = true;
            this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            this.gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
        else
        {
            //this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }

        observed = false;
	}

    void Observed()
    {
        observed = true;
    }
}
