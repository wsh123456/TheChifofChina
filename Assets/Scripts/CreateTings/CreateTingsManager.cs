#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):CreateTingsManager.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):CreateTingsManager
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;

public class CreateTingsManager : MonoBehaviour, IPunObservable
{
    private int networkInt = 0;
    private Transform handContainer;
    public void Awake()
    {
        // EventCenter.AddListener<string>(EventType.CreateTomaTo,CreateObject);
        handContainer = transform.Find("Hand");
    }
    public void OnDestroy()
    {
        // EventCenter.RemoveListener<string>(EventType.CreateTomaTo, CreateObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }

    // public GameObject CreateObject(string arg)
    // {
    //     GameObject go=ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient",Vector3.zero);
    //     go.GetComponent<FoodIngredient>().InitFoodIngredient(LevelInstance._instance.levelIngredient[arg]);
    //     go.AddComponent<Rigidbody>();
    //     go.tag = "Thing";
    //     go.GetComponent<Rigidbody>().isKinematic = true;
    //     go.transform.parent = handContainer;
    //     go.transform.localPosition = Vector3.zero;

    //     return go;
    // }
}