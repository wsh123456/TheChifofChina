#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):InputPlayerInfoController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):InputPlayerInfoController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using Photon.Pun;
using Photon;
using Photon.Realtime;

public class InputPlayerInfoController : UIControllerBase {

    public static InputPlayerInfoController ins;

    private void Awake()
    {
        ins = this;
    }

    public string playerName;

    public override void ModuleInit()
    {
        module.FindWidget("#SurePlayerInfoButton").AddOnClickListioner(OnSureButtonClick);
    }

    private void OnSureButtonClick()
    {
        if(CheckPlayerInput()){
            CanvasController.Instance.FindAndSetActive("BG", true);
        }
    }

    public bool CheckPlayerInput()
    {
        playerName = module.FindWidget("#PlayerNameInputField").GetInputText();
        if (string.IsNullOrEmpty(playerName)){
            return false;
        }
        PhotonNetwork.NickName = playerName;
        return true;
    }
}