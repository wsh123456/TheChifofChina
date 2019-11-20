#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):SelectPlayerRole.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):SelectPlayerRole
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SelectPlayerRole : MonoBehaviour {

    public static SelectPlayerRole ins;

    private Transform mesh;
    private List<GameObject> headSkin;
    [Header("换头")]
    public int index;

    private void Awake()
    {
        ins = this;
        headSkin = new List<GameObject>();
        mesh = transform.GetChild(1).GetChild(0).GetChild(0);
        for (int i = 1; i <= 7; i++)
        {
            headSkin.Add(mesh.GetChild(i).gameObject);
        }
    }

    public void LastRole()
    {
        // index = --index % headSkin.Count;
        // FindSkin(index);
        // SelectRoleController.instance.SetPlayerIndex(index);
        // PlayerController.playerController.ChangeChiefHead(index);
    }

    public void NextRole()
    {
        // index = ++index % headSkin.Count;
        // FindSkin(index);
        // SelectRoleController.instance.SetPlayerIndex(index);
        // PlayerController.playerController.ChangeChiefHead(index);
    }

    /// <summary>
    /// 更换英雄皮肤英雄
    /// </summary>
    private void FindSkin(int index)
    {
        for (int i = 0; i < headSkin.Count; i++)
        {
            headSkin[i].SetActive(index == i);
        }
    }

}