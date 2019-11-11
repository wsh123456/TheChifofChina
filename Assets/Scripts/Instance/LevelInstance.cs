#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-7
// 修改者列表(modifier):无
// 模块描述(Module description):关卡信息单例
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInstance{
    public float levelTime;
    public float menuTimer;
    public float destoryFoodMenuTimer;

    public Dictionary<string,FoodModel> levelFood;
    public Dictionary<string,FoodIngredientModel> levelIngredient;

    public static readonly LevelInstance _instance = new LevelInstance();
    private LevelInstance(){
        levelFood = new Dictionary<string, FoodModel>();
        levelIngredient = new Dictionary<string, FoodIngredientModel>();
        LoadLevel(1);
    }

    public void LoadLevel(int level){
        LevelModel message = JsonFileControl.LoadLevelMessage(1);
        levelTime = message.levelTime;
        menuTimer = message.menuTimer;
        destoryFoodMenuTimer = message.destoryFoodMenuTimer;
         Debug.Log("aaaa, " + levelTime + ", " + message.foodMenu.Count+"|"+destoryFoodMenuTimer);
        // 加载本关菜单

        levelFood = LoadFood(message.foodMenu);
        //Debug.Log(levelFood);
    }

    public Dictionary<string,FoodModel> LoadFood(List<string> foodMenu){

        Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
        List<FoodModel>levelFoodA=JsonFileControl.LoadFood(foodMenu);
        for (int i = 0; i < foodMenu.Count; i++)
        {
            levelFood.Add(foodMenu[i], levelFoodA[i]);
        }
        return levelFood;
    }
}
