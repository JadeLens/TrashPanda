using UnityEngine;
using System.Collections;
using Pathfinding;

public class unitPath : MonoBehaviour {

	private unitSelection unit;
	private unitMovement unitM;
	private Seeker seeker;
	private CharacterController controller;
	public Path path;

	public float speed;
	public float nextWaypointDistance = 10;
	private int currentWaypoint = 0;

	// Use this for initialization
	void Start () {

		seeker = GetComponent<Seeker> ();
		controller = GetComponent<CharacterController> ();
		unit = GameObject.Find ("RABBIT/RABBIT").GetComponent<unitSelection> ();
		unitM = GameObject.Find ("cameraTarget/Main Camera").GetComponent<unitMovement> ();

	}

	public void LateUpdate ()
	{

		if (unit.selected) {
			if(Input.GetMouseButtonDown(1))
			{
			
			}
		}
	}
	public void OnPathComplete(Path p)
	{
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	public void FixedUpdate()
	{
		Vector3 targetPos = new Vector3 (0.0f, -0.2181511f, 0.0f);
		seeker.StartPath(transform.position,targetPos,OnPathComplete);
		
		if (path == null)
			return;

		if (currentWaypoint >= path.vectorPath.Count)
			return;

		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.SimpleMove (dir);

		if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {

			currentWaypoint++;
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
