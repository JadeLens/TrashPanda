using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UnitStats_ForRTS : MonoBehaviour, IRtsUnit
{
    List<IObserver<OnAttackedInfo>> observers;
    baseRtsAI ai;
    IGameUnit target;
    [SerializeField]
    protected string targetKey = "Target";
    public string unitname;
    [SerializeField]
    protected float currentHealth;
    protected bool IsDead = false;

    public float sightRange = 6;
    public float attackRange = 5;
    public float attackDelay = 1;
    [SerializeField]
    protected float dmg = 10f;

    [SerializeField]
    protected float maxHealth = 100;
    Slider healthBar;
    private bool isAttacking = false;
    // Use this for initialization
    public faction myFaction;

    public faction getFaction()
    {
        return myFaction;
    }
    public baseRtsAI getAIcomponent()
    {

        return ai;
    }
    public AudioClip hurtSound;
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
    public float getMaxHealth()
    {
        return maxHealth;
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
        if (hurtSound)
            AudioManager.PlaySoundClip(hurtSound);
        //Debug.Log(unitname + " current health remaining : " + currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
            return true;
        }
        if (damage < 0)
        {
            OnAttackedInfo info;
            info.location = transform.position;
            foreach (IObserver<OnAttackedInfo> o in observers)
            {
                o.onUpdate(info);
            }


        }
        return false;
    }
    protected void Death()
    {
        IsDead = true;

        //this.gameObject.SetActive(false);
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }


    public void Register(IObserver<OnAttackedInfo> observer)
    {
        Debug.Log("observer");
        observers.Add(observer);
    }

    public void Unregister(IObserver<OnAttackedInfo> observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
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
        
        }
        

    }

    public IEnumerator AttackSphere(float range, float delay)
    {
        yield return new WaitForSeconds(delay); 

    }
    public void Awake()
    {

        observers = new List<IObserver<OnAttackedInfo>>();
    }
    void Start ()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = getMaxHealth();
        ai = gameObject.GetComponent<baseRtsAI>();
        currentHealth = maxHealth;
     
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(healthBar != null)
            healthBar.value = getCurrentHealth();
    }

    public float getAttackSpeed()
    {
        return attackDelay;
    }

    public void setFaction(faction newFaction)
    {
        myFaction = newFaction;
    }

    public float getAttackRange()
    {
        return attackRange;
    }

    public float getSightRange()
    {
        return sightRange;
    }


}
