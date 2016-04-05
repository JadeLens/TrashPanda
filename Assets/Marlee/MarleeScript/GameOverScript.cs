using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    public Animator skunk;
    public Animator raccoon;
    public Animator rabbit;
    public Animator rabbit2;
    public Animator rabbit3;
    public Animator rat1;
    public Animator rat2;
    public Animator rat3;
    public Animator rat4;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitandDie(2));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator WaitandDie(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayAnim("die");
        PlayRatAnim("dead");

    }

    void PlayAnim(string animName)
    {
        skunk.Play(animName);
        rabbit.Play(animName);
        rabbit2.Play(animName);
        rabbit3.Play(animName);
        raccoon.Play(animName);
        

    }
    void PlayRatAnim(string animName)
    {
        rat1.Play(animName);
        rat2.Play(animName);
        rat3.Play(animName);
        rat4.Play(animName);
    }
}
