#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):BombBehaviour.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):BombBehaviour
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BombBehaviour : Category {

    //锅检测灶台 
    private void OnTriggerStay(Collider other)
    {
        isCanCook = CheckHearth(other.gameObject);
        Debug.Log(isCanCook);
        if (other.GetComponent<FoodIngredient>())
        {
            //是肉类可以
            if (other.GetComponent<FoodIngredient>().GetIType() == FoodIngredientType.Chicken && other.GetComponent<FoodIngredient>().curState == FoodIngredientState.Cut && Input.GetKeyDown(KeyCode.Space))
            {
                //可以放
                CanPutIn(other.gameObject);
            }
        }
        //锅拿走 不可以烹饪  可以暂停
        if (!isCanCook && isCanStop)
        {
            Debug.Log("暂停");
            StopCurrentAction(transform.GetChild(0).gameObject);
            isCanStop = false;
        }
        //继续烹饪
        if (isCanCook&& !isCanStop)
        {
            Debug.Log("继续烹饪");
        }
        //装盘
        if (other.gameObject.name == "Plate")
        {
            if (transform.GetChild(0))
            {

                if (transform.GetChild(0).GetComponent<FoodIngredient>().curState != FoodIngredientState.Normal && Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC("SetParent", RpcTarget.All, transform.GetChild(0).GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID);
                }
            }
        }
    }
      //放入台子并且有东西被打断 之后
    private void Update()
    {
       
        if (transform.childCount==1)
        {
            Debug.Log(isCanCook+ "    isCanCook");
            Debug.Log(isContinue + "    isContinue");
            Debug.Log(isCanCook + "     isStop");
            //有灶台可以烹饪  可以继续
            if (isCanCook&&isContinue)
            {
                //可以炸
                Bomb(transform.GetChild(0).gameObject);
                isContinue = false;
               
                isCanCook = true;
            }
            if (isCheck)
            {
                CheckPotIsRipe();
            }
        }  
    }
}