#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlaneBehaviour.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlaneBehaviour
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class PlaneBehaviour : MonoBehaviourPunCallbacks {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player"||other.tag=="Thing")
        {
            try
            {
                // other.GetComponent<PhotonView>().TransferOwnership(photonView.ViewID);
                photonView.RPC("SetParent", RpcTarget.All, other.transform.GetComponent<PhotonView>().ViewID, transform.GetComponent<PhotonView>().ViewID);
            }
            catch
            {
            }

        }
    }
    [PunRPC]
    private void SetParent(int index,int parentIndex)
    {
        PhotonView.Find(index).transform.parent = PhotonView.Find(parentIndex).transform.parent;
    }
}