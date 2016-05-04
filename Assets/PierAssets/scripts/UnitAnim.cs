using UnityEngine;
using System.Collections;

public class UnitAnim : MonoBehaviour, IUnitAnim
{
    public string idle;
    public string run;
    public string attack;
    private Animator anim;
    public int index;
    public void crossFadeIdle()
    {
        
        anim.Play(idle);
        // throw new NotImplementedException();
    }

    public void playAttack()
    {
        //throw new NotImplementedException();
    }

    public void playIdle()
    {
       //  index = anim.GetLayerIndex(idle);
       
          anim.Play(idle);
        //throw new NotImplementedException();
    }

    public void playRun()
    {
        anim.Play(run);
        //throw new NotImplementedException();
    }

    // Use this for initialization
    void Start()
    {

        anim = GetComponentInChildren<Animator>();
    }
}
