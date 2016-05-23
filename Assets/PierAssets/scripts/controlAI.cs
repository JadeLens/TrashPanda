using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// script that allows player to give orders to his units
/// </summary>
public class controlAI : basePlayer
{

    Camera currentCAM;
    Camera minimapCam;
    Vector3 startClick;
    Transform cameraTarget;
    public bool attackModifier = false;
    public Texture2D selectionBox = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    // Use this for initialization
    void Start() {
        startClick = -Vector3.one; // selection box not drawn when set to this value
        mySelection = new List<baseRtsAI>();

        myBuilding.stats.Register(this);
        minimapCam = GameObject.FindGameObjectWithTag("minimapCam").GetComponent<Camera>();
        currentCAM = Camera.main;
        cameraTarget = GameObject.FindGameObjectWithTag("cameraTarget").transform;
    }
  
 
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.A))
        {
            attackModifier = true;

        }
        if (Input.GetButtonUp("Fire2"))
        {
            orderUnits();

        }
        if (Input.GetButtonDown("Fire1"))
        {

            startDragSelect();

        }
        if (Input.GetButtonUp("Fire1"))
        {

            if (cursorOverMinimap())
            {
                moveMinimap();
            }
            else
            {
                endDragSelect();
            }

        }
        if (Input.GetButton("Fire1"))
        {

            scaleSelectionBox();
        }
    }

    public void switchToMiniMapCam()
    {
        // 
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
        if (unit != null)
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

    void orderUnits()
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
            if (target != null)
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
    bool cursorOverMinimap()
    {

        return currentCAM == minimapCam;
    }

    private void moveMinimap()
    {
        RaycastHit hit;

        Ray ray = currentCAM.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            cameraTarget.position = new Vector3(hit.point.x, cameraTarget.position.y, hit.point.z);
        }

    }
   

 

//selection stuff
    public bool IsWithingBounds(GameObject ob)
    {

        //   Bounds selectionBox = getViewPortBounds(Camera.main, startClick, Input.mousePosition);

        Vector3 camPos = Camera.main.WorldToScreenPoint(ob.transform.position);
        camPos.y = invertMouseY(camPos.y);
        return selection.Contains(camPos);

        //  selectionBox.Contains(ob.transform.position);
    }
    void startDragSelect()
    {
        startClick = Input.mousePosition;

    }
    void endDragSelect()
    {
        attackModifier = false;

        if (selection.width < 0)
        {
            selection.x += selection.width;
            selection.width = -selection.width;
        }
        if (selection.height < 0)
        {
            selection.y += selection.height;
            selection.height = -selection.height;
        }

        ClearSelection();
        ////left click selection
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            IRtsUnit unit = hit.collider.gameObject.GetComponent<IRtsUnit>();

            if (unit != null)
            {

                addUnit(unit.getAIcomponent());
            }
        }
        ///drag selection
        foreach (IRtsUnit unit in RTSUnitManager.GetUnitList())
        {
            // Debug.Log("test");
            if (IsWithingBounds(unit.GetGameObject()))
            {
                if (unit.getFaction() == UnitFaction)
                {
                    if (unit.getAIcomponent() != null)
                        addUnit(unit.getAIcomponent());
                }
            }
        }

        startClick = -Vector3.one;

    }

    void scaleSelectionBox()
    {

        selection = new Rect(startClick.x, invertMouseY(startClick.y), Input.mousePosition.x - startClick.x, invertMouseY(Input.mousePosition.y) - invertMouseY(startClick.y));


    }

    //show selection box
    private void OnGUI()
    {
        if (startClick != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionBox);
        }
    }

    public static float invertMouseY(float y)
    {
        return Screen.height - y;
    }

    //static void drawBox(Vector3 topLeft, Vector3 botomRight, Vector3 topRight, Vector3 bottomLeft)
    //{
    //    Debug.DrawLine(topLeft, topRight, Color.magenta);
    //    Debug.DrawLine(topLeft, bottomLeft, Color.magenta);
    //    Debug.DrawLine(botomRight, topRight, Color.magenta);
    //    Debug.DrawLine(botomRight, bottomLeft, Color.magenta);


    //    Debug.DrawLine(topLeft, botomRight, Color.red);
    //}
}



