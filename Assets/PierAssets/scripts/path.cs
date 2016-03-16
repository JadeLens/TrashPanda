using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Pier_path : MonoBehaviour {
	public Transform[] points;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (points.Length > 1)
		{
			for (int i = 0; i < points.Length; i++) {
				int next;
				next = i + 1;
				if (i + 1 >= points.Length) {
					next = 0;
				}

				Debug.DrawRay (points [i].position, points [next].position - points [i].position, Color.black);
			}

		}
	}
}
