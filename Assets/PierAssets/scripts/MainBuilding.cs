﻿using UnityEngine;
using System.Collections;

public class MainBuilding : MonoBehaviour {
    IRtsUnit stats;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnEnable()
    {
        stats = this.gameObject.GetComponent<IRtsUnit>();
        RTSUnitManager.Register(stats);
    }

    void OnDisable()
    {
        //        Debug.Log("game over");
        //      Debug.Break();

        Application.LoadLevel("GAMEOVER");
        RTSUnitManager.Unregister(stats);

    }
}
