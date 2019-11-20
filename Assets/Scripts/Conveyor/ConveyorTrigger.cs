#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):StartTrigger.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):StartTrigger
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Photon.Pun;
public class ConveyorTrigger : MonoBehaviourPunCallbacks {

    public Transform Aim;
    public Transform End;
    public float speed=0.5f;
    private Vector3 dir;
    private void Start()
    {
         dir =   End.position- Aim.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Thing"&&other.transform.parent.name!="Hand")
        {
            photonView.RPC("SetParent", RpcTarget.All, other.transform.GetComponent<PhotonView>().ViewID, transform.GetComponent<PhotonView>().ViewID);

        }
    }
    [PunRPC]
    private void SetParent(int index, int parentIndex)
    {
        PhotonView.Find(index).transform.parent = PhotonView.Find(parentIndex).transform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Thing")
        {
            
                other.GetComponent<Rigidbody>().velocity = dir * speed;

        }
        if (other.tag=="Player")
        {
           // other.GetComponent<Rigidbody>().mass = 0f;
           // other.GetComponent<Rigidbody>().velocity=dir* speed;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
       
    }
}