using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class customEditorWindow : EditorWindow
{
	public string Test = "test";
	bool settings = false;
	bool boolTest;
	float myFloat = 1.2f;

	[MenuItem ("Window/My Window")]

	public static void  ShowWindow ()
	{
		EditorWindow.GetWindow (typeof(customEditorWindow));
	}

	void OnGUI ()
	{
		GUILayout.Label ("base Settings", EditorStyles.boldLabel);
		Test = EditorGUILayout.TextField ("text field", Test);



		boolTest = EditorGUILayout.BeginToggleGroup ("optional settigns", boolTest);
		settings = EditorGUILayout.Toggle ("toggle", settings);
		myFloat = EditorGUILayout.Slider ("slider", myFloat, 0, 1);
		EditorGUILayout.EndToggleGroup ();
		// The actual window code goes here
	}
}
