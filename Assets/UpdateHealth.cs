using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateHealth : MonoBehaviour
{

    public Slider healthBar;
    private UnitStats_ForRTS stats;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<UnitStats_ForRTS>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = stats.getMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = stats.getCurrentHealth();


    }
}

