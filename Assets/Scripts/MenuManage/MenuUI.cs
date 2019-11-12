using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour {
    Image cook;
    Timer destoryFoodMenuTimer;
    public Dictionary<string, FoodModel> levelFood;
    // Use this for initialization
    void Start() {
        levelFood = new Dictionary<string, FoodModel>();

    }
        // Update is called once per frame
        void Update ()
    {
        AddMenu();
        }
    void OnEnable()
    {
         destoryFoodMenuTimer= Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null); ;
    }
        void AddMenu()
    {
        
        if (destoryFoodMenuTimer.isDone)
            {
            
            Debug.Log("销毁菜单");
            Destroy(gameObject);
            }
        }
    }

