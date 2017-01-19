using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public static List<basePlayer> playerList = new List<basePlayer>();
	public static basePlayer getMainPlayer (){

		return	GameObject.FindObjectOfType<Player> ();
	}
	public static bool isMainPlayerFaction(faction f){
		if (getMainPlayer ().UnitFaction == f) {
			return true;
		} else {
			return false;
		}
	}
	public static basePlayer getPlayer(faction f){
		foreach (basePlayer p in playerList) {

			if (p.UnitFaction == f) {
				return p;
			}
		}
		return  null;
	}
	// Use this for initialization
	public static void Register(basePlayer player)
	{
		if (!playerList.Contains(player))
		{
			// Debug.Log(unit.gameObject.name);
			playerList.Add(player);
		}
	}
	public static void Unregister(basePlayer player)
	{
		if (playerList.Contains(player))
		{
			playerList.Remove(player);
		}
	}
}
