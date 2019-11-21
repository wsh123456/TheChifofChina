#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):CreateRoomController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):CreateRoomController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomController : UIControllerBase
{
    public static CreateRoomController ins;

    private void Awake()
    {
        ins = this;
    }

    public string roomName;
    public string roomMaxPlayerCount;

    public override void ModuleInit()
    {
        module.FindWidget("#SureCreatButton").AddOnClickListioner(OnCreateRoomBtnClick);
        module.FindWidget("#BackToLobby").AddOnClickListioner(BackToLobby);
    }


    private void BackToLobby(){
        CanvasController.Instance.ShowModule("RoomListPanel");
    }


    public void OnCreateRoomBtnClick()
    {
        roomName = module.FindWidget("#RoomNameInputField").GetInputText();
        roomMaxPlayerCount = module.FindWidget("#RoomCountInputField").GetInputText();
        CheckPlayerInput();
        InputPlayerInfoController.ins.CheckPlayerInput();
        PhotonNetwork.NickName = InputPlayerInfoController.ins.playerName;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(roomMaxPlayerCount);
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        this.gameObject.SetActive(false);
        CanvasController.Instance.ShowModule("PlayerPanel");
    }

    public void CheckPlayerInput()
    {
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + Random.Range(1, 101);
        }
        if (string.IsNullOrEmpty(roomMaxPlayerCount))
        {
            roomMaxPlayerCount = "2";
        }
        try
        {
            int maxPlayers = int.Parse(roomMaxPlayerCount);
            //限制人数2-4
            roomMaxPlayerCount = Mathf.Clamp(maxPlayers, 2, 4).ToString();
        }
        catch
        {
            roomMaxPlayerCount = "2";
        }
    }

}