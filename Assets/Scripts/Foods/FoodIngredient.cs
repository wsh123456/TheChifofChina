#region 模块信息
// **********************************************************************
// 文件名(File Name):FoodBase.cs
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
using Photon.Pun;

public class FoodIngredient : MonoBehaviourPunCallbacks,IPunObservable,IHand{
    /// <summary>
    /// 食材当前的状态
    /// </summary>
    public FoodIngredientState curState = FoodIngredientState.Normal;
    public FoodIngredientState previousState;//装盘之前的状态
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
    private bool isShowPrograss;

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
        if(photonView.IsMine){
            Debug.Log("aaaa");
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            DoAction(FoodIngredientState.Cut, null);
        }

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
    [PunRPC]
    // public FoodIngredient InitFoodIngredient(FoodIngredientModel food){
    public FoodIngredient InitFoodIngredient(string name){
        Debug.Log("进入InitFoodIngredient");
        FoodIngredientModel food = LevelInstance._instance.levelIngredient[name];
        foodIModel = food;
        //状态机设置状态
        stateMachine.SetState(this, food.curState);
        previousState = curState;

        SetUsingDict(food);     // 设置状态 网格和时间 字典
        // 加载当前状态的预制体，赋值

        // photonView.RPC("ChangeCurMesh", RpcTarget.MasterClient, (int)food.curState);
        ChangeCurMesh((int)food.curState);

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
        isShowPrograss = false;
        progressBar.SetActive(isShowPrograss);
        progressBar.transform.Find("Slider").GetComponent<Slider>().value = curProgress / actionTime;
    }

    /// <summary>
    /// 隐藏进度条
    /// </summary>
    public void HideProgras(){

        isShowPrograss = false;
        progressBar.gameObject.SetActive(isShowPrograss);

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


    #region RPC method

    [PunRPC]
    /// <summary>
    /// 设置父物体
    /// </summary>
    public void SetParent(int viewID){
        // ParentTrans = parent;
        Debug.Log(viewID + " qweqweqweqw");
        transform.tag = "Thing";
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(PhotonView.Find(viewID).GetComponent<PlayerHandController>().handContainer);
        transform.transform.localPosition = Vector3.zero;
    }
        
    #endregion



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //展示
            stream.SendNext(isShowPrograss);
            stream.SendNext(curProgress);
        }
        else
        {
            isShowPrograss = (bool)stream.ReceiveNext();
            curProgress = (float)stream.ReceiveNext();
        }
    }

    // [PunRPC]
    private void ChangeCurMesh(int target){
        Debug.Log("生成新的网格模型");
        if(target <= 2){
            isCooked = true;
        }
        FoodIngredientState state = (FoodIngredientState)target;
        previousState = curState;
        curState = state;
        ObjectPool.instance.RecycleObj(curMesh);
        // curMesh = ObjectPool.instance.CreateObject(foodIModel.foodIType.ToString() + state.ToString(), usingMesh[state],Vector3.zero);
        curMesh = ObjectPool.instance.CreateObjectLocal(foodIModel.foodIType.ToString() + state.ToString(), usingMesh[state], Vector3.zero);
        curMesh.transform.SetParent(modelPoint);
        curMesh.transform.localPosition = Vector3.zero;
    }

    
    // 设置使用的操作和时间
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

    #region Interface method
        
    // 捡
    public bool Pick(PlayerHandController player, Action<GameObject> callback){
        // callback(gameObject);
        return true;
    }

    // 扔
    public bool Throw(PlayerHandController player, Action<GameObject> callback){
        if(isCooked){
            return false;
        }

        // callback(gameObject);
        return true;
    }
    // 放
    public bool PutDown(PlayerHandController player, Action<GameObject> callback){
        callback(gameObject);
        return true;
    }


    #endregion


    
}

/// <summary>
/// 食物的状态(食材)
/// </summary>
public enum FoodIngredientState
{
    Normal=0,     // 默认状态(原材料)
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