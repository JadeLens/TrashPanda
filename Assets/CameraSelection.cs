using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraSelection : MonoBehaviour
{

    /// <summary>
    /// USE A BOUND TO CHECK UNIT POS IN VIEWPORT COORDINATES
    /// </summary>
	public Texture2D selectionBox = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    private Vector3 startClick = -Vector3.one;
    controlAI ctrl;
    
    // Update is called once per frame
    void Start()
    {

        ctrl = GameObject.FindObjectOfType<controlAI>();
    }
    void Update()
    {

        CheckCamera();

        // allUnits = new List<baseRtsAI>();
    }
    public static Bounds getViewPortBounds(Camera cam, Vector3 pos1, Vector3 pos2)
    {
        Vector3 v1 = Camera.main.ScreenToViewportPoint(pos1);

        Vector3 v2 = Camera.main.ScreenToViewportPoint(pos2);

        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        Bounds bounds = new Bounds();

        bounds.SetMinMax(min, max);
        return bounds;
    }
    public bool IsWithingBounds(GameObject ob)
    {

        //   Bounds selectionBox = getViewPortBounds(Camera.main, startClick, Input.mousePosition);

        Vector3 camPos = Camera.main.WorldToScreenPoint(ob.transform.position);
        camPos.y = CameraSelection.invertMouseY(camPos.y);
        return CameraSelection.selection.Contains(camPos);

        //  selectionBox.Contains(ob.transform.position);
    }
    private void CheckCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
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
            ctrl.ClearSelection();
            foreach (IRtsUnit unit in RTSUnitManager.GetUnitList())
            {
                // Debug.Log("test");
                if (IsWithingBounds(unit.GetGameObject()))
                {
                    if (unit.getFaction() == ctrl.UnitFaction)
                    {
                        if(unit.getAIcomponent() != null)
                            ctrl.addUnit(unit.getAIcomponent());
                    }
                }
            }
            startClick = -Vector3.one;


            /*if(Physics.BoxCastAll()){
				

			}*/
        }

        if (Input.GetMouseButton(0))
        {

            selection = new Rect(startClick.x, invertMouseY(startClick.y), Input.mousePosition.x - startClick.x, invertMouseY(Input.mousePosition.y) - invertMouseY(startClick.y));


        }

    }

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

}
