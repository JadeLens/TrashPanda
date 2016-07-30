using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using extensions;

public enum AItype {none, lamb, wolf,drone ,warrior,player};

//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(IGameUnit))]
public class aiBehavior : MonoBehaviour {

   public Dictionary<string, System.Object> blackBoard;

    /// <summary>
    /// type of ai  used for search seek and avoid 
    /// </summary>
    public AItype type; //to be remove or privatised

    /// <summary>
    /// root node of the tree
    /// </summary>
   public  aiBehaviorNode routine;
    //does the routine starts automatically
    public bool autoStart = false;
    /// <summary>
    /// how far can it sets its wander destination from current pos
    /// </summary>
    public float anchorRange = 10f;
    
    /// <summary>
    /// how far can it detect a target 
    /// </summary>
    private float detectionRange = 10;
    /// <summary>
    /// how close does it stop to a target
    /// </summary>
    public float SeekarriveRadius = 2.5f;
    public NavMeshAgent agent;

    public Queue<aiBehaviorNode> Orders;


    public IGameUnit stats;
    protected void Init()
    {

        stats = this.gameObject.GetComponent<IGameUnit>();
        blackBoard = new Dictionary<string, System.Object>();
        //stats = this.gameObject.get
    }
    // sets a routine based on ai type
    //these are mostly test routines
    void Awake()
    {

        Orders = new Queue<aiBehaviorNode>();

    }
	void Start () 
    {

        Init();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
      
        switch(type){
        case AItype.lamb:
            routine = CreateLamb();
        break;
        case AItype.wolf:
            routine = CreateWolf();
        break;
        default:
            routine = CreateDrone();
        break;

        }
        if (autoStart)
        {
            routine.Run();
        }
	}

    private aiBehaviorNode CreateDrone()
    {
        return new Node_Repeat
        (  
            new Node_PrioritySelector (
                new aiBehaviorNode[] 
                { 
                    new Node_FollowOrders(this),
                    new Node_Delay(0.5f)

                }
            )
        );
    
    }

    private aiBehaviorNode CreateLamb()
    {
        return new Node_Repeat
        (
            new Node_PrioritySelector
            (
                new aiBehaviorNode[] 
                { 
                    new Node_Flee(agent,detectionRange,0.3f,AItype.wolf),
                    new Node_Wander(anchorRange,agent),
                 
                }
            )
        );
    }    

    private aiBehaviorNode CreateWolf()
    {
        return new Node_Repeat
        (
            new Node_PrioritySelector
            (
                new aiBehaviorNode[] 
                {                
                    
                   new Node_Seek(agent,detectionRange,SeekarriveRadius,AItype.lamb) ,
                   new Node_Sequence
                    (
                        new aiBehaviorNode[] 
                        { 
                            new Node_PuffUp(3), 
                            new Node_PuffDown(3) 
                        }
                    )
                }
            )
        );
    }
    //resets the routine on mouse down
    //used for debug only
//also starts its if not yet started
    public void OnMouseDown()
    {
        if (routine.isReady()){
            //   routine.Run();
        }
        else{
            //routine.Reset();
           // routine.Run();
        }
    }
	// Update is called once per frame
    public void Update()
    {
        if (routine.isRunning())
        {
            routine.Act(this.gameObject);
        }
	}
}

//is going somewhere else
namespace extensions
{
    public static class FloatExtensions
    {
        public static void Double(this float i)
        {
           i= i + i;
        }
        public static void Double(this int i)
        {
            i = i + i;

        }
    }
}
/*
*/