#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-7
// 修改者列表(modifier):无
// 模块描述(Module description):json数据模型
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 关卡数据模型
/// </summary>
public class LevelModel{
    public int level;
    public float levelTime;
    public float menuTimer;
    public float destoryFoodMenuTimer;
    public List<string> foodMenu = new List<string>();
    public List<FoodModel> foodModle = new List<FoodModel>();
}
public class FoodModel{
    public List<FoodModel_FoodIngredient> foodIngredient = new List<FoodModel_FoodIngredient>();
    public float price;
    public string normalUI;
}


public class FoodModel_FoodIngredient{
    public string name;
    public string state;
    
}