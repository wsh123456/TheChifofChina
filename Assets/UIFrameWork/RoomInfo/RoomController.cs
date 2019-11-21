#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):RoomController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):RoomController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIFrameWork;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : UIControllerBase {

    public RoomInfo currentRoomInfo;

    public override void ModuleInit()
    {
        if (currentRoomInfo != null)
        {
            module.FindWidget("RoomName").SetTextText(currentRoomInfo.Name);
            module.FindWidget("Slots").SetTextText(currentRoomInfo.PlayerCount + "/" + currentRoomInfo.MaxPlayers);
            module.FindWidget("JoinButton").AddOnClickListioner(OnJoinBtnClick);
        }
    }

    private void OnJoinBtnClick()
    {
        PhotonNetwork.JoinRoom(currentRoomInfo.Name);
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRoom(currentRoomInfo.Name);
        }
    }
}