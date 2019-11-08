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
    public float maxMenuNum;
    public int levelOneStarScore;
    public int levelTwoStarScore;
    public int levelThreeStarScore;
    public static readonly LevelInstance _instance = new LevelInstance();
    private LevelInstance(){}

    public void LoadLevel(int level){
        LevelModel message = JsonFileControl.LoadLevelMessage(level);
        levelTime = message.levelTime;
        Debug.Log("aaaa, " + levelTime + ", " + message.foodMenu.Count);
        // 加载本关菜单
        Debug.Log("adfwggsdffgdfsd" + LoadFoodIngredient(message.foodMenu));
    }

    public List<FoodModel> LoadFoodIngredient(List<string> foodMenu){
        return JsonFileControl.LoadFoodIngredient(foodMenu);
    }
}
