using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour {
    Image cook;
    Image meterial;
    Image food;
    public Dictionary<string, FoodModel> levelFood;
    // Use this for initialization
    void Start () {
        levelFood = new Dictionary<string, FoodModel>();
        meterial = GameObject.Find("materialMenu/materialPannel/material").GetComponent<Image>();
        cook = GameObject.Find("materialMenu/cookPannel/cook").GetComponent<Image>();
        food= GameObject.Find("MenuFood/MenuFood/food").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void A()
    {
    }
}
