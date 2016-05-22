using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// script that allows player to give orders to his units
/// </summary>
public class controlAI : basePlayer
{
    Camera currentCAM;
    public Camera minimapCam;
  Vector3 FirstHit;
    public bool attackModifier = false;
	// Use this for initialization
	void Start () {
        FirstHit = Input.mousePosition;
        mySelection = new List<baseRtsAI>();
      
            myBuilding.stats.Register(this);
        currentCAM = Camera.main;
    }
   public void switchToMiniMapCam()
    {
       // Debug.Log("im inside");
        currentCAM = minimapCam;
    }
    public void switchToMainCam()
    {
        currentCAM = Camera.main;
      //  Debug.Log("im out");
    }
    public void addUnit(baseRtsAI unit)
    {
        unit.gameObject.GetComponentInChildren<Projector>().enabled = true;
        mySelection.Add(unit);
    }
    public void removeUnit(baseRtsAI unit)
    {
        if(unit != null)
            unit.gameObject.GetComponentInChildren<Projector>().enabled = false;
       // mySelection.Remove(unit);
    }
    public void ClearSelection()
    {
        foreach (baseRtsAI unit in mySelection)
        {
            removeUnit(unit);
        }
        mySelection.Clear();
    }
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

            Ray ray = currentCAM.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                PointOfInterest poi = hit.collider.gameObject.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    UnitOrders.giveOrders(mySelection, UnitOrders.OrderType.move, hit.point);
                    mySelection[0].Orders.Clear();
                    mySelection[0].Orders.Enqueue(UnitOrders.CapturePoint(mySelection[0], poi));

                    return;
                }
                IRtsUnit target = hit.collider.gameObject.GetComponent<IRtsUnit>();
           //     Debug.Log(hit.collider.gameObject.name);
                if(target != null)
                {
                  ///   Debug.Log("target test");

                    UnitOrders.giveOrders(mySelection, UnitOrders.OrderType.attackTarget, target);
                    return;
                }
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
	}
}


    
