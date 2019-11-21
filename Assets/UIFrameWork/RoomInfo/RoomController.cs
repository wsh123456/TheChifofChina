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
        module.FindWidget("#JoinButton").AddOnClickListioner(OnJoinBtnClick);
    }


    private void OnJoinBtnClick()
    {
        Debug.Log("加入房间：" + currentRoomInfo.Name);
        PhotonNetwork.JoinRoom(currentRoomInfo.Name);
        CanvasController.Instance.ShowModule("PlayerPanel");
    }


    public void SetRoomMessage(RoomInfo info){
        currentRoomInfo = info;
        module.FindWidget("#RoomName").SetTextText(currentRoomInfo.Name);
        module.FindWidget("#PeopleCount").SetTextText(currentRoomInfo.PlayerCount + "/" + currentRoomInfo.MaxPlayers);
    }
}