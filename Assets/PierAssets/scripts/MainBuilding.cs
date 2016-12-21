using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class MainBuilding : MonoBehaviour
{
  
	public IRtsUnit stats;
	public UnitBuilder builder;
	// Use this for initialization
	void Awake ()
	{
		stats = this.gameObject.GetComponent<IRtsUnit> ();
		builder = this.gameObject.GetComponent<UnitBuilder> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnEnable ()
	{
       
		RTSUnitManager.Register (stats);
	}

	void OnDisable ()
	{
		//        Debug.Log("game over");
		//      Debug.Break();
		RTSUnitManager.Unregister (stats);

		//  LoadScene.SafeLoad("GAMEOVER");

        
	}
   
}

