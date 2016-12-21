using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(PlayerResources))]
/// <summary>
/// base class inherited by both the human player and ai player
/// </summary>
public class basePlayer : MonoBehaviour, IObserver<OnAttackedInfo>
{
    public faction UnitFaction;
    public List<baseRtsAI> mySelection;
   public MainBuilding myBuilding;
    public PlayerResources myResources;

    protected void Awake()
    {
        myResources = GetComponent<PlayerResources>();

    }

    public void onUpdate(OnAttackedInfo value)
    {
        Debug.Log("attacked at location " + value.location);
    }
}
