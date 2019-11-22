#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):GameCanvasController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):GameCanvasController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon;
using Photon.Realtime;

public class GameCanvasController : MonoBehaviour {
    public Panel_CountDown countDown;
    public TimerContorller timer;
    public AddMenu foodMenu;
    public CoinContorller coinMenu;
    
    private void Awake() {
        countDown = transform.Find("CountDown").GetComponent<Panel_CountDown>();
        timer = transform.Find("Timer").GetComponent<TimerContorller>();
        foodMenu = transform.Find("MenuPoint").GetComponent<AddMenu>();

        coinMenu = transform.Find("Coin").GetComponent<CoinContorller>();  
    }

    public void StartCountDown(){
        countDown.StartCountDown();
    }

    public void StartGame(){
        // 时间开启
        timer.IsStart = true;
        // 菜单开启,由房主同步菜单信息
        if(PhotonNetwork.IsMasterClient){
            MenuManage.menuManage.StartGame();
        }
    }

}