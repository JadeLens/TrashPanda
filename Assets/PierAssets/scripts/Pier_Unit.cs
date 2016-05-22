using UnityEngine;
using System.Collections;
using Pathfinding;

public class Pier_Unit : MonoBehaviour {
  public  bool debug = false;

    // public Vector3 targetPosition;
    //public Vector3 castPosition;
    //public Vector3 tP;
     Vector3 DestinationGoal;
    private Seeker seeker;
    public IUnitAnim unitAnim;
   // public unitSelection unitS;


    public float separationCoeficient = 60;
    public float separationRadius = 1;
    public float destinationCoeficient = 25;
    //public CharacterController controller;
    public Path path;
    

    public float nextWaypointDistance = 1f;

    public int speed = 5;
    public int currentWaypoint = 0;

  //  public LayerMask layerItem;

    public bool stationary;
    public bool lookat;
   public OnPathDelegate del;

    public void SetDestination(Vector3 dest)
    {
        if (DestinationGoal != dest)
        {
            DestinationGoal = dest;
          if(debug)
            {
                Debug.Log("started new path");

            }
            path = seeker.StartPath(transform.position, DestinationGoal, del);

        }

    }
    void Awake()
    {

        seeker = GetComponent<Seeker>();
        unitAnim = GetComponent<IUnitAnim>();

        //layerItem = LayerMask.NameToLayer("ground");
        del = new OnPathDelegate(OnPathComplete);
     //   Debug.Log("setUp");
    }
    public void Start()
    {
      //  Debug.Log("setUp");

    }

    public void OnPathComplete(Path p)
    {
  //      Debug.Log("pathComplete ... " + p.error);
        if (!p.error)
        {
            if(debug)
                Debug.Log("path set ");
            path = p;
            currentWaypoint = 0;
        }
    }

    Vector3 Seperation()
    {
        // seperation behaviour steers boids into opposite directions of their neighbours
        Vector3 r = new Vector3();
        // get all neghbours in radius R
        var neighbours = RTSUnitManager.GetUnitList(); //world.GetNeighbours(this, config.Rseperation);
        // if no neighboiurs then no seperation needed
        if (neighbours.Count == 0)
        {
            return r;
        }
        // adding contibuting vectors of neghbours towarsds this boid
        foreach (var agent in neighbours)
        {
            // checking for field of vision boids
            if (Vector3.Distance(agent.GetGameObject().transform.position, this.transform.position) < separationRadius)
            {
                Vector3 towardsMe = this.transform.position - agent.GetGameObject().transform.position;

                if (towardsMe.magnitude > 0)
                {
                    // force that each agent has on me is inversely proportional to the distance
                    r += towardsMe.normalized / towardsMe.magnitude;

                }

            }
        }

        return r.normalized;
    }
    Vector3 Destination()
    {
        return   (path.vectorPath[currentWaypoint] - transform.position).normalized;


    }
    void part2Math()
    {
        Vector3 r = separationCoeficient * Seperation() + destinationCoeficient * Destination();
                   

        r = r.normalized * speed * Time.deltaTime;
        r = new  Vector3(r.x, 0, r.z);
        transform.position += r;
  
        //if (lookat)
        //{
            //  transform.LookAt(path.vectorPath[currentWaypoint]);

            Vector3 relativePos = path.vectorPath[currentWaypoint] - new Vector3(transform.position.x, path.vectorPath[currentWaypoint].y, transform.position.z);
            if (relativePos != Vector3.zero)
            { 
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = rotation;
            }
            //  StartCoroutine(lookTowards(0.1f));
            //lookat = false;
        //}

    }
    public void FixedUpdate()
    {
       
        if (path != null && currentWaypoint < path.vectorPath.Count)
        {
            stationary = false;
            part2Math(); //movement and rotation

            //are we at wayPoint
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;

            }
            if (currentWaypoint >= path.vectorPath.Count )
            {
                stationary = true;
                currentWaypoint = path.vectorPath.Count;
                path = null;
                unitAnim.crossFadeIdle();
              //  Debug.Log("test");
            }

        }
    

        if (!stationary )
        {
            unitAnim.playRun();
        }
        else
        {
            unitAnim.playIdle();
        }

      

    }

    void LateUpdate()
    {


       Debug.DrawRay(transform.position, DestinationGoal - transform.position, Color.white);

    }
}
