using UnityEngine;
using System.Collections;

public class unitStats_ForAiTest : MonoBehaviour, IGameUnit
{
    public string unitname;
     [SerializeField]
    protected float currentHealth;
    protected bool IsDead = false;
    [SerializeField]
    protected float dmgUpper = 10f;
    [SerializeField]
    protected float dmgLowe = 2f;
      [SerializeField]
    protected float maxHealth = 100;
    private bool isAttacking = false;


    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
     
	}

    public bool GetIsAttacking(){return isAttacking;}
    public void AttackTrue()
    {
        isAttacking = true;
    }

    public void AttackFalse()
    {
        isAttacking = false;
    }
    public bool CheckIfAlive() { return IsDead; }
   
    public float getCurrentHealth()
    { return currentHealth; }

    public virtual float GetDamage()
    {
        isAttacking = false;
        float damage = Random.Range(dmgLowe / 2, dmgUpper / 2) + Random.Range(dmgLowe / 2, dmgUpper / 2);
        return Mathf.Round(damage);
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
    /// <summary>
    /// the delay is to sync it up with the animations
    /// </summary>
    /// <param name="range"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator Attack(float range,float delay)
    {
        yield return new WaitForSeconds(delay);
        Ray ray;
        RaycastHit hit;
        if (this.gameObject.tag == "Player")
        {
            ray = (Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))); // ray to center of screen
        }
        else
        {
            ray = new Ray(transform.position, transform.forward);
        }
        if (Physics.Raycast(ray, out hit, range))
        {
            IGameUnit target = hit.collider.transform.gameObject.GetComponent(typeof(IGameUnit)) as IGameUnit;
    //        Debug.Log("hit something");

       //     Debug.Log(hit.collider.transform.gameObject.name);
            if (target != null && target != this)
            {
               // Debug.Log("hit something");
                float damage = GetDamage();
                if (target.changeHealth(-damage)) //returns true if target died
                {
                    //nothing for now
                }
            }
        }
    }
    public IEnumerator AttackSphere(float range, float delay)
    {
        yield return new WaitForSeconds(delay);
        //this.animationCtrl.SetFloat("HorizontalSpeed", 0f);
        //  this.animationCtrl.SetTrigger("attack");
        //  Vector2 endPos;
        // if (FaceRight)

        Collider[] hitColliders;
        hitColliders = Physics.OverlapSphere(transform.position, range);
        if (hitColliders.Length > 0)
        {
            foreach (Collider col in hitColliders)
            {
                IGameUnit target = col.transform.gameObject.GetComponent(typeof(IGameUnit)) as IGameUnit;
                if (target != null && target != this)
                {
                    
                    float damage = GetDamage();
                    if (target.changeHealth(-damage)) //returns true if target died
                    {
                        //nothing for now
                    }
                }
            }


        }
    }

}
