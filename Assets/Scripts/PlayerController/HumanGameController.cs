#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):HmanGameController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):HmanGameController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class HumanGameController : MonoBehaviour {

    public static HumanGameController ins;
    private void Awake()
    {
        //开始同步连接
        PhotonNetwork.ConnectUsingSettings();
        ins = this;
        //设置网络同步速率
        PhotonNetwork.SendRate = 40;

    }

}