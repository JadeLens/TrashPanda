using UnityEngine;
using System.Collections;

public class TestCameraMovement : MonoBehaviour
{ 
   // public Transform cameraTar;
   // public Quaternion cameraRot;
   // public Camera cam;
    //public float camSize;
    //public float ogCamSize = 5.5f;
    //public Vector3 ogCamRot = new Vector3(30.0f, 45, 0.0f);
    public int speed = 5;


    Vector3 left;
    Vector3 up;
    //Vector3 forward;

    void Start()
    {
        //transform.localEulerAngles
        //transform.localEulerAngles = new Vector3(45, 0, 0);
        //cameraTar = GameObject.Find("cameraTarget").GetComponent<Transform>();
        //cam = Camera.main;
        // ogCamSize = 5.5f;


        //left = Camera.main.WorldToViewportPoint(new Vector3(1.0f, 0.0f, 0.0f));
        //down = Camera.main.WorldToViewportPoint(new Vector3(0.0f, 1.0f, 0.0f));
        //transform.eulerAngles = new Vector3(0, 45, 0);

    }
    void Update()
    {
        //Drag left
        if (Input.mousePosition.x <= left.x && transform.position.x > -10)
        {
            transform.position += -transform.right / speed;
        }
        //Drag Right
        if (Input.mousePosition.x >= Screen.width && transform.position.x < 10)
        {
            transform.position += transform.right / speed;
        }
        //Drag Down
        if (Input.mousePosition.y <= up.y && transform.position.z > -15)
        {
            transform.position += -transform.forward / speed;
        }
        //Drag up
        if (Input.mousePosition.y >= Screen.height && transform.position.z < 10)
        {
            transform.position += transform.forward / speed;
        }
        //Reset Camera
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
           // cam.orthographicSize = ogCamSize;
            transform.position = new Vector3(0,10,0);
            transform.eulerAngles = new Vector3(0, 45, 0);
        }
        //Scroll Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > 3.71) //Down
        {
            transform.position += -transform.up / speed;
            transform.position += transform.forward / speed;
            //cam.orthographicSize = Mathf.Max(cam.orthographicSize - 1, camSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < 25) //Up
        {
            transform.position += transform.up / speed;
            transform.position += -transform.forward / speed;
            // cam.orthographicSize = Mathf.Max(cam.orthographicSize + 1, camSize);
        }
        //Middle Click Rotation
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.Rotate(Vector3.up, 100 * Time.deltaTime);
               // transform.RotateAround(Vector3.zero, Vector3.up, 100 * Time.deltaTime);
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.Rotate(-Vector3.up, 100 * Time.deltaTime);
               // transform.RotateAround(Vector3.zero, -Vector3.up, 100 * Time.deltaTime);
            }
        }
        //Increase Camera Speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5;
        }
        //WASD Camera Movement/rotation
        if (Input.GetKey(KeyCode.D) && transform.position.x < 10) //Right
        {
            transform.position += transform.right / speed;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > -10) //Left
        {
            transform.position += -transform.right / speed;
        }
        if (Input.GetKey(KeyCode.W) && transform.position.z < 10) //Up
        {
            transform.position += transform.forward / speed;
        }
        if (Input.GetKey(KeyCode.S) && transform.position.z > -15) //Down
        {
            transform.position += -transform.forward / speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-Vector3.up, 100 * Time.deltaTime);
            //transform.RotateAround(Vector3.zero, -Vector3.up, 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, 100 * Time.deltaTime);
            //transform.RotateAround(Vector3.zero, Vector3.up, 100 * Time.deltaTime);
        }

    }
}