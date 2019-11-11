using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManage : MonoBehaviour {
    public static readonly MenuManage menuManage = new MenuManage();
    private MenuManage() { }
    public int minMenuNum = 2;
    public int currentMenuNum = 0;
    public int maxMenuNum = 6;
    public Timer menuTimer;
    public Timer levelTimer;
    public Timer destoryFoodMenuTimer;
    public Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
    public List<string> menu;
    public Image food;
    Image meterial;
    Image cook;
    //public List<Timer> MenuTimer;
    bool isDestory = true;
    bool isAddMenu = true;
    bool isMenuDone = false;
    bool isMenuAdd = true;
    void Awake()
    {
        meterial = GameObject.Find("materialMenu/materialPannel/material").GetComponent<Image>();
        cook = GameObject.Find("materialMenu/cookPannel/cook").GetComponent<Image>();
        menu = new List<string>();
        destoryFoodMenuTimer = TimerInstance.instance.destoryFoodMenuTimer;
        menuTimer = TimerInstance.instance.menuTimer;
        levelTimer = TimerInstance.instance.levelTimer;
        levelFood = LevelInstance._instance.levelFood;
        food = GameObject.Find("MenuFood/MenuFood/food").GetComponent<Image>();
        //foreach (KeyValuePair<string,FoodModel> item in levelFood)
        //{
        //    Debug.Log(item.Value.foodIngredient[0].state);
        //}
    }
    void Start()
    {
        menu = ForeachLevelFood(levelFood);
    }
    void Update()
    {
        //Debug.Log(levelFood.Count);
        //Debug.Log(destoryFoodMenuTimer.GetTimeRemaining());
        //Debug.Log(levelTimer.GetRatioRemaining());
        //Debug.Log(menuTimer.GetTimeRemaining());
        //Debug.Log(levelFood);

        Initmenu();
        AddMenu();
        DestoryMenu();
        RandomMenu();


    }
    public void Initmenu()
    {
        if (currentMenuNum ==0&& isAddMenu)
        {
            currentMenuNum = 2;
            //Debug.Log(currentMenuNum);
            //Debug.Log("+2道菜");
            isAddMenu = false;   
        }
        
        
    }
    public void DestoryMenu()
    {
    if (destoryFoodMenuTimer.isDone && isDestory)
    {
            //Debug.Log("-1道菜");
    }
        isDestory = false;
        currentMenuNum -= 1;
        //Debug.Log("-1道菜");
        //Debug.Log(currentMenuNum);
    }
    public void CheckMenu()
    {

    }
   public void AddMenu()
    {
        if (menuTimer.isDone)
        { 
            if (!isMenuDone)
            {
                currentMenuNum += 2;
                //Debug.Log("+2道菜");
                //Debug.Log(currentMenuNum);
            }
            isMenuDone = true;
        }
    }
    public List<string> ForeachLevelFood(Dictionary<string, FoodModel> levelFood)
    {
        List<string> LevelFoodKey = new List<string>();
        foreach (KeyValuePair<string,FoodModel> item in levelFood)
        {
            LevelFoodKey.Add(item.Key);
        }
        return LevelFoodKey;
    }
    public int RandomLevelFoodKey()
    {
      int  random = Random.Range(0, 5);
        return random;
    }
    public void RandomMenu()
    {
        if (TimerInstance.instance.menuTimer.isDone && isMenuAdd)
        {
            Debug.Log(menu[RandomLevelFoodKey()]);
            string menuStr = menu[RandomLevelFoodKey()];
            string foodSprite = levelFood[menuStr].normalUI;
            food.sprite = Resources.Load<Sprite>(foodSprite);
            isMenuAdd = false;
        }
    }
}

