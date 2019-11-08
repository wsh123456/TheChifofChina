#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-7
// 修改者列表(modifier):无
// 模块描述(Module description):食材的数据模型
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;

/// <summary>
/// 食材数据模型
/// </summary>
public class FoodIngredientModel{
    // 食材类型
    public FoodIngredientType foodIType;
    // 食材状态
    public FoodIngredientState curState = FoodIngredientState.Normal;   // 状态
    // 默认预制体
    public string normalPrefab;
    // 切预制体 切时间
    public string cutPrefab;
    public float cutTime;
    // 炸预制体 炸时间
    public string friedPrefab;
    public float friedTime;
    // 煮预制体 煮时间
    public string poachPrefab;
    public float poachTime;


    public FoodIngredientModel(){}
}


