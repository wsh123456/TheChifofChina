using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour {
    Image cook;
    Timer destoryFoodMenuTimer;
    public Image food;
    Image meterial;
    string foodSprite;
    Dictionary<string, FoodModel> levelFood;
    string menuStr;
    string meterialStr;
    AddMenu addMenu;
     Dictionary<string, FoodIngredientModel> levelIngredient;
    // Use this for initialization
    void Awake() {
        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        food = transform.Find("FoodPoint/MenuFood/food"). GetComponent<Image>();
        addMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<AddMenu>();
        meterial = transform.Find("materialPoint/mterial/materialPannel/material").GetComponent<Image>();
        //cook = transform.Find("cookPannel/cook").GetComponent<Image>();
        //Debug.Log(meterialStr);
        menuStr = MenuManage.menuManage.menuStr;
        meterialStr=levelFood[menuStr].foodIngredient[0].name;
        foodSprite = levelFood[menuStr].normalUI;
        meterial.sprite = Resources.Load<Sprite>(levelIngredient[meterialStr].normalUI);
        food.sprite = Resources.Load<Sprite>(foodSprite);

        //Debug.Log("simple : " + menuStr);
    }
        // Update is called once per frame
        void Update ()
    {
        //Debug.Log(foodSprite);

        
        AddMenu();
       
    }
    void OnEnable()
    {
         destoryFoodMenuTimer= Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null);
        
    }
        void AddMenu()
    {
        
        if (destoryFoodMenuTimer.isDone)
            {
            MenuManage.menuManage.currentMenuNum -= 1;
            //Debug.Log(MenuManage.menuManage.currentMenuNum);
            //string foodSprite = levelFood[MenuManage.menuManage.menuList[]].normalUI;
            //food.sprite = Resources.Load<Sprite>(foodSprite);
            destoryFoodMenuTimer.Cancel();
            ObjectPool.instance.RecycleObj(gameObject);
            }
        
        }
    }

