using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //public List<Timer> MenuTimer;
    bool isDestory = true;
    bool isAddMenu = true;
    bool isMenuDone = false;
    bool isMenuAdd = true;
    void Awake()
    {
        menu = new List<string>();
        destoryFoodMenuTimer = TimerInstance.instance.destoryFoodMenuTimer;
        menuTimer = TimerInstance.instance.menuTimer;
        levelTimer = TimerInstance.instance.levelTimer;
        levelFood = LevelInstance._instance.levelFood;
        //foreach (KeyValuePair<string,FoodModel> item in levelFood)
        //{
        //    Debug.Log(item.Value.foodIngredient[0].state);
        //}
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
        menu = ForeachLevelFood(levelFood);
            if (TimerInstance.instance.menuTimer.isDone&& isMenuAdd)
            {
            Debug.Log(menu[RandomLevelFoodKey()]);
            isMenuAdd = false;
        }
    }
}

