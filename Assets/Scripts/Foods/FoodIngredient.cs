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
using UnityEngine.Events;
using UnityEngine.UI;

public class FoodIngredient : MonoBehaviour {
    /// <summary>
    /// 食材当前的状态
    /// </summary>
    public FoodIngredientState curState = FoodIngredientState.Normal;
    public FoodIngredientState previousState;
    /// <summary>
    /// 是否为熟食(煮过,炸过)
    /// </summary>
    public bool isCooked = false;
    /// <summary>
    /// 对应不同状态的网格
    /// </summary>
    private Dictionary<FoodIngredientState, string> usingMesh;
    private Dictionary<FoodIngredientState, float> usingTime;
    /// <summary>
    /// 当前的操作进度
    /// </summary>
    public float actionTime = 2;
    /// <summary>
    /// 当前使用模型
    /// </summary>
    public GameObject curMesh;

    private float curProgress = 0;  // 当前进度
    private GameObject progressBar; // 进度条
    private bool isActive = false;  // 是否正在进行操作
    private bool isFirstTime = true;    // 是否第一次进行操作
    /// <summary>
    /// 石材模型
    /// </summary>
    private FoodIngredientModel foodIModel;
    /// <summary>
    /// 食物状态机
    /// </summary>
    private FoodIngredientMachine stateMachine;
    private Action finishCallback;      // 完成切换状态时的回调函数


    private GameObject canvas;
    private Transform iconPoint;
    private Transform progressPoint;
    private Transform modelPoint;


    private void Awake() {
        usingMesh = new Dictionary<FoodIngredientState, string>();
        usingTime = new Dictionary<FoodIngredientState, float>();
        stateMachine = new FoodIngredientMachine();

        // 实例一个进度条
        progressBar = Instantiate(Resources.Load<GameObject>(UIConst.PROGRESS_BAR));
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        iconPoint = transform.Find("IconPoint").transform;
        progressPoint = transform.Find("ProgressPoint").transform;
        modelPoint = transform.Find("ModelPoint").transform;
    }

    private void Start() {

        progressBar.transform.SetParent(canvas.transform);
        progressBar.gameObject.SetActive(false);
    }

    private void Update() {
        //if(Input.GetKeyDown(KeyCode.Q)){
        //    DoAction(FoodIngredientState.Cut, null);
        //}

        //if(Input.GetKeyDown(KeyCode.W)){
        //    DoAction(FoodIngredientState.Fried, null);
        //}

        //if(Input.GetKeyDown(KeyCode.E)){
        //    DoAction(FoodIngredientState.InPlate, null);
        //}

        //if(Input.GetKeyDown(KeyCode.R)){
        //    DoAction(FoodIngredientState.Break, null);
        //}

        //if(Input.GetKeyDown(KeyCode.S)){
        //    BreakCurrentAction();
        //}

        progressBar.transform.position = Camera.main.WorldToScreenPoint(progressPoint.position);
    }


    /// <summary>
    /// 初始化食材属性并返回这个对象
    /// </summary>
    public FoodIngredient InitFoodIngredient(FoodIngredientModel food){
        foodIModel = food;
        //状态机设置状态
        stateMachine.SetState(this, food.curState);
        previousState = curState;

        SetUsingDict(food);     // 设置状态 网格和时间 字典
        // 加载当前状态的预制体，赋值

        ChangeCurMesh(food.curState);
        actionTime = 0;

        isCooked = false;
        curProgress = 0;
        isActive = false;
        isFirstTime = true;

        HideProgras();
        return this;
    }

    /// <summary>
    /// 打断当前的操作(煮，切等操作)
    /// </summary>
    public void BreakCurrentAction(){
        isActive = false;
        StopCoroutine("ChangingStatus");
    }

    
    /// <summary>
    /// 继续当前的操作
    /// </summary>
    public void DoCurrentAction(FoodIngredientState state){
        actionTime = usingTime[state];
        if(!isActive){
            isActive = true;
            StartCoroutine("ChangingStatus");
        }
    }
    /// <param name="state">要转换到的状态</param>
    /// <param name="finishCallback">完成时的回调函数</param>
    /// <returns>是否可以做这个操作</returns>
    public bool DoAction(FoodIngredientState state, Action finishCallback){
        this.finishCallback = finishCallback;
        return stateMachine.ChangeState(this, state);
    }

    /// <summary>
    /// 显示进度条
    /// </summary>
    public void ShowProgras(){
        //Debug.Log("当前进度: " + curProgress / actionTime);
        // 显示进度条
        progressBar.SetActive(true);
        progressBar.transform.Find("Slider").GetComponent<Slider>().value = curProgress / actionTime;
    }

    /// <summary>
    /// 隐藏进度条
    /// </summary>
    public void HideProgras(){

        progressBar.gameObject.SetActive(false);
    }
    public FoodIngredientType GetIType()
    {
        return foodIModel.foodIType;
    }

    // 转换状态
    private IEnumerator ChangingStatus(){

        Debug.Log(curProgress + ", " + actionTime);
        while(true){
            if(actionTime > 0){
                ShowProgras();
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            curProgress += Time.fixedDeltaTime;
            if(curProgress >= actionTime){
                break;
            }
        }
        Debug.Log("切换完成，用时 " + curProgress);
        stateMachine.FinishChange(ChangeCurMesh);
        HideProgras();
        curProgress = 0;
        isActive = false;
        if(finishCallback != null){
            Debug.Log("我有用");

            finishCallback();
        }
    }


    private void ChangeCurMesh(FoodIngredientState state){
        previousState = curState;
        curState = state;
        ObjectPool.instance.RecycleObj(curMesh);
        curMesh = ObjectPool.instance.CreateObject(foodIModel.foodIType.ToString() + state.ToString(), usingMesh[state],Vector3.zero);
        curMesh.transform.SetParent(modelPoint);
        curMesh.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="food"></param>
    private void SetUsingDict(FoodIngredientModel food){
        if(food.normalPrefab != null){
            usingMesh.Add(food.curState, food.normalPrefab);
            usingTime.Add(food.curState, 0);
        }
        if(food.cutPrefab != null){
            usingMesh.Add(FoodIngredientState.Cut, food.cutPrefab);
            usingTime.Add(FoodIngredientState.Cut, food.cutTime);
        }
        if(food.friedPrefab != null){
            usingMesh.Add(FoodIngredientState.Fried, food.friedPrefab);
            usingTime.Add(FoodIngredientState.Fried, food.friedTime);
        }
        if(food.poachPrefab != null){
            usingMesh.Add(FoodIngredientState.Poach, food.poachPrefab);
            usingTime.Add(FoodIngredientState.Poach, food.poachTime);
        }
        if(food.inPlate != null){
            usingMesh.Add(FoodIngredientState.InPlate, food.inPlate);
            usingTime.Add(FoodIngredientState.InPlate, 0);
        }
        if(food.breakPrefab != null){
            usingMesh.Add(FoodIngredientState.Break, food.breakPrefab);
            usingTime.Add(FoodIngredientState.Break, 0);
        }
    }
}

/// <summary>
/// 食物的状态(食材)
/// </summary>
public enum FoodIngredientState
{
    Normal,     // 默认状态(原材料)
    NormalCanPlate, // 默认可装盘
    Cut,        // 切块状态
    Fried,      // 炸状态
    Poach,      // 水煮状态
    InPlate,    // 装盘状态
    Break       // 破坏状态
}

/// <summary>
/// 食材类型
/// </summary>
public enum FoodIngredientType
{
    Cabbage,Potato,Tomato,Chicken
}