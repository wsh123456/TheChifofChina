#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):MainController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):MainController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIFrameWork;
using Photon.Pun;
using Photon.Realtime;

public class MainController : UIControllerBase {

    public override void ModuleInit()
    {
        module.FindWidget("#SelectChefButton").AddOnClickListioner(OnSelectChefButtonClick);
        module.FindWidget("#CreateRoom").AddOnClickListioner(CreatRoom);
        module.FindWidget("#JoinRandomRoom").AddOnClickListioner(JoinRandomRoom);
        JoinLobby();
    }

    private void JoinLobby()
    {
        Debug.Log("加入大厅");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        CanvasController.Instance.ShowModule("RoomListPanel");
    }

    private void JoinRandomRoom()
    {
        InputPlayerInfoController.ins.CheckPlayerInput();
        PhotonNetwork.NickName = InputPlayerInfoController.ins.playerName;
        Debug.Log(PhotonNetwork.CountOfRooms);
        if(PhotonNetwork.CountOfRooms == 0){
            // 没有房间，创建房间
            CreatRoom();
        }else{
            if(!PhotonNetwork.InRoom)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            CanvasController.Instance.ShowModule("PlayerPanel");
        }
    }

    private void CreatRoom()
    {
        CanvasController.Instance.ShowModule("CreatRoomPanel");
    }

    private void OnSelectChefButtonClick()
    {
        CanvasController.Instance.ShowModule("SelectRolePanel");
    }
}
