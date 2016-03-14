using UnityEngine;
using System.Collections;

public class CameraSelection : MonoBehaviour {


	public Texture2D selectionBox = null;
	public static Rect selection = new Rect(0,0,0,0);
	private Vector3 startClick = -Vector3.one;
	

	// Update is called once per frame
	void Update () {
		CheckCamera ();
	}

	private void CheckCamera()
	{
		if (Input.GetMouseButtonDown (0)) {
			startClick = Input.mousePosition;
		}
		else if( Input.GetMouseButtonUp(0))
		{
			if(selection.width < 0)
			{
				selection.x += selection.width;
				selection.width = -selection.width;
			}
			if(selection.height < 0)
			{
				selection.y += selection.height;
				selection.height = -selection.height;
			}

			startClick = -Vector3.one;
		}

		if (Input.GetMouseButton (0))
        {

			selection = new Rect(startClick.x,invertMouseY(startClick.y),Input.mousePosition.x - startClick.x,invertMouseY(Input.mousePosition.y) - invertMouseY(startClick.y));
			                   
			                               
		}

	}

	private void OnGUI()
	{
		if (startClick != -Vector3.one)
        {
			GUI.color = new Color(1,1,1,0.5f);
			GUI.DrawTexture(selection, selectionBox);
		}
	}

	public static float invertMouseY(float y)
    {
	return Screen.height - y;
	}

}
