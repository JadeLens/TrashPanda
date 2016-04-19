using UnityEngine;
using System.Collections;

public class rabbitAnim : MonoBehaviour, IUnitAnim
{

	public Animation anim;
	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animation> ();

	}
	
    public void playRun()
    {

        anim.Play("run");

    }
    public void crossFadeIdle()
    {

        anim.CrossFade("idle1");

    }
    public void playAttack()
    {


    }
    public void playIdle()
    {

        anim.Play("idle1");
    }
	// Update is called once per frame
	void Update () {


	
	}
}
