﻿using UnityEngine;
using System.Collections;

public class UnitStats_ForRTS : MonoBehaviour,IGameUnit
{
    baseRtsAI ai;
    IGameUnit target;
    [SerializeField]
    protected string targetKey;
    public string unitname;
    [SerializeField]
    protected float currentHealth;
    protected bool IsDead = false;
    [SerializeField]
    protected float dmg = 10f;

    [SerializeField]
    protected float maxHealth = 100;
    private bool isAttacking = false;
    // Use this for initialization

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
    public void AttackTrue()
    {
        isAttacking = true;
    }

    public void AttackFalse()
    {
        isAttacking = false;
    }
    public bool CheckIfAlive()
    {
        return IsDead;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
    public virtual float GetDamage()
    {
        isAttacking = false;
        return -dmg;
    }
    /// <summary>
    /// send it negative value for damage
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool changeHealth(float damage)
    {
        currentHealth += damage;
        //Debug.Log(unitname + " current health remaining : " + currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
            return true;
        }
        return false;
    }
    protected void Death()
    {
        IsDead = true;

        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public IEnumerator Attack(float range, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        GameObject ob = Node_IsNull.GetBBVar(ai.blackBoard, targetKey) as GameObject;
        if(ob!= null)
            target = ob.GetComponent<IGameUnit>();
        if (target != null)
        {
            target.changeHealth(GetDamage());
            Debug.Log("testing");
        }
        

    }

    public IEnumerator AttackSphere(float range, float delay)
    {
        yield return new WaitForSeconds(delay); 

    }


    void Start ()
    {
        ai = gameObject.GetComponent<baseRtsAI>();
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
