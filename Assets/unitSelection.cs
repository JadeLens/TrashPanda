using UnityEngine;
using System.Collections;

public class unitSelection : MonoBehaviour {

	public bool selected = false;
	public Material[] materials;
	public float changeInterval = 0.33F;
	public Renderer rend;
	public Shader shader1;
	public Shader shader2;

	void Start()
	{
		shader1 = Shader.Find ("Legacy Shaders/Bumped Specular");
		shader2 = Shader.Find ("Outlined/Silhouetted Diffuse");
		rend.material.shader = shader1;
	}
	// Update is called once per frame
	void Update () {

		if (gameObject.GetComponent<Renderer>().isVisible && Input.GetMouseButtonUp (0)) {

			Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
			camPos.y = CameraSelection.invertMouseY(camPos.y);
			selected = CameraSelection.selection.Contains(camPos);


			int index = Mathf.FloorToInt(Time.time / changeInterval);

			//index = index % materials.Length;
			if(selected)
			{

				rend.material.shader = shader2;
				Debug.Log("selected");
			}
			else 
			{
				rend.material.shader = shader1;
			}
			//rend.sharedMaterial = materials[index];
				//gameObject.GetComponent<Renderer>().material.color = Color.red;
			// else
			//	gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	
	}
}
