#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):ConnectToMaster.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):ConnectToMaster
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToMaster : MonoBehaviourPunCallbacks {

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // public override void OnJoinedRoom()
    // {
    //     PlayerListController.ins.ShowPlayerList();
    // }

    // public override void OnLeftRoom()
    // {
    //     PlayerListController.ins.ShowPlayerList();
    // }

}