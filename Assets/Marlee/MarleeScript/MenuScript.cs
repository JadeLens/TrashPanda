using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    //public GameObject spawn4;
    //public GameObject spawn5;
    //public GameObject spawn6;
    //public GameObject spawn7;
    public float timer = 0.0f;
    //public GameObject screamaudio;

    public float force = 2.0f;
    private int spawner;
    // Use this for initialization
    void Start()
    {
        //knockaudio.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= Time.time)
        {
            //screamaudio.SetActive(false);
            timer = Time.time + 6.0f;
            spawner = Random.Range(1, 4);
            spawn();
            if (spawner != 3)
            {
                AudioSource audio = GetComponent<AudioSource>(); GetComponent<AudioSource>();
                audio.Play();
            }

            Debug.Log("bye2");

        }

    }
    //IEnumerator waitformeow()
    //{
    //    yield return new WaitForSeconds(1);
    //    AudioSource audio = GetComponent<AudioSource>(); GetComponent<AudioSource>();
    //    audio.Play();

    //}
    void spawn()
    {

        if (spawner == 1)
        {

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawn1.transform.position;
            this.gameObject.transform.rotation = spawn1.transform.rotation;
            this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;

            // Debug.Log("hi1");
        }
        else if (spawner == 2)
        {

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawn2.transform.position;
            this.gameObject.transform.rotation = spawn2.transform.rotation;
            this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;

            //Debug.Log("hi2");
        }
        else if (spawner == 3)
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.gameObject.transform.position = spawn3.transform.position;
            this.gameObject.transform.rotation = spawn3.transform.rotation;
            this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;
            //Debug.Log("hi3");
        }
        //else if (spawner == 4)
        //{
        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //    this.gameObject.transform.position = spawn4.transform.position;
        //    this.gameObject.transform.rotation = spawn4.transform.rotation;
        //    this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;

        //    //Debug.Log("hi4");
        //}
        //else if (spawner == 5)
        //{
        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //    this.gameObject.transform.position = spawn5.transform.position;
        //    this.gameObject.transform.rotation = spawn5.transform.rotation;
        //    this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;

        //    //Debug.Log("hi4");
        //}
        //else if (spawner == 6)
        //{

        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //    this.gameObject.transform.position = spawn6.transform.position;
        //    this.gameObject.transform.rotation = spawn6.transform.rotation;
        //    this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;

        //    //Debug.Log("hi4");
        //}

        //else if (spawner == 7)
        //{
        //    //screamaudio.SetActive(true);
        //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //    this.gameObject.transform.position = spawn7.transform.position;
        //    this.gameObject.transform.rotation = spawn7.transform.rotation;
        //    this.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity + force * transform.forward;
        //    //StartCoroutine(waitformeow());
        //    //Debug.Log("hi4");
        //}
    }
}
