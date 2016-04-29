using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class cursorScript : MonoBehaviour {
    public RectTransform cursor;
    public float speed = 25;
    Button[] allButtons;
    
	// Use this for initialization
	void Start () {
        allButtons = GameObject.FindObjectsOfType<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0) * speed * Time.deltaTime;
       // transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * speed * Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            Debug.Log(cursor.anchoredPosition);
          
            foreach (Button btn in allButtons)
            {
               // RectTransformUtility.
           //     Debug.Log(((RectTransform)btn.transform).anchoredPosition);
                if (((RectTransform)btn.transform).rect.Contains(cursor.anchoredPosition, true))
                {

                    Debug.Log("Im inside");

                }

            }
            }
    }
}
