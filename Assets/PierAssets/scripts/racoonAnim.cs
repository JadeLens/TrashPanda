using UnityEngine;
using System.Collections;
using System;

public class racoonAnim : MonoBehaviour, IUnitAnim
{
    public Animator anim;
    public void crossFadeIdle()
    {
        anim.Play("idle01");
        // throw new NotImplementedException();
    }

    public void playAttack()
    {
        //throw new NotImplementedException();
    }

    public void playIdle()
    {
        anim.Play("idle01");
        //throw new NotImplementedException();
    }

    public void playRun()
    {
        anim.Play("run");
        //throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {

        anim = GetComponentInChildren< Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
