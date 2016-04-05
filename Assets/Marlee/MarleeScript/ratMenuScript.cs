using UnityEngine;
using System.Collections;

public class ratMenuScript : MonoBehaviour {

    public Transform spawnpoint;
  
    public float timer = 0.0f;
 
    public Rigidbody rats;
    public Animator ratAnim1;
    public Animator ratAnim2;
    public Animator ratAnim3;
    public Animator ratAnim4;
    public Animator ratAnim5;
    public Animator ratAnim6;



    void Update()
    {
        if (timer <= Time.time)
        {
            timer = Time.time + 30.0f;

           // spawner = Random.Range(1, 4);
            spawn();

            // skunk.AddForce(spawnpoint.forward * 5000);
        }
    }

    void spawn()
    {

       

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawnpoint.transform.position;
            this.gameObject.transform.rotation = spawnpoint.transform.rotation;
            PlayAnim("run");

    
        

    }

    void PlayAnim(string animName)
    {
        ratAnim1.Play(animName);
        ratAnim2.Play(animName);
        ratAnim3.Play(animName);
        ratAnim4.Play(animName);
        ratAnim5.Play(animName);
        ratAnim6.Play(animName);
    }
}
