using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    public GameObject researchHolder;
    bool running = false;
    Slider slider;
	// Use this for initialization
	void Start () {
        slider = this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
	if(running == true)
        {
            slider.value += Time.deltaTime;
            if(slider.value >= 10)
            {
                researchHolder.GetComponent<research>().finished();
                //GetComponentInParent<research>().finished();
            }
        }
	}

    public void StartTimer(GameObject caller)
    {
        running = true;
    }
}
