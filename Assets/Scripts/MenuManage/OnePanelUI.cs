using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnePanelUI : MonoBehaviour {
    Timer destoryFoodMenuTimer;
    public Image food;
    public Image meterial;
    public Image mterialTwo;
    string foodSprite;
    string menuStr;
    string meterialStr;
    string meterialTwoStr;
    Dictionary<string, FoodModel> levelFood;
    Dictionary<string, FoodIngredientModel> levelIngredient;
    AddMenu addMenu;
    // Use this for initialization
    void Awake () {
        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        addMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<AddMenu>();
        menuStr = MenuManage.menuManage.menuStr;
        food = transform.Find("FoodPoint/MenuFood/food").GetComponent<Image>();
        meterial = transform.Find("materialPoint/mterial/materialPannel/material").GetComponent<Image>();

        mterialTwo = transform.Find("materialPoint/mterialTwo/materialPannel/material").GetComponent<Image>();
        
        meterialStr = levelFood[menuStr].foodIngredient[0].name;
        meterialTwoStr= levelFood[menuStr].foodIngredient[1].name;
        //Debug.Log("dsyadgiuasfhyiudshfiudshfuidshfuidshfuidsfgyru");
        //Debug.Log(levelFood["Chips"].foodIngredient[1].name+"auisdhusafhdsfhudsfydsugfydsufhudsihfudshf");
        foodSprite = levelFood[menuStr].normalUI;
        food.sprite = Resources.Load<Sprite>(foodSprite);
        meterial.sprite = Resources.Load<Sprite>(levelIngredient[meterialStr].normalUI);
        mterialTwo.sprite = Resources.Load<Sprite>(levelIngredient[meterialTwoStr].normalUI);

        //Debug.Log("double : " + menuStr);
    }
	
	// Update is called once per frame
	void Update () {
        
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
    void OnEnable()
    {
        destoryFoodMenuTimer = Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null);
        
    }
}
