using UnityEngine;
using System.Collections;

//Put this on the Enemy
public class FogOfWarVisibility : MonoBehaviour
{
    private bool observed = false;
    private MeshRenderer minimapRenderer;
    private SkinnedMeshRenderer myRenderer;
    private Canvas myCanvas;
    // Use this for initialization
    void Start ()
    {
        minimapRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        myRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        myCanvas = this.gameObject.GetComponentInChildren<Canvas>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (observed)
        {
            //this.gameObject.GetComponent<Renderer>().enabled = true;
            minimapRenderer.enabled = true;
            myRenderer.enabled = true;
            myCanvas.enabled = true;
        }
        else
        {
            //this.gameObject.GetComponent<Renderer>().enabled = false;
            minimapRenderer.enabled = false;
            myRenderer.enabled = false;
            myCanvas.enabled = false;
        }

        observed = false;
	}

    public void Observed()
    {
        observed = true;
   //    Debug.Log("yo");
    }
}
