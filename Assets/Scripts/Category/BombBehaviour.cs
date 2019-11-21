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
        if (other.GetComponent<FoodIngredient>())
        {
            //是肉类可以
            if ( other.GetComponent<FoodIngredient>().curState == FoodIngredientState.Cut && Input.GetKeyDown(KeyCode.Space)&& other.GetComponent<FoodIngredient>().GetIType() == FoodIngredientType.Chicken)
            {
                //可以放
                CanPutIn(other.gameObject);
            }
            if (other.GetComponent<FoodIngredient>().curState == FoodIngredientState.Cut && Input.GetKeyDown(KeyCode.Space) && other.GetComponent<FoodIngredient>().GetIType() == FoodIngredientType.Potato)
            {
                //可以放
                CanPutIn(other.gameObject);
            }

            if (other.GetComponent<FoodIngredient>().GetIType() == FoodIngredientType.Chicken)
            {
                Debug.Log(other.GetComponent<FoodIngredient>().curState);
            }
        }

        //装盘
        InPlate(other);
    }
      //放入台子并且有东西被打断 之后
    private void Update()
    {
        
        if (transform.GetChild(0).childCount>0)
        {
            //有灶台可以烹饪  可以继续  不可以暂停
            if (transform.parent.name.Contains("CheckHearth") && isCanCook&&!isContinue)
            {
                Debug.Log("开始烹饪");
                Bomb(transform.GetChild(0).GetChild(0).gameObject);
                isContinue = true;
            }
            if (transform.parent.name!= "CheckHearth" && !isCanCook&&isContinue)
            {
                Debug.Log("暂停");
                photonView.RPC("BreakCurrentAction",RpcTarget.All);
            
                isContinue = false;
            }
            //if (isCheck)
           // {
           //     CheckPotIsRipe();
           // }
        }  
    }
}