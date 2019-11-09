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

public class CreateTingsManager : MonoBehaviour {

   // public GameObject food;
    public void Awake()
    {
       // food = Resources.Load<GameObject>("Cube1");
        EventCenter.AddListener<int>(EventType.CreateTomaTo,CreateObject);
    }
    public void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventType.CreateTomaTo, CreateObject);
    }
    private void CreateObject(int arg)
    {
        if (arg==0)
        {
         GameObject go=ObjectPool.instance.CreateObject(AssetConst.Tomato);
            go.AddComponent<Rigidbody>();
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.transform.parent = PlayerHandController.Instance.transform;
            go.transform.localPosition = Vector3.zero;
        }
        //if (arg==1)
        //{
            //包材番茄炸鸡 土豆  生菜 

        //}
        //if (arg == 2)
        //{

        //}
        //if (arg == 3)
        //{

        //}
    }
}