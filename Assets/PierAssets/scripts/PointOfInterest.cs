using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PointOfInterest : MonoBehaviour
{
    public static basePlayer[] players;
	public faction owningFaction;
	public Material mat1;
	public Material mat2;
	public int curentmat = 1;
    public int currentPlayerIndex = -1;
	Renderer render;


    ///copied from baseResourceColletion
    public float TimeToCapture = 0.0f; //time to take control of the base
    private float enterTime;
    private float elapsedTime;

    public float IncRate = 1.0f;
    private float CapturedTime;
    private float TimeOwned = 0.0f;
    private float LastInc;

    private float TimeInBase = 0.0f;

    //public int MaxTrash = 0; //Max Trash a Base Provides
    //public int MaxWater = 0; //Max Water a Base Provides
    public int incTrash = 2; //decrementation rate
    public int incWater = 1; //decrementation rate

    ///////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindObjectsOfType<basePlayer>();

     
        render = this.GetComponent<Renderer>();
    }
    public void ChangeFaction(faction newFaction){

		owningFaction  = newFaction;

        CapturedTime = Time.time;

    }
    public void CapturePT(baseRtsAI unit)
    {
        ChangeFaction(unit.UnitFaction);
        toggleMat();
        for (int i = 0; i < players.Length; i++)
        {

            if (players[i].UnitFaction == owningFaction)
            {
                currentPlayerIndex = i;
                break;
            }
        }

    }

    public	void toggleMat()
    {
	//	Debug.Log(this.gameObject.name);
		if(owningFaction == faction.faction1)
        {
			curentmat =2;
			render.material = mat2;
		}
		else
        {
			curentmat =1;
			render.material = mat1;
		}
	}
	
	
	// Update is called once per frame
	void Update ()
    {
        if (currentPlayerIndex != -1)
        {

            TimeOwned = Time.time - CapturedTime;
            if (TimeOwned >= IncRate + LastInc)
            {
                LastInc = Time.time - CapturedTime;
                players[currentPlayerIndex].myResources.IncrementTrash(incTrash);
                players[currentPlayerIndex].myResources.IncrementWater(incWater);
            }
        }
    }


    void OnEnable()
    {
        HouseManager.Register(this);
    }

    void OnDisable()
    {
        HouseManager.Unregister(this);

    }
}
