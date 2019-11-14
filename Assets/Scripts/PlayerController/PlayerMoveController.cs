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
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMoveController :MonoBehaviourPunCallbacks,IPunObservable {
   
    private Animator ani;
    public static  PlayerMoveController instance;
    private PhotonView phView;
    private  void Awake()
    {
        instance = this;
        ani =GetComponent<Animator>();
        phView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (phView.IsMine)
        {
            HumanGameController.ins.currentPlayer = phView;
        }
    }
    private void Update()
    {
        if (!phView.IsMine)
            return;
        Move();
    }
    /// <summary>
    /// 玩家移动
    /// </summary>
    private void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        ani.SetFloat("Walk",Mathf.Abs(hor)+Mathf.Abs(ver)*100f-0.0000001f);
        transform.position +=new Vector3(hor,0,ver)*Time.deltaTime*4f;
        Vector3 dir = new Vector3(hor, 0, ver);
        
        if (dir!=Vector3.zero)
        {

            if (Vector3.Angle(new Vector3(hor, 0, ver), -transform.forward)>3)
            {
                //判断方向在人物的哪侧面
                if (Vector3.Cross(new Vector3(hor, 0, ver), -transform.forward).normalized==Vector3.down)
                {
                    transform.Rotate(0, 6f, 0);
                }
               else
                    transform.Rotate(0, -6f, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ani.SetTrigger("Run");
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 300f, ForceMode.Force);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }
}