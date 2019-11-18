﻿#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):Category.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):Category
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Category:MonoBehaviourPunCallbacks{
    /// <summary>
    /// 继续炸
    /// </summary>
    protected bool isContinue=true;
    protected bool isCanCook;
    protected bool isCanStop;
    protected bool isCheck;
    /// <summary>
    /// 检测锅中食物是否成熟
    /// </summary>
    /// <param name="game"></param>
    protected virtual void CheckPotIsRipe()
    {
        if (transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().curState == FoodIngredientState.Poach || transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().curState == FoodIngredientState.Fried)
        {
            
            isCanCook = false;
            isCheck = true;
            // photonView.RPC("ChangeFoodState",RpcTarget.All ,transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().photonView.ViewID);
        }
    }
    /// <summary>
    /// 炸
    /// </summary>
    /// <param name="锅里的东西"></param>
    protected virtual void Bomb(GameObject game)
    {

        photonView.RPC("DoActionFried", RpcTarget.All, game.GetComponent<PhotonView>().ViewID);

    }

    [PunRPC]
    protected virtual void DoActionFried(int index)
    {
       PhotonView.Find(index).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Fried, null);
    }

    [PunRPC]
    protected virtual void DoActionPoach(int index)
    {
        PhotonView.Find(index).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Poach, null);
    }
    //执行操作端锅终止操作
 
    protected virtual void StopCurrentAction(GameObject game)
    {
        Debug.Log("打断");
        photonView.RPC("BreakCurrentAction", RpcTarget.All);
        isContinue =false;
    }

    //煮
    protected virtual void Boil(GameObject game)
    {
        photonView.RPC("DOAction", RpcTarget.All, game.GetComponent<PhotonView>().ViewID);
    }
    /// <summary>
    /// 可以装盘
    /// </summary>
    /// 食物有碰撞体 食物与锅接触
    protected virtual void CanPutIn(GameObject inPot)
    {
        if (transform.childCount>1)
        {
            return;
        }
        photonView.RPC("SetParent",RpcTarget.All,inPot.GetComponent<PhotonView>().ViewID, photonView.ViewID);
    }
    //检测灶台
    protected virtual bool CheckHearth(GameObject game)
    {
        //if (game.name== "CheckHearth")
        //{
        //    if (game.GetComponentsInChildren<Transform>().Length == 1 &&Input.GetKeyDown(KeyCode.Space))
        //    {
        //        photonView.RPC("SetParent", RpcTarget.All, photonView.ViewID, game.GetComponent<PhotonView>().ViewID);
        //    }
        //}
        try
        {
            if (transform.parent.name == "CheckHearth")
            {
                Debug.Log("检测到了灶台");
                return true;
            }
            if (transform.parent.name == "Hand")
            {
                return false;
            }
            return false;
        }
        catch
        {
            return false;
        }

  
        

    }
    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="categoryIndex"></param>
    /// <param name="plantIndex"></param>
    /// 
    [PunRPC]
    protected virtual void SetParent(int categoryIndex,int plantIndex)
    {
        PhotonView.Find(categoryIndex).transform.SetParent(PhotonView.Find(plantIndex).transform);
        PhotonView.Find(categoryIndex).transform.localPosition = Vector3.zero;
        PhotonView.Find(categoryIndex).GetComponent<Rigidbody>().isKinematic=true;
    }
}