using UnityEngine;
using System.Collections;

//Put this on the Player Object
public class FogOfWarSight : MonoBehaviour
{
    public float radius = 5.0f;
    public LayerMask layerMask = -1;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    foreach(Collider col in Physics.OverlapSphere(transform.position, radius, layerMask))
        {
            FogOfWarVisibility fogscript = col.gameObject.GetComponent<FogOfWarVisibility>();
            if (fogscript != null)
            {
                fogscript.Observed();
                //col.gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
	}
}
