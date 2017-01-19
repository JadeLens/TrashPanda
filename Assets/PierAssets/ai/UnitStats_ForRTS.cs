using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UnitStats_ForRTS : MonoBehaviour, IRtsUnit
{
	[SerializeField]
	private MeshRenderer MiniMapSprite;
	[SerializeField]
	private MeshRenderer teamColorRenderer;
    List<IObserver<OnAttackedInfo>> observers;
	FogOfWar fogControl;
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
    [SerializeField]
    private float attackDelay = 1;
    [SerializeField]
    protected float dmg = 10f;

    [SerializeField]
    protected float maxHealth = 100;
    Slider healthBar;
    private bool isAttacking = false;
    // Use this for initialization
    public faction myFaction;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Sprite portrait;

    public string getRace()
    {
        return unitname;
    }
    public Sprite getIcon()
    {
        return icon;
    }
    public Sprite getPortrait()
    {
        return portrait;
    }
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
    public Transform GetTransform()
    {
        return transform;
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
       //ebug.Log("observer");
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
		fogControl = gameObject.GetComponent<FogOfWar> ();
    }
    void Start ()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = getMaxHealth();
        ai = gameObject.GetComponent<baseRtsAI>();

		if(fogControl != null)
			fogControl.setFaction (myFaction);
        currentHealth = maxHealth;
		if (teamColorRenderer != null) 
		{
			teamColorRenderer.material.color = new Color(GameManager.getPlayer (myFaction).PlayerColor.r,
				GameManager.getPlayer (myFaction).PlayerColor.g,
				GameManager.getPlayer (myFaction).PlayerColor.b,
			.35f);
			
		}
		if(MiniMapSprite != null){
			MiniMapSprite.material.color =GameManager.getPlayer (myFaction).PlayerColor;

		}
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
		fogControl.setFaction (myFaction);
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
