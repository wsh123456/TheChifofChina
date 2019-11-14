#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlayerController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlayerController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon;

public class PlayerController : MonoBehaviourPunCallbacks,IPunObservable {

    // private PhotonView photonView;
    public PlayerHandController handController;
    public PhotonView creatFIView;

    private int curHead = 0;
    public int CurHead{
        get{return curHead;}
        set{
            headList.GetChild(curHead).gameObject.SetActive(false);
            curHead = value;
            headList.GetChild(curHead).gameObject.SetActive(true);
        }
    }

    private Transform headList;

    private Transform parentTrans;
    // public Transform ParentTrans{
    //     get{return parentTrans;}
    //     set{
    //         parentTrans = value;
    //         transform.SetParent(value);
    //     }
    // }

    private void Awake() {
        headList = transform.Find("Player 4/Chef/Mesh/Chef_Head");
        handController = transform.Find("Player 4").GetComponent<PlayerHandController>();
        // photonView = GetComponent<PhotonView>();
    }

    private void Update() {
        if(photonView.IsMine){
            if(Input.GetKeyDown(KeyCode.N)){
                ChangeChiefHead((curHead + 1) % (headList.childCount-1));
            }
        }
    }

    /// <summary>
    /// 更换厨师的头部显示模型
    /// </summary>
    public void ChangeChiefHead(int index){
        if(index >= headList.childCount){
            return;
        }
        CurHead = index;
    }

    [PunRPC]
    /// <summary>
    /// 设置父物体
    /// </summary>
    public void SetParent(int parentIndex){
        // ParentTrans = parent;
        transform.SetParent(GameManager._ins.initPoints[parentIndex].transform);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(CurHead);
            // stream.SendNext(ParentTrans);
        }else{
            CurHead = (int)stream.ReceiveNext();
            // ParentTrans = (Transform)stream.ReceiveNext();
        }
    }
}