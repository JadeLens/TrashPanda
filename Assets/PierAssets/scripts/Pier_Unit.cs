using UnityEngine;
using System.Collections;
using Pathfinding;

public class Pier_Unit : MonoBehaviour {


   // public Vector3 targetPosition;
    //public Vector3 castPosition;
    //public Vector3 tP;

    private Seeker seeker;
    public IUnitAnim unitAnim;
   // public unitSelection unitS;


    public float separationCoeficient = 60;
    public float separationRadius = 1;
    public float destinationCoeficient = 25;
    //public CharacterController controller;
    public Path path;

    Ray camRay;
    RaycastHit camRayHit;

    public float nextWaypointDistance = 1f;

    public int speed = 5;
    private int currentWaypoint = 0;

    public LayerMask layerItem;

    public bool stationary;
    public bool lookat;
   public OnPathDelegate del;
    IEnumerator lookTowards(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        lookat = false;
    }

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        unitAnim = GetComponent<IUnitAnim>();
        //    controller = GetComponent<CharacterController>();
    //    rAnim = GameObject.Find("RABBIT").GetComponent<rabbitAnim>();
    //    unitS = GameObject.Find("RABBIT/RABBIT").GetComponent<unitSelection>();

        stationary = true;
        unitAnim.playIdle();

        layerItem = LayerMask.NameToLayer("ground");
        del = new OnPathDelegate(OnPathComplete);
            
    }

    public void OnPathComplete(Path p)
    {
  //      Debug.Log("pathComplete ... " + p.error);
        if (!p.error)
        {
    //        Debug.Log("path set ");
            path = p;
            stationary = true;
            currentWaypoint = 0;
        }
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lookat = true;
        }
    }
    //void part1()
    //{

    //    camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(camRay, out camRayHit))
    //    {
    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            lookat = true;
    //            if (camRayHit.transform.gameObject.layer == layerItem)
    //            {
    //                targetPosition = new Vector3(camRayHit.point.x, 0.0f, camRayHit.point.z);
    //            }
    //            //  seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    //        }
    //    }
    //}

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
   //     Debug.Log(currentWaypoint);
      


        Vector3 r = separationCoeficient * Seperation() + destinationCoeficient * Destination();
                   ;

        //   dir *= speed * Time.deltaTime;
        //     controller.SimpleMove(dir);
        r = r.normalized * speed * Time.deltaTime;
        r = new  Vector3(r.x, 0, r.z);
        transform.position += r;
     //   transform.Translate(dir * speed * Time.deltaTime,Space.World);
    //  transform.position = path.vectorPath[currentWaypoint];
        //  tP = new Vector3(0.0f, path.vectorPath[currentWaypoint].y, path.vectorPath[currentWaypoint].z);

        //Vector3 targetDir = path.vectorPath[currentWaypoint] - transform.position;
        //float step = speed * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        if (lookat)
        {
            //  transform.LookAt(path.vectorPath[currentWaypoint]);

            Vector3 relativePos = path.vectorPath[currentWaypoint] - new Vector3(transform.position.x, path.vectorPath[currentWaypoint].y, transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
            //  StartCoroutine(lookTowards(0.1f));
            lookat = false;
        }

    }
    public void FixedUpdate()
    {
        if (path == null)
        {
            stationary = true;
            // return;
        }
        else
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                stationary = true;
                unitAnim.crossFadeIdle();
                //   Debug.Log("End Of Path Reached");
                //     return;
            }
            else
            {
                part2Math();

                if (stationary)
                {

                    Vector3 relativePos = path.vectorPath[currentWaypoint] - new Vector3(transform.position.x, path.vectorPath[currentWaypoint].y, transform.position.z);
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = rotation;
                    lookat = true;
                    stationary = false;
                }

                if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                {
                    currentWaypoint++;
                    //        Debug.Log("next Pt");
                    lookat = true;
                    //  return;

                }
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

    //void LateUpdate()
    //{


    //    Debug.DrawRay(camRay.origin, camRay.direction * 50, Color.green);

    //}
}
