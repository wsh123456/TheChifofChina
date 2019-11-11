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

public class PlayerMoveController : Singleton<PlayerMoveController> {

    private Animator ani;
    private Transform mesh;
    private GameObject knife;
    private List<GameObject> handSkin;
    [Header("换头")]
    public int index;
    private bool isCute;
    protected override void Awake()
    {
        ani = GetComponent<Animator>();
        handSkin = new List<GameObject>();
        mesh = transform.GetChild(0).GetChild(0).GetChild(0);
        for (int i = 1; i <=7; i++)
        {
            handSkin.Add(mesh.GetChild(i).gameObject);
        }
        knife =transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(3).Find("Knife").gameObject;
    }
    private void Start()
    {
        knife.SetActive(false);
    }
    private void Update()
    {
        Move();
        if (Input.GetMouseButtonDown(0))
        {
            index = ++index % handSkin.Count;
            FindSkin(index);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
           
            isCute = true;
          
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ani.SetTrigger("Run");
            transform.DOMove(transform.position - transform.forward * 3f, 0.5f);
        }
        if (ani.GetFloat("Walk")>0)
        {
            isCute = false;
        }
        knife.SetActive(isCute);
        ani.SetBool("Cute", isCute);

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
    }
   
    /// <summary>
    /// 更换英雄皮肤英雄
    /// </summary>
    private void FindSkin(int Index)
    {
        for (int i = 0; i < handSkin.Count; i++)
        {
            if (Index == i)
            {
                handSkin[i].SetActive(true);
              
            }
            else
                handSkin[i].SetActive(false);
        }
    }
}