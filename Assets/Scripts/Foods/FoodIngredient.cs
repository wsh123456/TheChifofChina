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
using System;

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
    private FoodIngredientModel foodIModel;
    private FoodIngredientMachine stateMachine;


    private void Awake() {
        stateMachine = new FoodIngredientMachine();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)){
            DoAction(FoodIngredientState.Cut);
        }
    }


    /// <summary>
    /// 初始化食材属性
    /// </summary>
    public void InitFoodIngredient(FoodIngredientModel food){
        foodIModel = food;
        stateMachine.SetState(this, FoodIngredientState.Normal);
    }

    /// <summary>
    /// 打断当前的操作(煮，切等操作)
    /// </summary>
    public void BreakCurrentAction(){
        StopCoroutine("ShowProgras");
    }

    
    /// <summary>
    /// 继续当前的操作
    /// </summary>
    public void DoCurrentAction(){
        if(!isActive){
            isActive = true;
            StartCoroutine("ChangingStatus");
        }
    }

    
    // public void StopChangingState(){}

    /// <summary>
    /// 做当前的操作
    /// </summary>
    public void DoAction(FoodIngredientState state){
        stateMachine.ChangeState(this, state);
    }

    /// <summary>
    /// 显示进度条
    /// </summary>
    public void ShowProgras(){
        Debug.Log("当前进度: " + curProgress / actionTime);
    }

    /// <summary>
    /// 隐藏进度条
    /// </summary>
    public void HideProgras(){
        Debug.Log("隐藏进度条");
    }

    // 转换状态
    private IEnumerator ChangingStatus(){
        Debug.Log(curProgress + ", " + actionTime);
        while(true){
            ShowProgras();
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            curProgress += Time.fixedDeltaTime;
            if(curProgress >= actionTime){
                break;
            }
        }
        Debug.Log("切换完成，用时 " + curProgress);
        stateMachine.FinishChange(this);
        HideProgras();
        curProgress = 0;
        isActive = false;
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
    Break       // 破坏状态
}

/// <summary>
/// 食材类型
/// </summary>
public enum FoodIngredientType
{
    Cabbage,Potato,Tomato,Chicken

}