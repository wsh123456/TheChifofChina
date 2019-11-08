#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):FoodBase.cs
// 公司(Company):#COMPANY#
// 作者(Author): 王舜华
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-7
// 修改者列表(modifier):无
// 模块描述(Module description): 食材基类
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodIngredient : MonoBehaviour {
    /// <summary>
    /// 食材当前的状态
    /// </summary>
    public FoodIngredientState curState = FoodIngredientState.Normal;
    /// <summary>
    /// 是否为熟食(煮过,炸过)
    /// </summary>
    public bool isCooked = false;
    /// <summary>
    /// 对应不同状态的网格
    /// </summary>
    private Dictionary<FoodIngredientState, GameObject> usingMesh;
    /// <summary>
    /// 当前的操作进度
    /// </summary>
    private float curProgress = 0;
    public float actionTime = 2;
    private bool isActive = false;  // 是否正在进行操作

    /// <summary>
    /// 初始化食材属性
    /// </summary>
    public void InitFoodIngredient(FoodIngredientModel food){

    }

    /// <summary>
    /// 打断当前的操作(煮，切等操作)
    /// </summary>
    public void BreakCurrentAction(){

    }

    
    /// <summary>
    /// 继续当前的操作
    /// </summary>
    public void ContinueCurrentAction(){
        StartCoroutine("ShowProgras");
    }


    IEnumerator ShowProgras(){
        while(curProgress < actionTime){
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}

/// <summary>
/// 食物的状态(食材)
/// </summary>
public enum FoodIngredientState
{
    Normal,     // 默认状态(原材料)
    Cut,        // 切块状态
    Fried,      // 炸状态
    Poach,      // 水煮状态
}

/// <summary>
/// 食材类型
/// </summary>
public enum FoodIngredientType
{
    Cabbage,Potato,Tomato,Chicken

}