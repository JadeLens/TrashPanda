using UnityEngine;
using System.Collections;

public class controlAI : MonoBehaviour {
    public aiBehavior[] Selection;
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
                foreach (baseRtsAI rabbit in Selection)
                {
                    IMoveToNode commande = moveComand(rabbit);
                    commande.SetDestination(hit.point);
                    commande.SetArriveRadius(2.5f);
                    rabbit.Orders.Clear();
                    rabbit.Orders.Enqueue((aiBehaviorNode)commande);
                }
            }
        }
    }

    IMoveToNode moveComand(baseRtsAI rabbit)
    {
        IMoveToNode commande = new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del, rabbit.m_unit);

        return commande;
    }
    /*
    aiBehaviorNode attackMove(baseRtsAI rabbit)
    {
        return new Node_Sequence
        (
            new aiBehaviorNode[] 
            {
                           
                new Node_Seek_Modular(moveComand(rabbit),10,2.5f,AItype.lamb),
                //new Node_Align(agent),
                //new Node_AlignToTarget(agent,detectionRange,SeekarriveRadius,AItype.lamb),
                new Node_Attack_Activate_Weapon(MainWeapon,stats),
                new Node_Delay(1f)
            }
        );
    }*/
}
