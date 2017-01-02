using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

// Create a dockable empty window at the top left corner of the screen
// with 100px width and 150px height
public class EditorWindowTest : EditorWindow
{
	static Rect myBox;
	static Color boxColor;
	public	UnityEvent m_MyEvent;
	public string Test = "test";

	Rect windowRect = new Rect (100 + 100, 100, 300, 400);
	Rect windowRect2 = new Rect (100, 100, 300, 400);

	[MenuItem ("Example/Display simple sized Window")]
	static void Initialize ()
	{
		EditorWindowTest window = (EditorWindowTest)EditorWindow.GetWindowWithRect (typeof(EditorWindowTest), new Rect (0, 0, 400, 400));
		myBox = new Rect (100, 200, 50, 50);
		boxColor = Color.red;
	}

	void OnGUI ()
	{
		GUILayout.Label ("Base yooo", EditorStyles.boldLabel);
		boxColor = EditorGUI.ColorField (new Rect (0, 50, 60, 30), boxColor);
		GUILayout.Label ("adv yooo", EditorStyles.boldLabel);
		EditorGUI.DrawRect (myBox, boxColor);

		Handles.BeginGUI ();
		Handles.DrawBezier (windowRect.center, windowRect2.center, new Vector2 (windowRect.xMax + 50f, windowRect.center.y), new Vector2 (windowRect2.xMin - 50f, windowRect2.center.y), Color.red, null, 5f);
		Handles.EndGUI ();


		BeginWindows ();
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Box1");
		windowRect2 = GUI.Window (1, windowRect2, WindowFunction, "Box2");

		EndWindows ();

	}

	SerializedObject test;




	void WindowFunction (int windowID)
	{
		Test = EditorGUILayout.TextField (Test);
		test = new SerializedObject (this);
		SerializedProperty tt = test.FindProperty ("m_MyEvent");
		EditorGUILayout.PropertyField (tt);
		GUI.DragWindow ();
	}
}
