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

    //Fog Of War
    public MeshRenderer LightRenderer;
    public float sightRadius = 4.0f;
    public LayerMask layerMask = -1;

    ///////////////////////////////////////////////
    public AudioClip playerCapture;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindObjectsOfType<basePlayer>();
        //LightRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();

        render = this.GetComponent<Renderer>();
    }
    public void ChangeFaction(faction newFaction){

		owningFaction  = newFaction;

        CapturedTime = Time.time;
        LastInc = Time.time ;
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
            if(playerCapture)
                AudioManager.PlaySoundClip(playerCapture);
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

            TimeOwned += Time.deltaTime;
            if (TimeOwned >= IncRate )
            {
                TimeOwned = 0;
                players[currentPlayerIndex].myResources.IncrementTrash(incTrash);
                players[currentPlayerIndex].myResources.IncrementWater(incWater);

            }
            if (currentPlayerIndex == 1 && LightRenderer != null) //real player owns the object
            {
                LightRenderer.enabled = true;

                foreach (Collider col in Physics.OverlapSphere(transform.position, sightRadius, layerMask))
                {
                    if (col.gameObject.GetComponentInChildren<Renderer>() != null)
                    {
                        col.SendMessage("Observed", SendMessageOptions.DontRequireReceiver);
                        //col.gameObject.GetComponent<Renderer>().enabled = true;
                    }
                }
            }
            else if(LightRenderer != null)
            {
                LightRenderer.enabled = false;
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
