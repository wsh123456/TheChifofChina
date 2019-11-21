#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):Co2TriggerCheck.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):Co2TriggerCheck
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;

public class Co2TriggerCheck : MonoBehaviour {

    private ExtinguisherBehaviour extinguisher;

    private void Awake() {
        extinguisher = transform.GetComponentInParent<ExtinguisherBehaviour>();
    }

    private void OnTriggerStay(Collider other){
        Debug.Log(other + "---------------------");
        // 如果灭火器在使用
        if(extinguisher.IsUse){
            try{
                other.GetComponent<PlantController>().IsFire = false;
            }catch{}
        }
    }

}