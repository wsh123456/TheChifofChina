#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):RoomListController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):RoomListController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIFrameWork;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListController : UIControllerBase {

    private List<RoomInfo> roomInfos;
    
    private List<RoomController> roomObjInfos;
    private GameObject roomObjPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        roomInfos = new List<RoomInfo>();
        roomObjInfos = new List<RoomController>();
        roomObjPrefab = Resources.Load<GameObject>("Prefabs/RoomInfo");
        ClearRoomObjInfos();
        UpdateRoomObjs();
    }

    private void ShowRoomInfos(List<RoomInfo> roomList)
    {
        roomInfos = roomList;
        ClearRoomObjInfos();
        UpdateRoomObjs();
    }

    private void ClearRoomObjInfos()
    {
        for (int i = 0; i < roomObjInfos.Count; i++)
        {
            Destroy(roomObjInfos[i].gameObject);
        }
        //清空List
        roomObjInfos.Clear();
    }

    private void UpdateRoomObjs()
    {
        for (int i = 0; i < roomInfos.Count; i++)
        {
            if(roomInfos[i].MaxPlayers == 0){
                continue;
            }
            RoomController roomObjInfo = Instantiate(roomObjPrefab).GetComponent<RoomController>();
            roomObjInfo.transform.SetParent(module.FindWidget("#RoomList").transform);
            roomObjInfo.SetRoomMessage(roomInfos[i]);
            // roomObjInfo.transform.localScale = Vector3.one;
            roomObjInfos.Add(roomObjInfo);
            roomObjInfo.currentRoomInfo = roomInfos[i];
            Debug.Log(roomInfos[i].Name);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ShowRoomInfos(roomList);
    }

    public override void OnLeftLobby()
    {
        ClearRoomObjInfos();
    }
}
