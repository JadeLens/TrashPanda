﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomPanelUI : MonoBehaviour {
    public basePlayer player;
    public Transform iconPrefab;
    public Image portraitOb;

    public Transform IconParent;
    public Text damageText;
    public Text attackSpeedText;
    public Text RangeText;
    public Text HealthText;

    public void addUnit(Sprite portrait, Sprite Icon)
    {
        GameObject IconOb = Instantiate(iconPrefab,Vector3.zero, Quaternion.identity) as GameObject;
      
          IconOb.GetComponent<Image>().sprite = Icon;
        portraitOb.sprite = portrait;

        IconOb.transform.SetParent(IconParent);
        FixRectTransform(IconOb.transform  as RectTransform);
    }

    public static void FixRectTransform(RectTransform rect)
    {//not needed anymore  because of setParent keepWorldPosition set to false
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.Euler(Vector3.zero);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
            
            IconParent.gameObject.SetActive(!(player.mySelection.Count == 1));

            if(IconParent.childCount != player.mySelection.Count)
            {
                for (int i = 0; i < IconParent.childCount; i++)
                {
                    Destroy(IconParent.GetChild(i).gameObject);

                }
                portraitOb.sprite = null;
                foreach (baseRtsAI unit in player.mySelection)
                {
                    addUnit(unit.stats.getPortrait(), unit.stats.getIcon());

                }
            }
        }
	
}
