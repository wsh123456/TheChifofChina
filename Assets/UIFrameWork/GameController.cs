#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):GameController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):GameController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameController : MonoBehaviourPunCallbacks {

    public static GameController ins;

    private bool hasPlayerGenerate = false;
    private PhotonView currentPlayer;

    private void Awake()
    {
        ins = this;
        PhotonNetwork.SendRate = 40;
    }

    private void Start()
    {
        SetCurrentPlayerLoadLevel();
        NotificationGenerate();
    }

    private void SetCurrentPlayerLoadLevel()
    {
        Hashtable loadHash = new Hashtable();
        loadHash.Add("LoadLevel", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(loadHash);
    }

    private bool AllPlayersLoaded()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            object loaded = false;
            if (!PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("LoadLevel", out loaded))
            {
                return false;
            }
        }
        return false;
    }

    private void NotificationGenerate()
    {
        if (!hasPlayerGenerate && PhotonNetwork.IsMasterClient && AllPlayersLoaded())
        {
            Hashtable generatePlayerHash = new Hashtable();
            generatePlayerHash.Add("LoadSuccess", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(generatePlayerHash);
            hasPlayerGenerate = true;
        }
    }

    private void InitPlayers()
    {
        object index = 0;
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerIndex", out index))
        {
            index = 1;
        }
    }

}