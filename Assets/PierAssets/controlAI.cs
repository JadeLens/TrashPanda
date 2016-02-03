using UnityEngine;
using System.Collections;

public class controlAI : MonoBehaviour {
    public aiBehavior[] Selection;
    public baseRtsAI leader;
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        Vector3 pos = transform.position;
        /*
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //Debug.Log(hit.point);

                IMoveToNode commande = new Node_MoveTo_With_Astar(leader.gameObject, leader.m_seeker,leader.m_unit.OnPathComplete);
                commande.SetDestination(hit.point);
                commande.SetArriveRadius(0.5f * Selection.Length * 2);
                leader.Orders.Enqueue((aiBehaviorNode)commande);
        

            }

        }*/
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
               // Debug.Log(hit.point);
             
                IMoveToNode commande = new Node_MoveTo_With_Astar(leader.gameObject, leader.m_seeker, leader.m_unit.del);
                commande.SetDestination(hit.point);
                commande.SetArriveRadius(2.5f);
                leader.Orders.Clear();
                leader.Orders.Enqueue((aiBehaviorNode)commande);


            }
        }


    }
}
