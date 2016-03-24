using UnityEngine;
using System.Collections;

//Put this on the Enemy
public class FogOfWarVisibility : MonoBehaviour
{
    private bool observed = false;
    private SkinnedMeshRenderer myRenderer;
    private Canvas myCanvas;
    // Use this for initialization
    void Start ()
    {
        myRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        myCanvas = this.gameObject.GetComponentInChildren<Canvas>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (observed)
        {
            //this.gameObject.GetComponent<Renderer>().enabled = true;
            myRenderer.enabled = true;
            myCanvas.enabled = true;
        }
        else
        {
            //this.gameObject.GetComponent<Renderer>().enabled = false;
            myRenderer.enabled = false;
            myCanvas.enabled = false;
        }

        observed = false;
	}

    void Observed()
    {
        observed = true;
   //    Debug.Log("yo");
    }
}
