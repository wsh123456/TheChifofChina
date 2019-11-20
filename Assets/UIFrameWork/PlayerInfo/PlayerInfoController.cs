#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlayerInfoController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlayerInfoController
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

public class PlayerInfoController : UIControllerBase {

    public static PlayerInfoController ins;
    private Player currentPlayer;

    private void Awake()
    {
        ins = this;
    }

    public override void ModuleInit()
    {
        // module.FindWidget("#NotReadyButton").AddOnClickListioner(OnReadyBtnClick);
    }

    // private void OnReadyBtnClick()
    // {
    //     Hashtable hashtable = new Hashtable();
    //     hashtable.Add("IsReady", true);
    //     PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    //     module.FindWidget("#NotReadyButton").SetObjectActive(false);
    // }

    public void SetChefHead(int index){
        module.FindWidget("#PlayerModel").transform.GetChild(0).GetComponent<PlayerController>().ChangeChiefHead(index);
    }

    public void PlayerInit(Player player)
    {
        currentPlayer = player;
        module.FindWidget("#PlayerName").SetTextText(player.NickName);
    }

    public Transform GetPlayerPoint(){
        return module.FindWidget("#PlayerModel").transform;
    }
}