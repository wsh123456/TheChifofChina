#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlayerListController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlayerListController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIFrameWork;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListController : UIControllerBase {

    public static PlayerListController ins;

    private GameObject playerObjPrefab;
    private List<Player> players;
    private List<PlayerInfoController> playerObjs;

    private void Awake()
    {
        ins = this;
        playerObjPrefab = Resources.Load<GameObject>("PlayerInfo");
        players = new List<Player>();
        playerObjs = new List<PlayerInfoController>();
    }

    public override void ModuleInit()
    {
        module.FindWidget("#LeftRoomButton").AddOnClickListioner(OnLeftRoomBtnClick);
        if (module.FindWidget("#StartGameButton"))
        {
            module.FindWidget("#StartGameButton").AddOnClickListioner(OnStartGameBtnClick);
        }
    }

    private void OnLeftRoomBtnClick()
    {
        // this.gameObject.SetActive(false);
        //TODO:转移房主或者清空房间信息
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            CanvasController.Instance.ShowModule("RoomListPanel");
        }
    }

    public override void OnEnable(){
        base.OnEnable();
        ShowPlayerList();
    }

    public override void OnJoinedRoom(){
        ShowPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ShowPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ShowPlayerList();
    }

    public void ShowPlayerList()
    {
        ClearPlayerList();
        UpdatePlayerList();
        ShowStartGameButton();
    }

    private void ClearPlayerList()
    {
        for (int i = 0; i < playerObjs.Count; i++)
        {
            Destroy(playerObjs[i].gameObject);
        }
        playerObjs.Clear();
    }

    private void UpdatePlayerList()
    {
        object temp;
        if (PhotonNetwork.PlayerList.Length > 0)
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PlayerInfoController playerObjInfo = Instantiate(playerObjPrefab).GetComponent<PlayerInfoController>();
                if(PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("ChefHeadIndex", out temp)){
                    playerObjInfo.SetChefHead((int)temp);
                }else{
                    playerObjInfo.SetChefHead(0);
                }
                
                playerObjInfo.transform.SetParent(module.FindWidget("#PlayerList").transform);
                playerObjInfo.transform.localScale = Vector3.one;
                playerObjInfo.transform.localPosition = Vector3.zero;
                playerObjInfo.PlayerInit(PhotonNetwork.PlayerList[i]);
                playerObjs.Add(playerObjInfo);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        object temp;
        // ShowPlayerList();
        if(changedProps.TryGetValue("ChefHeadIndex", out temp)){
            ShowPlayerList();
        }
    }

    private void ShowStartGameButton()
    {
        if(module){
            module.FindWidget("#StartGameButton").SetObjectActive(PhotonNetwork.IsMasterClient);
        }
    }

    private void OnStartGameBtnClick()
    {
        // object ready;
        // for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        // {
        //     if (!PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("IsReady", out ready))
        //     {
        //         return;
        //     }
        // }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("LoadScene");
    }
}