using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// script that allows player to give orders to his units
/// </summary>
public class Player : basePlayer
{
	Camera currentCAM;
	Camera minimapCam;
	Vector3 startClick;
	Transform cameraTarget;
	public bool attackModifier = false;
	public Texture2D selectionBox = null;
	public static Rect selection = new Rect (0, 0, 0, 0);
	List<baseRtsAI>[] controlGroups;
	public bool MouseUi = false;
	private bool IsDragSelecting = false;
	public bool spellTargetModeOn = false;
	public Texture2D spellCursorTexture;
	public Texture2D attackCursorTexture;

	enum InputMode
	{
		normal,
		attack,
		spellTarget}

	;

	InputMode m_InputMode = InputMode.normal;
	// Use this for initialization
	void Start ()
	{
		startClick = -Vector3.one; // selection box not drawn when set to this value
		mySelection = new List<baseRtsAI> ();
		controlGroups = new List<baseRtsAI>[10];
		myBuilding.stats.Register (this);
		minimapCam = GameObject.FindGameObjectWithTag ("minimapCam").GetComponent<Camera> ();
		currentCAM = Camera.main;
		cameraTarget = GameObject.FindGameObjectWithTag ("cameraTarget").transform;
	}

	public void selectAllUnis ()
	{
		foreach (IRtsUnit unit in RTSUnitManager.GetUnitList()) {
			if (unit.getFaction () == UnitFaction) {
				if (unit.getAIcomponent () != null)
					addUnit (unit.getAIcomponent ());
			}   
		}
	}

	public void setMouseOberUI (bool val)
	{
		MouseUi = val;
	}

	void setControlGroup (int group)
	{
		if (mySelection != null) {
			if (controlGroups [group] == null) {
				controlGroups [group] = new List<baseRtsAI> ();
			}
			controlGroups [group].Clear ();
			foreach (baseRtsAI u in mySelection) {
				controlGroups [group].Add (u);
			}
		}
	}

	public void SelectControlGroup (int group)
	{
		if (controlGroups [group] != null) {
			ClearSelection ();
			foreach (baseRtsAI u in controlGroups[group]) {   
				addUnit (u);
			}   
		}
	}

	void handleGroup (bool isCtrlDown, int group)
	{
		if (isCtrlDown) {
			setControlGroup (group);
		} else {
			SelectControlGroup (group);
		}
	}

	void ControlGroupsInput ()
	{
		bool ctrl = false;
		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl) || Input.GetKey (KeyCode.X)) {
			ctrl = true;
		}
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			handleGroup (ctrl, 1);
		} else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			handleGroup (ctrl, 2);
		} else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			handleGroup (ctrl, 3);
		} else if (Input.GetKeyUp (KeyCode.Alpha4)) {
			handleGroup (ctrl, 4);
		} else if (Input.GetKeyUp (KeyCode.Alpha5)) {
			handleGroup (ctrl, 5);
		} else if (Input.GetKeyUp (KeyCode.Alpha6)) {
			handleGroup (ctrl, 6);
		} else if (Input.GetKeyUp (KeyCode.Alpha7)) {
			handleGroup (ctrl, 7);
		} else if (Input.GetKeyUp (KeyCode.Alpha8)) {
			handleGroup (ctrl, 8);
		} else if (Input.GetKeyUp (KeyCode.Alpha9)) {
			handleGroup (ctrl, 9);
		} else if (Input.GetKeyUp (KeyCode.Alpha0)) {
			handleGroup (ctrl, 0);
		}
	}

	public void ActivateAttackModifier ()
	{
		Debug.Log ("attc md activated");
		m_InputMode = InputMode.attack;
		attackModifier = true;
		Cursor.SetCursor (attackCursorTexture, Vector2.zero, CursorMode.Auto);

		Input.ResetInputAxes ();
	}

	void resetInputMode ()
	{
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		m_InputMode = InputMode.normal;
		attackModifier = false;
		Debug.Log ("md reset");
	}

	void AttackInputModeUpdate ()
	{
		if (Input.GetButtonUp ("Fire1")) {

			if (MouseUi == false || cursorMinimap () == true) {
				Debug.Log ("frm attc md");
				orderUnits ();
				resetInputMode ();
				return;
			} else {
				resetInputMode ();
				return;
			}
		} else if (Input.anyKeyDown && !Input.GetButton ("Fire1")) {
			resetInputMode ();
			Debug.Log ("tst attc md");
			Update ();
		}
	}

	void SpellTargetInputModeUpdate ()
	{


	}

	void NormalInputModeUpdate ()
	{
		ControlGroupsInput ();
		if (Input.GetKeyUp (KeyCode.A)) {
			ActivateAttackModifier ();
		}
		if (Input.GetButtonUp ("Fire2")) {
			if (MouseUi == false || cursorMinimap () == true)
				orderUnits ();
		}
		if (Input.GetButtonDown ("Fire1")) {
			if (MouseUi == false)
				startDragSelect ();
		}
		if (Input.GetButtonUp ("Fire1")) {
			if (cursorMinimap ()) {
				moveMinimap ();
			} else if (MouseUi == false) {
				endDragSelect ();
               
			}
			startClick = -Vector3.one;
		}
		if (Input.GetButton ("Fire1")) {
			scaleSelectionBox ();
		}

	}

	void Update ()
	{
		switch (m_InputMode) {
		case InputMode.spellTarget:
			SpellTargetInputModeUpdate ();
			break;
		case InputMode.attack:

			AttackInputModeUpdate ();

			break;

		default:

			NormalInputModeUpdate ();
			break;
		}
	}

	public void switchToMiniMapCam ()
	{
		// 
		// Debug.Log("im inside");
		currentCAM = minimapCam;
	}

	public void switchToMainCam ()
	{
		currentCAM = Camera.main;
		//  Debug.Log("im out");
	}

	//unit selection
	public void addUnit (baseRtsAI unit)
	{
		//Debug.Log(mySelection.Count);
		unit.gameObject.GetComponentInChildren<Projector> ().enabled = true;
		mySelection.Add (unit);
		//  Debug.Log(mySelection.Count);
	}

	public void removeUnit (baseRtsAI unit)
	{
		if (unit != null)
			unit.gameObject.GetComponentInChildren<Projector> ().enabled = false;
		// mySelection.Remove(unit);
	}

	public void ClearSelection ()
	{
		foreach (baseRtsAI unit in mySelection) {
			removeUnit (unit);
		}
		mySelection.Clear ();

		//   Debug.Log("cleared");
	}

	public void stopUnits ()
	{

		UnitOrders.removeOrders (mySelection);
	}

	void orderUnits ()
	{
		RaycastHit hit;
		Ray ray = currentCAM.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			PointOfInterest poi = hit.collider.gameObject.GetComponent<PointOfInterest> ();
			if (poi != null) {
				UnitOrders.giveOrders (mySelection, UnitOrders.OrderType.move, hit.point);
				mySelection [0].Orders.Clear ();
				mySelection [0].Orders.Enqueue (UnitOrders.CapturePoint (mySelection [0], poi));

				return;
			}
			IRtsUnit target = hit.collider.gameObject.GetComponent<IRtsUnit> ();
			//     Debug.Log(hit.collider.gameObject.name);
			if (target != null) {
				///   Debug.Log("target test");
				UnitOrders.giveOrders (mySelection, UnitOrders.OrderType.attackTarget, target);
				return;
			}
			UnitOrders.OrderType type;
			if (attackModifier) {
				type = UnitOrders.OrderType.attackMove;
			} else {
				type = UnitOrders.OrderType.move;
			}
			UnitOrders.giveOrders (mySelection, type, hit.point);
			attackModifier = false;
		}
	}

	bool cursorMinimap ()
	{
		return currentCAM == minimapCam;
	}

	private void moveMinimap ()
	{
		RaycastHit hit;
		Ray ray = currentCAM.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			cameraTarget.position = new Vector3 (hit.point.x, cameraTarget.position.y, hit.point.z);
		}

	}
   
	//selection stuff
	public bool IsWithingBounds (GameObject ob)
	{
		//   Bounds selectionBox = getViewPortBounds(Camera.main, startClick, Input.mousePosition);
		Vector3 camPos = Camera.main.WorldToScreenPoint (ob.transform.position);
		camPos.y = invertMouseY (camPos.y);
		return selection.Contains (camPos);

		//  selectionBox.Contains(ob.transform.position);
	}

	void startDragSelect ()
	{
		startClick = Input.mousePosition;
	}

	void endDragSelect ()
	{
		attackModifier = false;
		if (selection.width < 0) {
			selection.x += selection.width;
			selection.width = -selection.width;
		}
		if (selection.height < 0) {
			selection.y += selection.height;
			selection.height = -selection.height;
		}
        
		ClearSelection ();
		////left click selection
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			IRtsUnit unit = hit.collider.gameObject.GetComponent<IRtsUnit> ();
			if (unit != null) {
				addUnit (unit.getAIcomponent ());
			}
		}
		///drag selection
		foreach (IRtsUnit unit in RTSUnitManager.GetUnitList()) {
			// Debug.Log("test");
			if (IsWithingBounds (unit.GetGameObject ())) {
				if (unit.getFaction () == UnitFaction) {
					if (unit.getAIcomponent () != null)
						addUnit (unit.getAIcomponent ());
				}
			}
		}
	}

	void scaleSelectionBox ()
	{
		selection = new Rect (startClick.x, invertMouseY (startClick.y), Input.mousePosition.x - startClick.x, invertMouseY (Input.mousePosition.y) - invertMouseY (startClick.y));
	}

	//show selection box
	private void OnGUI ()
	{
		if (startClick != -Vector3.one) {
			GUI.color = new Color (1, 1, 1, 0.5f);
			GUI.DrawTexture (selection, selectionBox);
		}
	}

	public static float invertMouseY (float y)
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



