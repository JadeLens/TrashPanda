using UnityEngine;
using System.Collections;

public class SkunkMenuScript : MonoBehaviour
{

    public Transform spawnpoint;
    public Transform spawnpoint2;
    public Transform spawnpoint3;
    public float timer = 0.0f;
    private int spawner;
    public Rigidbody skunk;
    public Animator skunkAnim;


    void Update()
    {
        if (timer <= Time.time)
        {
            timer = Time.time + 15.0f;
            
            spawner = Random.Range(1, 4);
            spawn();
           
            // skunk.AddForce(spawnpoint.forward * 5000);
        }
    }

    void spawn()
    {
       
        if (spawner == 1)
        {

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawnpoint.transform.position;
            this.gameObject.transform.rotation = spawnpoint.transform.rotation;
            PlayAnim("walk");

        }
        else if (spawner == 2)
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawnpoint2.transform.position;
            this.gameObject.transform.rotation = spawnpoint2.transform.rotation;
            PlayAnim("walk");
        }
        else if (spawner == 3)
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawnpoint3.transform.position;
            this.gameObject.transform.rotation = spawnpoint3.transform.rotation;
            PlayAnim("walk");
        }

    }

    void PlayAnim(string animName)
    {
        skunkAnim.Play(animName);
    }
}
