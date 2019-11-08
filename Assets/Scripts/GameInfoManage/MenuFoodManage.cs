using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MenuFoodManage
{
    public static readonly MenuFoodManage _instance = new MenuFoodManage();
    TimerContorller times;
    JsonFileControl json;
    public List<string> menuFood;
    AddMenu addMenu;
    void Awake()
    {
        times = new TimerContorller();
        menuFood = new List<string>();
        json = new JsonFileControl();
        addMenu = new AddMenu();
        //Add("Cabbage");
    }
    
    /// <summary>
    /// 添加菜谱
    /// </summary>
    /// <param name="value">菜</param>
    public void Add(Foods food)
    {
        //menuFood.Add(food);
    }
    public void AddCookBook()
    {
        //times.AddMenuP();
        
    }
    public void AddMenuP()
    {
        if (times.menuTimer.GetTimeRemaining() == 0)
        {
            addMenu.AddMenuPanel();
            foreach (var item in menuFood)
            {
                Debug.Log(item);
            }
            
            //JsonFileControl.LoadFoodIngredient();
        }
    }
}

