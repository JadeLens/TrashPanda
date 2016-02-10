using UnityEngine;
using System.Collections;

public abstract class baseWeapon : command
{
    public abstract  IEnumerator attack();
    public float range = 100.0f;
    public float damage= 10;
    public float delay= 0;

    //to chane functionality in a child class juste use the override
    //default way to call an attack

    public override void onActivate(IGameUnit caster)
    {
        //called onbuttondown
        StartCoroutine(caster.Attack(range, delay));
        StartCoroutine(attack());

    }

    //used for charging up
    public override void onChanel(IGameUnit caster)
    {


    }

    //release charge up
    public override void onRelease(IGameUnit caster)
    {


    }
}
