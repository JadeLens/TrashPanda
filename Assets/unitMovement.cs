using UnityEngine;
using System.Collections;
using Pathfinding;

public class unitMovement : MonoBehaviour {

	public Vector3 targetPosition;
    public Vector3 castPosition;
    public Vector3 tP;

    public Seeker seeker;
	public rabbitAnim rAnim;
	public unitSelection unitS;
	private CharacterController controller;
	public Path path;

    Ray camRay;
    RaycastHit camRayHit;

    public float nextWaypointDistance = 0.1f;

    public int speed = 200;
    private int currentWaypoint = 0;
	
	public LayerMask layerItem;
	
	public bool stationary;
	public bool lookat;

	IEnumerator lookTowards(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		lookat = false;
	}

	public void Start ()
    {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		rAnim = GameObject.Find ("RABBIT").GetComponent<rabbitAnim> ();
		unitS = GameObject.Find ("RABBIT/RABBIT").GetComponent<unitSelection> ();
		
		stationary = true;
		rAnim.anim.Play ("idle1");

		layerItem = LayerMask.NameToLayer ("ground");
	}
	public void OnPathComplete (Path p)
    {
		Debug.Log ("pathComplete "+p.error);
		if (!p.error)
        {
			path = p;
			stationary = true;
			currentWaypoint = 0;
		}
	}
	public void Update ()
    {
		if (Input.GetMouseButtonDown (1))
        {
			lookat = true;
		}
	}

	public void FixedUpdate()
	{


		camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (camRay, out camRayHit))
        {
			if (Input.GetMouseButtonDown (1))
            {
				lookat = true;
				if (camRayHit.transform.gameObject.layer == layerItem)
                {
					targetPosition = new Vector3 (camRayHit.point.x, 0.0f, camRayHit.point.z);
				}
				seeker.StartPath (transform.position, targetPosition, OnPathComplete);
			}
		}
		
		if (path == null)
        {
			stationary = true;
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count)
        {
			stationary = true;
			rAnim.anim.CrossFade ("idle1");
			Debug.Log ("End Of Path Reached");
			return;
		}
	
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.deltaTime;
		controller.SimpleMove (dir);
		tP = new Vector3 ( 0.0f, path.vectorPath [currentWaypoint].y,  path.vectorPath [currentWaypoint].z);

		Vector3 targetDir = path.vectorPath [currentWaypoint] - transform.position;
		float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0f);

		if (lookat)
        {
			transform.LookAt (path.vectorPath [currentWaypoint]);
			StartCoroutine (lookTowards (0.1f));

		}
		if (stationary)
        {
			transform.LookAt (path.vectorPath [currentWaypoint]);
			lookat = true;
			stationary = false;
		} 
		if (!stationary)
        {
			rAnim.anim.Play ("run");
		}
        else
        {
			rAnim.anim.Play ("idle1");
		}
		
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
			currentWaypoint++;
			lookat = true;
			return;
		
		}

	}

	void LateUpdate () 
	{


		Debug.DrawRay (camRay.origin, camRay.direction * 50, Color.green);
		
	}
} 


