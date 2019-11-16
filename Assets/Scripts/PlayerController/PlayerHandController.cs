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


    private GameObject knife;
    /// <summary>
    /// 食物种类
    /// </summary>
    // private FoodType foodType;
    private Animator ani;
 
    private List<PickTings> handObj;
    /// <summary>
    /// 触发器碰到除物品外的所有东西
    /// </summary>
    private List<GameObject> allThings;
    private bool isCute;
    private GameObject UserObj;
    private GameObject inHandObj;
    //手上容器
    public Transform handContainer;
    // public static PlayerHandController Instance;
    private CreateTingsManager thingManager;    // 生成东西
    private GameObject canPickUpThings;

    private void Awake()
    {
        // Instance = this;
        thingManager = transform.parent.GetComponent<CreateTingsManager>();
        ani = transform.parent.GetComponent<Animator>();


        handObj = new List<PickTings>();
        allThings = new List<GameObject>();
        // knife = transform.Find("Chef/Skeleton/Base/RightHand/Knife").gameObject;
    }

    private void Start()
    {
        canPickUpThings = GameObject.Find("CanPickUpThings");
        knife = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).GetChild(0).gameObject;
        knife.SetActive(false);

        handContainer = transform.parent.Find("Hand");

    }

    [PunRPC]
    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(int index)
    {
        Debug.Log("进入RPCPickUp");
        inHandObj = PhotonView.Find(index).gameObject;
        inHandObj.transform.parent = handContainer;
        inHandObj.transform.localPosition = Vector3.zero;
        inHandObj.transform.localEulerAngles = Vector3.zero;
        inHandObj.GetComponent<Rigidbody>().isKinematic = true;
        RemoveLight();

    }
    private void FixedUpdate() {
        inHandObj = GetOnHand();

        knife.SetActive(isCute);
        ani.SetBool("Cute", isCute);

        if (!photonView.IsMine)
        {
            return;
        }

        ani.SetBool("snackAttack", inHandObj != null);

        if (inHandObj != null)
        {
            //扔
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (inHandObj.GetComponent<IHand>().Throw(this, null))
                {
                    // 同步扔
                    photonView.RPC("ThrowThings", RpcTarget.All);
                }
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //TODO.自己显示箭头
            }
            //放下
            if (inHandObj && Input.GetKeyDown(KeyCode.Tab) && inHandObj.GetComponent<IHand>().PutDown(this, null))
            {
                // LayDownThings();
                photonView.RPC("LayDownThings", RpcTarget.All, canPickUpThings.GetComponent<PhotonView>().ViewID, handContainer.GetChild(0).GetComponent<PhotonView>().ViewID);
            }
        }

        if (ani.GetFloat("Walk") > 0)
        {
            isCute = false;
        }


    }




    private void OnTriggerStay(Collider other)
    {
        if(!photonView.IsMine){
            return;
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            //拿东西
            if (other.tag == "Thing" && Input.GetKeyDown(KeyCode.Space) &&inHandObj==null)
            {
                Debug.Log(handObj[0].gameObject.GetComponent<PhotonView>().ViewID + "++++++++");
                Debug.Log(handObj[0].gameObject.GetComponent<FoodIngredient>().photonView.ViewID + "---------");
                photonView.RPC("PickUp", RpcTarget.All, handObj[0].gameObject.GetComponent<PhotonView>().ViewID);
                Debug.Log("进入");
                RemoveAllLight();
            }
        }
        if (other.tag == "Plant")
        {
            string name = null;
            //取食材
            if (inHandObj==null && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2 && other.name == "Chicken")
            {
                if (other.name.Contains("(Clone)"))
                {
                    name = other.name.Substring(0, other.name.LastIndexOf("("));
                }
                name = other.name;

                // 如果手上没东西
                if (inHandObj == null)
                {
                    GameObject go = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient", handContainer.position);
                    go.GetComponent<FoodIngredient>().photonView.RPC("InitFoodIngredient", RpcTarget.All, name);
                    go.GetComponent<FoodIngredient>().photonView.RPC("SetParent", RpcTarget.All, photonView.ViewID);
                }

            }
            //---------------切的状态 完成
            if (inHandObj == null && other.name == "CuttingBoard" && Input.GetKeyDown(KeyCode.E))
            {
                // 如果子对象
                try
                {
                    int viewID = other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<FoodIngredient>().photonView.ViewID;
                    photonView.RPC("DoFoodCut", RpcTarget.All, viewID);
                }
                catch { }
            }
            if (!isCute && ani.GetFloat("Walk") != 0)
            {
                if (UserObj != null)
                {
                    photonView.RPC("BreakCurrentAction", RpcTarget.All);
                }
            }
            //放到台子上
            if (inHandObj != null && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2)
            {
                try{
                    int targetID = other.gameObject.transform.GetChild(0).GetComponent<PhotonView>().ViewID;
                    photonView.RPC("PutOnTable", RpcTarget.All, targetID, handContainer.GetChild(0).GetComponent<PhotonView>().ViewID);
                }catch{
                    return;
                }
               

            }


            if (other.gameObject.GetComponentsInChildren<Transform>().Length >= 3)
            {
                //物体是盘子
                if (inHandObj && Input.GetKeyDown(KeyCode.Space) && other.transform.GetChild(0).GetChild(0).name.Contains("Plate"))
                {

                    if (inHandObj == null)
                        return;
                    if (other.transform.GetChild(0).GetChild(0).GetComponent<PlateBehaviour>().CanInPlate(inHandObj.GetComponent<FoodIngredient>(), inHandObj))
                    {
                        Debug.Log("放入盘子");
                        //TODO。。。。。 改变物体贴图
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
        if(!photonView.IsMine){
            return;
        }
        if (other.tag == "Plant")
        {
            allThings.Add(other.gameObject);
            allThings[0].AddComponent<HighlighterFlashing>();
            allThings[0].AddComponent<Highlighter>();
        }
        if (other.tag == "Thing")
        {
            PickTings pickTings = other.gameObject.AddComponent<PickTings>();
            handObj.Add(pickTings);
            handObj.Sort();
            if (inHandObj)
            {
                handObj[0].gameObject.AddComponent<HighlighterFlashing>();
                handObj[0].gameObject.AddComponent<Highlighter>();
            }
            if (inHandObj == null && other.GetComponent<Rigidbody>().velocity.z > 3 || other.GetComponent<Rigidbody>().velocity.z < -3)
            {
               photonView.RPC("PickUp", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
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
        if(!photonView.IsMine){
            return;
        }

        if (other.tag == "Thing")
        {
            for (int i = 0; i < handObj.Count; i++)
            {
                if (handObj[i].gameObject == null)
                {
                    handObj.Remove(handObj[i]);
                    Destroy(handObj[i].GetComponent<PickTings>());
                }
                if (other.gameObject == handObj[i].gameObject)
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

    [PunRPC]








    /// <summary>
    /// 放下东西
    /// </summary>
    private void LayDownThings(int emptyIndex, int handThingIndex)
    {
        Debug.Log(PhotonView.Find(emptyIndex));
        PhotonView.Find(handThingIndex).GetComponent<Rigidbody>().isKinematic = false;

        PhotonView.Find(handThingIndex).transform.parent = PhotonView.Find(emptyIndex).transform;
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //切的动作
            // stream.SendNext(isCute);

        }
        else
        {
            // isCute = (bool)stream.ReceiveNext();
        }
    }


    /// <summary>
    /// 获取手上的东西，如果返回null则没有东西
    /// </summary>
    private GameObject GetOnHand()
    {
        try
        {
            handContainer.GetChild(0).GetComponent<IHand>();
            return handContainer.GetChild(0).gameObject;
        }
        catch
        {
            return null;
        }
    }


    #region photon同步调用  

    /// <summary>
    /// 生成新的食材(设置父物体)
    /// </summary>
    // public void CreateFoodIngredient(GameObject ingredient)
    // {
    //     ingredient.tag = "Thing";
    //     ingredient.GetComponent<Rigidbody>().isKinematic = true;
    //     ingredient.transform.parent = handContainer;
    //     ingredient.transform.localPosition = Vector3.zero;

    //     return;
    // }


    /// <summary>
    /// 扔东西
    /// </summary>
    [PunRPC]
    public void ThrowThings()
    {
        inHandObj.GetComponent<Rigidbody>().isKinematic = false;
        inHandObj.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        inHandObj.transform.parent = GameObject.Find("CanPickUpThings").transform;
        ani.SetTrigger("Push");
    }


    /// <summary>
    /// 进行切的操作
    /// </summary>
    [PunRPC]
    public void DoFoodCut(int viewID)
    {
        if (PhotonView.Find(viewID).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Cut, StopCut))
        {
            UserObj = PhotonView.Find(viewID).gameObject;
            isCute = true;
        }
    }


    /// <summary>
    /// 把东西放到桌子上
    /// </summary>
    [PunRPC]
    public void PutOnTable(int targetID, int handID)
    {
        GameObject target = PhotonView.Find(targetID).gameObject;
        GameObject handObj = PhotonView.Find(handID).gameObject;

        handObj.transform.SetParent(target.transform);
        handObj.transform.localPosition = Vector3.zero;
        handObj.transform.localEulerAngles = Vector3.zero;
        // handObj.GetComponent<Rigidbody>().isKinematic = false;
    }


    /// <summary>
    /// 打断当前动作
    /// </summary>
    [PunRPC]
    public void BreakCurrentAction(){
        if(UserObj){
            UserObj.GetComponent<FoodIngredient>().BreakCurrentAction();
            UserObj = null;
            isCute = false;
        }
    }

    #endregion 
}

