using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
	public float radius = 5.0f;
	public LayerMask layerMask = -1; 
	public bool MainPlayerOwned;
	//make a unit layer mask
	faction unitFaction = faction.neutral; //copied from units stats for most instance

	private bool observed = false;
	[SerializeField]
	private GameObject fogLight;
	[SerializeField]
	private MeshRenderer minimapRenderer;
	[SerializeField]
	private SkinnedMeshRenderer myRenderer;
	private Canvas myCanvas;
	// Use this for initialization
	void Start () {
		
	//	minimapRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
	//	myRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		myCanvas = this.gameObject.GetComponentInChildren<Canvas>();

		CheckIfMainPlayerOwned ();
	}
	public void setFaction(faction newFaction)
	{
		unitFaction = newFaction;
		CheckIfMainPlayerOwned ();
	}
	public void CheckIfMainPlayerOwned(){
		//always true
		if (GameManager.isMainPlayerFaction(unitFaction)) 
		{
			MainPlayerOwned = true;

		} 
		else 
		{
			MainPlayerOwned = false;
		}
		fogLight.SetActive (MainPlayerOwned);
	}
	// Update is called once per frame
	void Update () 
	{
		if (MainPlayerOwned) 
		{
			foreach (Collider col in Physics.OverlapSphere(transform.position, radius, layerMask)) 
			{
				FogOfWar fogscript = col.gameObject.GetComponent<FogOfWar> ();
				if (fogscript != null) 
				{
					fogscript.Observed ();
				}
			}
		} 
		else 
		{


			if (observed)
			{
				//this.gameObject.GetComponent<Renderer>().enabled = true;
				minimapRenderer.enabled = true;
				myRenderer.enabled = true;
				myCanvas.enabled = true;
			}
			else
			{
				//this.gameObject.GetComponent<Renderer>().enabled = false;
				minimapRenderer.enabled = false;
				myRenderer.enabled = false;
				myCanvas.enabled = false;
			}

			observed = false;
		}
	}
	public void Observed()
	{
		observed = true;
		//    Debug.Log("yo");
	}
}
