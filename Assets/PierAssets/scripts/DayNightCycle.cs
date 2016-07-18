using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
    public Light sun;

    public float secondsInFullDay = 120f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;

    float sunInitialIntensity;
    public Color morningColor;
    public Color noonColor;
    public Color nightColor;
    /// <summary>
    /// stops the passage of time
    /// </summary>
    bool freezeTime = false;
    /// <summary>
    /// intensity of sun in the morning
    /// </summary>
    public float morningIntensity = 0.8f;
    /// <summary>
    /// intensity of sun at noon
    /// </summary>
    public float noonIntensity = 1.2f;
    /// <summary>
    /// intensity of sun at night
    /// </summary>
    public float nightIntensity = 0.5f;
    // Use this for initialization
    /// <summary>
    /// day percentage at which night time starts
    /// </summary>
    public float nightTreshold = 0.80f;
    /// <summary>
    /// day percentage at which day time starts
    /// </summary>
    public float dayTreshold = 0.25f;
    void Start () {
        sunInitialIntensity = sun.intensity;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateSun();
        if(!freezeTime)
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun()
    {
       // sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
     
       // float intensityMultiplier = 1;
      if(currentTimeOfDay < dayTreshold)
        {
            
            float colorRatio = currentTimeOfDay / dayTreshold;
            sun.intensity = Mathf.Lerp(nightIntensity, morningIntensity, colorRatio);
            sun.color = new Color(Mathf.Lerp(nightColor.r, morningColor.r, colorRatio),
                                  Mathf.Lerp(nightColor.g, morningColor.g, colorRatio),
                                  Mathf.Lerp(nightColor.b, morningColor.b, colorRatio));
        }
      else if(currentTimeOfDay >= dayTreshold &&  currentTimeOfDay <= nightTreshold)
        {
          //  sun.intensity = 1.2f;

            float colorRatio = (currentTimeOfDay - dayTreshold) / (nightTreshold - dayTreshold);
            sun.intensity = Mathf.Lerp( morningIntensity,noonIntensity, colorRatio);
            sun.color = new Color(Mathf.Lerp(morningColor.r, noonColor.r, colorRatio),
                                Mathf.Lerp(morningColor.g, noonColor.g, colorRatio),
                                Mathf.Lerp(morningColor.b, noonColor.b, colorRatio));


        }
        else
        {
         //   sun.intensity = 0.5f;
        
            float colorRatio = (currentTimeOfDay - nightTreshold) / (1 - nightTreshold);
            sun.intensity = Mathf.Lerp( noonIntensity, nightIntensity, colorRatio);
            // Debug.Log(currentTimeOfDay.ToString()+ "/"+ colorRatio.ToString());
            sun.color = new Color(Mathf.Lerp(noonColor.r, nightColor.r, colorRatio),
                               Mathf.Lerp(noonColor.g, nightColor.g, colorRatio),
                               Mathf.Lerp(noonColor.b, nightColor.b, colorRatio));

        }
       // sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
