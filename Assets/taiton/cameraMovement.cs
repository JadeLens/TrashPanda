using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour
{
    Vector3 ogPosition;
    public Transform cameraTar;
    public Quaternion cameraRot;
    public Camera cam;
    public float camSize;
    public float ogCamSize = 5.5f;
    public Quaternion ogCamRot;
    public int speed = 5;

    public bool stopScroll = false;
    public bool stopScroll2 = false;
    public bool stopLeft = false;
    public float inScrollClamp;
    public float outScrollClamp;

    public float leftWallClamp;
    public float downWallClamp;
    public float rightWallClamp;
    public float upWallClamp;
    Vector3 left;
    Vector3 down;

    void Start()
    {
        
        cameraTar = GameObject.Find("CameraTarget").GetComponent<Transform>();
        cam = Camera.main;
        ogCamSize = 5.5f;
        ogPosition = transform.position;

        left = Camera.main.WorldToViewportPoint(new Vector3(1.0f, 0.0f, 0.0f));
        down = Camera.main.WorldToViewportPoint(new Vector3(0.0f, 1.0f, 0.0f));
        // transform.eulerAngles = new Vector3(0, 45, 0);

    
    }
    void Update()
    {
        //inScrollClamp = -5.0f;
        //outScrollClamp = 8.0f;
        ogCamRot = Quaternion.Euler(0.0f, 60.0f, 0.0f);
        //leftWallClamp = 0.0f;
        //downWallClamp = 0.0f;
        //rightWallClamp = 50.0f;
        //upWallClamp = 50.0f;

        if (cameraTar.position.y <= inScrollClamp)
        {
            cameraTar.position = new Vector3(cameraTar.position.x, inScrollClamp, cameraTar.position.z);
            stopScroll = true;
        }
        else
        {
            stopScroll = false;
        }
        if (cameraTar.position.y >= outScrollClamp)
        {
            cameraTar.position = new Vector3(cameraTar.position.x, outScrollClamp, cameraTar.position.z);
            stopScroll2 = true;
        }
        else
        {
            stopScroll2 = false;
        }

        if (cameraTar.position.x <= leftWallClamp)
        {
            cameraTar.position = new Vector3(leftWallClamp, cameraTar.position.y, cameraTar.position.z);
            stopLeft = true;
        }
        else
        {
            stopLeft = false;
        }
        if (cameraTar.position.z <= downWallClamp)
        {
            cameraTar.position = new Vector3(cameraTar.position.x, cameraTar.position.y, downWallClamp);
        }
        if (cameraTar.position.x >= rightWallClamp)
        {
            cameraTar.position = new Vector3(rightWallClamp, cameraTar.position.y, cameraTar.position.z);
        }
        if (cameraTar.position.z >= upWallClamp)
        {
            cameraTar.position = new Vector3(cameraTar.position.x, cameraTar.position.y, upWallClamp);
        }

        Vector3 temp = new Vector3(30.0f, 45, 0.0f);
        cameraRot = Quaternion.Euler(temp);
        //Drag left
        if (Input.mousePosition.x <= 0)
        {
            if(!stopLeft)
            cameraTar.position += -transform.right / speed;
        }
        //Drag Right
        if (Input.mousePosition.x >= Screen.width)
        {
            cameraTar.position += transform.right / speed;
        }
        //Drag Down
        if (Input.mousePosition.y <= 0)
        {
            cameraTar.position += -transform.forward / speed;
        }
        //Drag up
        if (Input.mousePosition.y >= Screen.height)
        {
            cameraTar.position += transform.forward / speed;
        }
        //Reset Camera
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            cameraTar.position = ogPosition;//new Vector3(cameraTar.position.x, 1.5f, cameraTar.position.z);
            //cameraTar.rotation = Quaternion.Slerp(transform.rotation, ogCamRot, 1.0f);
           // transform.rotation = Quaternion.Slerp(transform.rotation, ogCamRot, 1.0f);
        }
        //Scroll Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (!stopScroll)
            {
                cameraTar.position += transform.forward / speed;
                cameraTar.position += -transform.up / speed;
            }
            //  Debug.Log("WheelScroll");
            // cam.orthographicSize = Mathf.Max(cam.orthographicSize - 1, camSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (!stopScroll2)
            {
                cameraTar.position += -transform.forward / speed;
                cameraTar.position += transform.up / speed;
            }
            //   cam.orthographicSize = Mathf.Max(cam.orthographicSize + 1, camSize);
        }
        //Middle Click Rotation
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.RotateAround(cameraTar.transform.position, -Vector3.up, 100 * Time.deltaTime);
              //  Debug.Log("Move");
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.RotateAround(cameraTar.transform.position, Vector3.up, 100 * Time.deltaTime);
            //    Debug.Log("Move");
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
        if (Input.GetKey(KeyCode.D))
        {
            cameraTar.position += transform.right / speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraTar.position += -transform.right / speed;

        }
        if(Input.GetKey(KeyCode.W))
        {
           // transform.forward
            cameraTar.position += transform.forward / speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraTar.position += -transform.forward / speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(cameraTar.transform.position, -Vector3.up, 100 * Time.deltaTime);
         
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(cameraTar.transform.position, Vector3.up, 100 * Time.deltaTime);
        }

    }
}
