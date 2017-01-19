using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMovement : MonoBehaviour,IunitMovement 
{
	public bool stationary;
	//private Vector3 m_destination;
	private NavMeshAgent m_agent;
	private NavMeshObstacle m_obstacle;
	public IUnitAnim unitAnim;

	public	void removeDestination()
	{
		m_agent.enabled = false;
		m_obstacle.enabled = true;

	//	m_agent.SetDestination (this.transform.position);
	//	stationary = true;
	}

public	void SetDestination(Vector3 dest)
	{
	//	stationary = false;
		m_obstacle.enabled = false;
		m_agent.enabled = true;
		m_agent.SetDestination (dest);


	}


	// Use this for initialization
	void Start () 
	{
		m_obstacle = gameObject.GetComponent<NavMeshObstacle> ();
		m_agent = gameObject.GetComponent<NavMeshAgent> ();
		unitAnim = GetComponent<IUnitAnim>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_agent.velocity.magnitude < 0.9f) 
		{
			stationary = true;
		} 
		else 
		{
			stationary = false;
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
}
