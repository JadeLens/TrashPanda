using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// base class inherited by both the human player and ai player
/// </summary>
public class basePlayer : MonoBehaviour {
    public faction UnitFaction;
    public List<baseRtsAI> mySelection;

}
