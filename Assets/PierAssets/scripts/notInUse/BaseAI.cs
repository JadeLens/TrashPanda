using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAI : aiBehavior {



    public Pier_path normalPath;
	public Transform homeLoc;
	public Transform meetingLoc;

	public bool AlarmOn = false;
	public bool dayMeeting = false;
	private bool pauseAI = false;

	Dictionary<string, bool> blackBoard;
	// Use this for initialization
	void Start () {
		
		Init();
		blackBoard = new Dictionary<string, bool> ();
		blackBoard.Add ("alarm", false);
		blackBoard.Add ("meeting", false);
		//routine = CreateBaseVillager();
		//routine = test();
		if (autoStart)
		{
			routine.Run();
		}

	}
	
	// Update is called once per frame
	public void FixedUpdate()
	{
		if (!pauseAI) {
			if (routine.isRunning ()) {
				routine.Act (this.gameObject);
			}
		}
		blackBoard ["alarm"] = AlarmOn;
		blackBoard ["meeting"] = dayMeeting;
	}
	private aiBehaviorNode CreateBaseVillager()
	{
		//return new Node_Repeat
		//	(
		return	null;//new Node_Follow_Path(agent,normalPath.points);
			//);
	}    
//	private aiBehaviorNode test()
//	{
//		return new Node_Repeat
//		(
//			new Node_Selector
//			(
//				new aiBehaviorNode[] 
//				{
//					new Node_Concurent
//					(
//						new aiBehaviorNode[] 
//						{ 	
//							new Node_Invert//if == false
//							(
//								new Node_CheckBool (blackBoard,"alarm")
//							)
//							,
//							new Node_CheckBool (blackBoard,"meeting")
//							,
//							new Node_MoveTo_With_Avoid(agent, meetingLoc.position)
//						}
//
//					)
//					,
//					new Node_Concurent
//					(
//						new aiBehaviorNode[] 
//						{ 
//							new Node_Invert//if == false
//							(
//								new Node_CheckBool (blackBoard,"alarm")
//							)
//							,
//							new Node_Follow_Path(agent,normalPath.points)
//						}
//
//					)
//				,
//				new Node_MoveTo_With_Avoid(agent,homeLoc.position)
//				}
//			)
//		
//		);
//
//	}
}
