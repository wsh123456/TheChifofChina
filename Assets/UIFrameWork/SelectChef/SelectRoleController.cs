#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):SelectRoleController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):SelectRoleController
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
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectRoleController : UIControllerBase {

    public static SelectRoleController instance;
    public GameObject chefPrefab;
    public GameObject chefObject;
    public int curHeadIndex;

    private void Awake()
    {
        instance = this;
        curHeadIndex = 0;

        chefPrefab = Resources.Load<GameObject>("ChefPlayerUI");
        chefObject = Instantiate(chefPrefab);
        ChangeChiefHead(0);
        chefObject.gameObject.SetActive(false);
    }

    public override void OnEnable(){
        base.OnEnable();
        // if(PhotonNetwork.InLobby){
        //     chefObject.SetActive(true);
        // }
        // if(PhotonNetwork.InRoom){
            chefObject.SetActive(true);
        // }
    }

    public override void OnDisable() {
        chefObject.SetActive(false);
        base.OnDisable();

        // 可能需要操作
    }

    

    public override void ModuleInit()
    {
        module.FindWidget("#NextRoleButton").AddOnClickListioner(ChangeChiefHead, 1);
        module.FindWidget("#LastRoleButton").AddOnClickListioner(ChangeChiefHead, -1);

        //module.FindWidget("#SureSelectButton").AddOnClickListioner(delegate ()
        //{
        //    this.SetPlayerIndex(SelectPlayerRole.ins.index);
        //});
        module.FindWidget("#SureSelectButton").AddOnClickListioner(HideThisPanel);

        chefObject.transform.SetParent(module.FindWidget("#PlayerPrefab").transform);
        chefObject.transform.localPosition = Vector3.zero;
        chefObject.transform.localScale = new Vector3(20000f,20000f,20000f);
    }

    private void HideThisPanel()
    {
        if(PhotonNetwork.InRoom){
            CanvasController.Instance.ShowModule("PlayerPanel");
        }else if(PhotonNetwork.InLobby){
            CanvasController.Instance.ShowModule("RoomListPanel");
        }else{
            Debug.LogError("无处可去。。。");
        }

    }

    // public void SetPlayerIndex(int playerIndex)
    // {
    //     Hashtable playerProperties = new Hashtable();
    //     playerProperties.Add("PlayerIndex", playerIndex);
    //     PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    // }

    // 切换厨师头, 传入1或-1
    private void ChangeChiefHead(int offset){
        curHeadIndex = (curHeadIndex + 7 + offset) % 7;
        chefObject.GetComponent<PlayerController>().ChangeChiefHead(curHeadIndex);

        Hashtable hashtable = new Hashtable();
        hashtable.Add("ChefHeadIndex", curHeadIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }


}
