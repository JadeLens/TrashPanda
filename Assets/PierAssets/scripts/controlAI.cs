using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class controlAI : MonoBehaviour 
{

    public aiBehavior[] Selection;
    public List<baseRtsAI> mySelection;
    public bool attackModifier = false;
	// Use this for initialization
	void Start () {
        FirstHit = Input.mousePosition;
        mySelection = new List<baseRtsAI>();
    }
    Vector3 FirstHit;
    static void drawBox(Vector3 topLeft, Vector3 botomRight,Vector3 topRight, Vector3 bottomLeft)
    {
        Debug.DrawLine(topLeft, topRight, Color.magenta);
        Debug.DrawLine(topLeft,bottomLeft, Color.magenta);
        Debug.DrawLine(botomRight, topRight, Color.magenta);
        Debug.DrawLine(botomRight, bottomLeft, Color.magenta);

   
        Debug.DrawLine(topLeft,botomRight, Color.red);
    }
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.A))
        {
            attackModifier = true;

        }
		if (Input.GetButtonUp("Fire2"))
		{
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                UnitOrders.OrderType type;
                if (attackModifier)
                {
                    type = UnitOrders.OrderType.attackMove;
                }
                else
                {
                    type = UnitOrders.OrderType.move;
                }
                UnitOrders.giveOrders(mySelection, type, hit.point);
                attackModifier = false;
            }
          
		}
        if (Input.GetButtonUp("Fire1"))
        {
            attackModifier = false;

        }

        if (Input.GetKeyDown(KeyCode.A))
		{
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				PointOfInterest poi = hit.collider.gameObject.GetComponent<PointOfInterest>();
				if(poi)
				{

					aiBehaviorNode commande = UnitOrders.CapturePoint((baseRtsAI)Selection[0],poi);//new Node_Call_Delegate(poi.toggleMat);
					Selection[0].Orders.Clear();
					Selection[0].Orders.Enqueue(commande);
					//UnitOrders.giveOrders(Selection,UnitOrders.OrderType.capture,hit.point);
				}
				

			}
		}
	}
}


    
