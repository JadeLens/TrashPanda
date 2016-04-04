using UnityEngine;
using System.Collections;

//Makes the object LookAt the Camera // Use on the Health UI
public class CameraLookAt : MonoBehaviour
{
    //Insert the Main Camera //Camera Target
    public Transform target;
    public GameObject[] cameras;

    void Update()
    {
        if (target != null)
        {
            transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
        }
        else
        {
            cameras = GameObject.FindGameObjectsWithTag("MainCamera");
            target = cameras[0].transform;
        }
    }
}