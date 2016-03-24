using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(PlayerResources))]
/// <summary>
/// base class inherited by both the human player and ai player
/// </summary>
public class basePlayer : MonoBehaviour {
    public faction UnitFaction;
    public List<baseRtsAI> mySelection;

    public PlayerResources myResources;

    void Awake()
    {
        myResources = GetComponent<PlayerResources>();

    }
}
