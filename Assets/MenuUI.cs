using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour {
    Image cook;
    
    public Dictionary<string, FoodModel> levelFood;
    // Use this for initialization
    void Start () {
        levelFood = new Dictionary<string, FoodModel>();
        
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    //void A()
    //{
    //    foreach (KeyValuePair<string,FoodModel> item in levelFood)
    //    {
    //       //Sprite a = Resources.Load<Sprite>(item.Value.normalUI);
    //      //food.sprite=Instantiate(a);
    //    }
    //}
}
