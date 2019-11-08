#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):behandthings.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):behandthings
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandController : MonoBehaviour {

   private Animator ani;
    /// <summary>
    /// 手上是否有东西
    /// </summary>
    private bool isPick;
    private List<PickTings> handObj;
    //private GameObject[] games;
    private void Start()
    {
        ani = transform.root.GetComponent<Animator>();
        handObj = new List<PickTings>();
    }
    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(GameObject  other)
    {
        other.transform.parent = transform;
        other.transform.localPosition = Vector3.zero;
        //端盘子
        ani.SetBool("snackAttack", true);
        other.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Update()
    {
        IsObjectInHand();
    }

    private void OnTriggerStay(Collider other)
    {
        //拿东西
        if (other.tag=="Thing"&& Input.GetKeyDown(KeyCode.Space)&&!isPick)
        {
            //
            PickUp(handObj[0].gameObject);
        }
        //仍
        if (isPick&&Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrowThings(other.gameObject);
        }
        if (isPick && Input.GetKey(KeyCode.LeftShift))
        {
            //TODO.显示箭头
        }
        //放下
        if (isPick&&Input.GetKeyDown(KeyCode.Tab))
        {
            LayDownThings(other.gameObject);
        }
    }
    /// <summary>
    /// 触碰飞来的游戏物体
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Thing" && other.GetComponent<Rigidbody>().velocity != Vector3.zero &&!isPick)
        {
            PickUp(other.gameObject);
        }
        if (other.tag=="Thing")
        {
           PickTings  pickTings=other.gameObject.AddComponent<PickTings>();
            handObj.Add(pickTings);
            handObj.Sort();
        }
    }
    /// <summary>
    /// 有东西在手上
    /// </summary>
    private void IsObjectInHand()
    {
        if (transform.childCount != 0)
        {
            isPick = true;
            handObj.Clear();
        }
        else
            isPick = false;
    }
    /// <summary>
    /// 扔东西
    /// </summary>
    private void ThrowThings(GameObject other)
    {
        other.GetComponent<Rigidbody>().isKinematic = false;
        other.GetComponent<Rigidbody>().AddForce(transform.forward*500f);
        ani.SetBool("snackAttack", false);
        other.transform.parent=GameObject.Find("CanPickUpThings").transform;
    }
    /// <summary>
    /// 放下东西
    /// </summary>
    private void LayDownThings(GameObject other)
    {
        other.GetComponent<Rigidbody>().isKinematic = false;
        ani.SetBool("snackAttack", false);
        other.transform.parent = GameObject.Find("CanPickUpThings").transform;
    }
    /// <summary>
    /// 判断手上东西的种类
    /// </summary>
    private bool InHandObjectType()
    {
        return false;
        //ToDo.........
    }
  
}