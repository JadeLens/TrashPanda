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
		if (Input.GetButton("Fire2"))
		{
			RaycastHit hit;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				// Debug.Log(hit.point);
				foreach (baseRtsAI rabbit in Selection)
				{
					aiBehaviorNode commande = attackMove(rabbit,hit.point);
					
					rabbit.Orders.Clear();
					rabbit.Orders.Enqueue(commande);
				}
			}
		}
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
               // Debug.Log(hit.point);
                foreach (baseRtsAI rabbit in Selection)
                {
					aiBehaviorNode commande = moveComand(rabbit,hit.point);
				
                    rabbit.Orders.Clear();
                    rabbit.Orders.Enqueue(commande);
                }
            }
        }
    }

	aiBehaviorNode moveComand(baseRtsAI rabbit,Vector3 loc)
    {
		IMoveToNode commande = new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del, rabbit.m_unit,loc);
	
		commande.SetArriveRadius(2.5f);
		return (aiBehaviorNode)commande;
    }
    
	aiBehaviorNode attackMove(baseRtsAI rabbit,Vector3 loc)
	{	
		return new Node_Selector
		(
			new  aiBehaviorNode[] 
			{
     			new Node_Sequence
                (
                    new  aiBehaviorNode[] 
                    {
                       
                        new Node_Seek_Modular
                        (
							(IMoveToNode)(new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del,rabbit.m_unit)),
							rabbit.detectionRange,rabbit.SeekarriveRadius,rabbit.typeToChase
                        ),
						new Node_Attack_Activate_Weapon(rabbit.MainWeapon,rabbit.stats),
                        new Node_Delay(1f)
                    }
				),
				new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del, rabbit.m_unit,loc)

			}

		);
	}
}
