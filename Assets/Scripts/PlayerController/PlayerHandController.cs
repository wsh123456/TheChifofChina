﻿#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):behandthings.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):behandthings
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using System;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHandController : MonoBehaviourPunCallbacks, IPunObservable
{

    private LevelInstance Levelins ;
    private GameObject knife;
    /// <summary>
    /// 食物种类
    /// </summary>
    // private FoodType foodType;
    private Animator ani;
    /// <summary>
    /// 手上是否有东西
    /// </summary>
    private bool isPick;
    private List<PickTings> handObj;
    /// <summary>
    /// 触发器碰到除物品外的所有东西
    /// </summary>
    private List<GameObject> allThings;
    private bool isCute;
    private PhotonView phView;
    private GameObject UserObj;
    private GameObject inHandObj;
    //手上容器
    public Transform handContainer;
    // public static PlayerHandController Instance;
    private CreateTingsManager thingManager;    // 生成东西


    private void Awake() {
        // Instance = this;
        thingManager = transform.parent.GetComponent<CreateTingsManager>();
        ani = transform.parent.GetComponent<Animator>();
        Levelins = LevelInstance._instance;
        menu = new Dictionary<string, List<string>>();
        handObj = new List<PickTings>();
        allThings = new List<GameObject>();
        // knife = transform.Find("Chef/Skeleton/Base/RightHand/Knife").gameObject;
    }

    private void Start()
    {
        knife = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).GetChild(0).gameObject;
        knife.SetActive(false);
        Menu();
        handContainer = transform.parent.Find("Hand");

    }


    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(GameObject other)
    {
        inHandObj = other;
        other.transform.parent = handContainer;
        other.transform.localPosition = Vector3.zero;
        other.transform.localEulerAngles = Vector3.zero;
        other.GetComponent<Rigidbody>().isKinematic = true;
        RemoveLight();
     
    }
    private void Update()
    {
        inHandObj = GetOnHand();

        if(!photonView.IsMine){
            return;
        }

        ani.SetBool("snackAttack", inHandObj != null);

        if(inHandObj != null){
            //扔
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if(inHandObj.GetComponent<IHand>().Throw(this, null)){
                    photonView.RPC("ThrowThings", RpcTarget.All);
                    // 同步扔
                    // Hashtable hashtable = new Hashtable();
                    // hashtable.Add("ThrowThingInHand", inHandObj.GetComponent<PhotonView>().ViewID);
                    // hashtable.Add("ThrowThingViewID", photonView.ViewID);
                    // PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                }
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //TODO.自己显示箭头
            }
            //放下
            // if (isPick && Input.GetKeyDown(KeyCode.Tab) && foodType.canDropDown)
            // {
            //     LayDownThings(handContainer.GetChild(0).gameObject);

            // }
        }
       
        if (ani.GetFloat("Walk") > 0)
        {
            isCute = false;
        }
        knife.SetActive(isCute);
        ani.SetBool("Cute", isCute);
    }




    private void OnTriggerStay(Collider other)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")|| ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            //拿东西
            if (other.tag=="Thing"&& Input.GetKeyDown(KeyCode.Space)&&!isPick)
            {
                PickUp(handObj[0].gameObject);
                RemoveAllLight();
            }
        }
        if (other.tag == "Plant")
        {
            string name = null;
            //取食材
            if (!isPick && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2 && other.name == "Chicken")
            {
                isPick = false;
                if (other.name.Contains("(Clone)"))
                {
                    name = other.name.Substring(0, other.name.LastIndexOf("("));
                }
                name = other.name;

                // 如果手上没东西
                if(inHandObj == null){
                    GameObject go = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient",handContainer.position);
                    // go.GetComponent<FoodIngredient>().InitFoodIngredient(LevelInstance._instance.levelIngredient[name]);
                    go.GetComponent<FoodIngredient>().photonView.RPC("InitFoodIngredient", RpcTarget.All, name);
                    go.GetComponent<FoodIngredient>().photonView.RPC("SetParent", RpcTarget.All, photonView.ViewID);
                    // Debug.Log("调用=======RPC");
                    // Hashtable hashtable = new Hashtable();
                    // hashtable.Add("CreatFoodIngredient", go.GetComponent<FoodIngredient>().photonView.ViewID);
                    // hashtable.Add("CreatFoodViewID", photonView.ViewID);
                    // PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
                }
                
            }
            //---------------切的状态 完成
            if (!isPick && other.name == "CuttingBoard" && Input.GetKeyDown(KeyCode.E) &&
                other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Cut, StopCut))
            {
                UserObj = other.transform.GetChild(0).GetChild(0).gameObject;
                isCute = true;
            }
            if (!isCute && ani.GetFloat("Walk") != 0)
            {
                if (UserObj != null)
                {
                    UserObj.GetComponent<FoodIngredient>().BreakCurrentAction();
                    UserObj = null;
                }
            }
            //放到台子上
            if (isPick && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2)
            {
                Debug.Log("放到台子上");
                handObj[0].gameObject.transform.parent = other.gameObject.transform.GetChild(0);
                handObj[0].gameObject.transform.localPosition = Vector3.zero;
                handObj[0].gameObject.transform.localEulerAngles = Vector3.zero;
            }
            if (other.gameObject.GetComponentsInChildren<Transform>().Length >= 3)
            {
                //物体是盘子
                if (isPick && Input.GetKeyDown(KeyCode.Space) && other.transform.GetChild(0).GetChild(0).name.Contains("Plate"))
                {

                    if (inHandObj == null)
                        return;
                    //状态不等于Normal
                    if (inHandObj.GetComponent<FoodIngredient>() == null)

                        return;

                    if (inHandObj.GetComponent<FoodIngredient>().curState != FoodIngredientState.Normal)
                    {
                        //检测重复
                        if (other.transform.GetChild(0).GetChild(0).childCount != 0)
                        {
                            //盘子中的食材
                            for (int i = 0; i < other.transform.GetChild(0).GetChild(0).childCount; i++)
                            {
                                if (inHandObj.GetComponent<FoodIngredient>().GetIType() == other.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.GetComponent<FoodIngredient>().GetIType())
                                {
                                    return;
                                }
                            }

                        }
                        //满足食谱条件
                        foreach (var item in menu)
                        {
                            if (item.Value.Contains(inHandObj.GetComponent<FoodIngredient>().GetIType().ToString()) && inHandObj.GetComponent<FoodIngredient>().DoAction(FoodIngredientState.InPlate, null))
                            {
                                inHandObj.transform.parent = other.transform.GetChild(0).GetChild(0);
                                inHandObj.transform.localPosition = Vector3.zero;
                                inHandObj.transform.localEulerAngles = Vector3.zero;
                                Destroy(inHandObj.transform.GetComponent<Rigidbody>());
                                inHandObj.tag = "Untagged";
                            }

                        }
                    }
                }
            }
        }

    }

    private void StopCut()
    {
        isCute = false;
    }

    /// <summary>
    /// 触碰飞来的游戏物体
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Plant")
        {
            allThings.Add(other.gameObject);
            allThings[0].AddComponent<HighlighterFlashing>();
            allThings[0].AddComponent<Highlighter>();
        }
        if (other.tag == "Thing" )
        {
            // FoodsType(other.gameObject);
           
           PickTings  pickTings=other.gameObject.AddComponent<PickTings>();
            handObj.Add(pickTings);
            handObj.Sort();
            if (!isPick)
            {
                handObj[0].gameObject.AddComponent<HighlighterFlashing>();
                handObj[0].gameObject.AddComponent<Highlighter>();
            }
            if (!isPick && other.GetComponent<Rigidbody>().velocity.z > 3 || other.GetComponent<Rigidbody>().velocity.z < -3)
            {
                PickUp(other.gameObject);
            }
        }
    }
    /// <summary>
    /// 移除灯光
    /// </summary>
    private void RemoveLight()
    {
        if (handObj.Count == 0)
            return;


        Destroy(handObj[0].GetComponent<HighlighterFlashing>());
        Destroy(handObj[0].GetComponent<Highlighter>());
    }
    private void RemoveAllLight()
    {
        for (int i = 0; i < handObj.Count; i++)
        {
            Destroy(handObj[i].GetComponent<HighlighterFlashing>());
            Destroy(handObj[i].GetComponent<Highlighter>());
        }
    }
    /// <summary>
    /// 物体离开清除脚本
    /// 从队列中移除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Thing")
        { 
            for (int i = 0; i < handObj.Count; i++)
            {
                if (other.gameObject==handObj[i].gameObject)
                {
                    if (i == 0)
                    {
                        RemoveLight();
                    }
                    Destroy(handObj[i].GetComponent<PickTings>());

                    handObj.Remove(handObj[i]);
                    return;
                }
            }

        }
        if (other.tag == "Plant")
        {
            Destroy(allThings[0].GetComponent<HighlighterFlashing>());
            Destroy(allThings[0].GetComponent<Highlighter>());
            allThings.Remove(allThings[0]);
        }
    }

    
    /// <summary>
    /// 放下东西
    /// </summary>
    private void LayDownThings(GameObject other)
    {
        other.GetComponent<Rigidbody>().isKinematic = false;
       
        other.transform.parent = GameObject.Find("CanPickUpThings").transform;
    }


    private Dictionary<string, List<string>> menu;
   
   //拿到菜谱 
    private void Menu()
    {
        foreach (var item in Levelins.levelFood)
        {
            List<string> CName = new List<string>();
            foreach (var i in item.Value.foodIngredient)
            {
                CName.Add(i.name);
            }
            menu.Add(item.Key, CName);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //切的动作
            stream.SendNext(isCute);
            
        }
        else
        {
            isCute = (bool)stream.ReceiveNext();
        }
    }


    /// <summary>
    /// 获取手上的东西，如果返回null则没有东西
    /// </summary>
    private GameObject GetOnHand(){
        try{
            handContainer.GetChild(0).GetComponent<IHand>();
            return handContainer.GetChild(0).gameObject;
        }catch{
            return null;
        }
    }


    #region photon同步调用  

    /// <summary>
    /// 生成新的食材(设置父物体)
    /// </summary>
    public void CreateFoodIngredient(GameObject ingredient){
        ingredient.tag = "Thing";
        ingredient.GetComponent<Rigidbody>().isKinematic = true;
        ingredient.transform.parent = handContainer;
        ingredient.transform.localPosition = Vector3.zero;

        return;
    }


    /// <summary>
    /// 扔东西
    /// </summary>
    [PunRPC]
    public void ThrowThings()
    {
        inHandObj.GetComponent<Rigidbody>().isKinematic = false;
        inHandObj.GetComponent<Rigidbody>().AddForce(transform.forward*500f);
        inHandObj.transform.parent=GameObject.Find("CanPickUpThings").transform;

        ani.SetTrigger("Push");
    }

    #endregion 
}

