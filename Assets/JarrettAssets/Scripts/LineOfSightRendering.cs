using UnityEngine;
using System.Collections;

//Currently Unused. Use Fog Of War Scripts instead.
public class LineOfSightRendering : MonoBehaviour
{
    public string viewerTag = "Player";
    public LayerMask layerMask = -1;

    private GameObject player;

	// Update is called once per frame
	void Update ()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(viewerTag);
        }

        RaycastHit hit;

        //                  player position     raycast to player
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject == player)
            {
                this.gameObject.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
	}
}
