using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour
{
    public Transform cameraTar;
    public Quaternion cameraRot;
    public Camera cam;
    public float camSize;
    public float ogCamSize = 5.5f;
    public Vector3 ogCamRot = new Vector3(30.0f, 45, 0.0f);
    public int speed = 5;


    Vector3 left;
    Vector3 down;

    void Start()
    {
        
        cameraTar = GameObject.Find("cameraTarget").GetComponent<Transform>();
        cam = Camera.main;
        ogCamSize = 5.5f;


        left = Camera.main.WorldToViewportPoint(new Vector3(1.0f, 0.0f, 0.0f));
        down = Camera.main.WorldToViewportPoint(new Vector3(0.0f, 1.0f, 0.0f));
        transform.eulerAngles = new Vector3(0, 45, 0);

    }
    void Update()
    {

        Vector3 temp = new Vector3(30.0f, 45, 0.0f);
        cameraRot = Quaternion.Euler(temp);
        //Drag left
        if (Input.mousePosition.x <= left.x)
        {
            cameraTar.position += -transform.right / speed;
        }
        //Drag Right
        if (Input.mousePosition.x >= Screen.width)
        {
            cameraTar.position += transform.right / speed;
        }
        //Drag Down
        if (Input.mousePosition.y <= down.y)
        {
            cameraTar.position += -transform.up / speed;
        }
        //Drag up
        if (Input.mousePosition.y >= Screen.height)
        {
            cameraTar.position += transform.up / speed;
        }
        //Reset Camera
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            cam.orthographicSize = ogCamSize;
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraRot, 1.0f);
        }
        //Scroll Zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log("WheelScroll");
            cam.orthographicSize = Mathf.Max(cam.orthographicSize - 1, camSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cam.orthographicSize = Mathf.Max(cam.orthographicSize + 1, camSize);
        }
        //Middle Click Rotation
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.RotateAround(Vector3.zero, Vector3.up, 100 * Time.deltaTime);
                Debug.Log("Move");
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.RotateAround(Vector3.zero, -Vector3.up, 100 * Time.deltaTime);
                Debug.Log("Move");
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
            cameraTar.position += transform.up / speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraTar.position += -transform.up / speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(Vector3.zero, -Vector3.up, 100 * Time.deltaTime);
         
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, 100 * Time.deltaTime);
        }

    }
}
