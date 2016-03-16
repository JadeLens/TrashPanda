using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour {

	public faction owningFaction;
	public Material mat1;
	public Material mat2;
	public int curentmat = 1;
	Renderer render;
	public void ChangeFaction(faction newFaction){
		owningFaction  = newFaction;
	}

public	void toggleMat(){
		Debug.Log(this.gameObject.name);
		if(curentmat ==1){
			curentmat =2;
			render.material = mat2;
		}
		else{
			curentmat =1;
			render.material = mat1;

		}
	}
	// Use this for initialization
	void Start () {
		render  = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
