using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshMovement : MonoBehaviour,IunitMovement 
{

	//private Vector3 m_destination;
	private NavMeshAgent m_agent;
	public IUnitAnim unitAnim;

	public	void removeDestination()
	{
		m_agent.SetDestination (this.transform.position);
	}

public	void SetDestination(Vector3 dest)
	{

		m_agent.SetDestination (dest);
	}
	// Use this for initialization
	void Start () 
	{
		m_agent = gameObject.GetComponent<NavMeshAgent> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
