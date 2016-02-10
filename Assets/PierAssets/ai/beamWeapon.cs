
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class beamWeapon : baseWeapon
{
    Vector2 mouse;
    RaycastHit hit;
    
    LineRenderer line;
    public Material lineMaterial;
    public Transform flashSprite;
    public Transform hitSprite;
    Transform burnMark;
    public bool isPlayerWeapon;
    bool hitOnce = false;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.GetComponent<Renderer>().material = lineMaterial;
           // renderer.material = lineMaterial;
        line.SetWidth(0.15f, 0.1f);
        line.enabled = false;
    }
  public override  IEnumerator attack()
    {
        

        line.enabled = true;
        if (flashSprite)
        {
            flashSprite.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;
        if (flashSprite)
        {
            flashSprite.gameObject.SetActive(false);
        } 
        hitOnce = false;
       // HitSprite.gameObject.SetActive(false);
    }
    void Update()
  {
      if (line.enabled)
      {
          Ray ray;
          if (isPlayerWeapon)
          {
              ray = (Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))); // ray to center of screen
          }
          else
          {
              ray = new Ray(transform.position, transform.forward);
          }
          if (Physics.Raycast(ray, out hit, range))
          {

              line.SetPosition(0, transform.position);  //sets the line position
              line.SetPosition(1, hit.point);
              if (hitOnce == false)
              {
               //   print("BURN");
                  if (hitSprite)
                  {
                      burnMark = Instantiate(hitSprite); //if this is the first frame of the hit spawn a burn sprite
                  }
                      hitOnce = true;
              }
              if (burnMark != null)
              {
                  burnMark.position = hit.point;  //if we have a burn mark move to to hit location
                  burnMark.rotation = hit.collider.transform.rotation;  // place it along the surface hit
              }
          }
          else
          {//if we havent hit just put the end of the line at max range
              line.SetPosition(0, transform.position);
              line.SetPosition(1, ray.GetPoint(range));
          }
      }

    }
}