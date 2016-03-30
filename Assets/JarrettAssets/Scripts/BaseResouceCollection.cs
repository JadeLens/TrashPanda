using UnityEngine;
using System.Collections;

public class BaseResouceCollection : MonoBehaviour
{
    private PlayerResources Owner; //owner of the base

    //public bool bInfiniteBase = false; //is there cap on resources in the base?
    public bool bControlled = false; //does somebody own the base?

    public float TimeToCapture = 0.0f; //time to take control of the base
    private float enterTime;
    private float elapsedTime;

    public float IncRate = 1.0f;
    private float CapturedTime;
    private float TimeOwned = 0.0f;
    private float LastInc;

    private float TimeInBase = 0.0f;
    private bool bContested = false;
    private bool bInBase = false;

    //public int MaxTrash = 0; //Max Trash a Base Provides
    //public int MaxWater = 0; //Max Water a Base Provides
    public int incTrash = 0; //decrementation rate
    public int incWater = 0; //decrementation rate

    private MeshRenderer myRenderer;

    void OnTriggerEnter(Collider other)
    {
        // CollectResource Resource = (CollectResource)other.gameObject.GetComponent(typeof(CollectResource));
        ResourceCollisionHandling Player = (ResourceCollisionHandling)other.gameObject.GetComponent(typeof(ResourceCollisionHandling));
        Owner = Player.GetPlayer();
        StartCapturing();
    }

    void OnTriggerExit(Collider other)
    {
        StopCapturing();
    }

    void Start ()
    {
        myRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        //InvokeRepeating("getIncWater", 0, 1.0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if( Owner.tag == "Player")
        {
            Debug.Log("PlayerOwned");
        }
        if (!bInBase && !bControlled)
        {
            enterTime = Time.time;
        }
        else if (bInBase && !bControlled)
        {
            TimeInBase = Time.time - enterTime;
            if (TimeToCapture <= TimeInBase)
            {
                bControlled = true;
                CapturedTime = Time.time;
            }
        }
        else if (bControlled && !bContested)
        {
            //CapturedTime = Time.time;
            TimeOwned = Time.time - CapturedTime;
            if (TimeOwned >= IncRate + LastInc)
            {
                LastInc = Time.time - CapturedTime;
                Owner.IncrementTrash(getIncTrash());
                Owner.IncrementWater(getIncWater());
            }
            this.gameObject.GetComponentInChildren<Renderer>().enabled = true;
        }
    }

    void StartCapturing()
    {
        if (!bControlled && !bInBase && !bContested)
        {
            bInBase = true;
        }
        else if (bContested)
        {

        }
    }

    void StopCapturing()
    {
        if (!bContested)
        {
            bInBase = false;
        }
    }

    public int getIncWater()
    {
        return incWater;
    }
    public int getIncTrash()
    {
        return incTrash;
    }
}
