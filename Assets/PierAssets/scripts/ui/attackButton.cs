using UnityEngine;
using System.Collections;

public class attackButton : MonoBehaviour {


    private Player player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
    public void button()
    {
        player.ActivateAttackModifier();

    }
    // Update is called once per frame
    void Update () {
	
	}
}
