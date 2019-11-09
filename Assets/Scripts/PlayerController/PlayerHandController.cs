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
using HighlightingSystem;

public class PlayerHandController : Singleton<PlayerHandController> {

   private Animator ani;
    /// <summary>
    /// 手上是否有东西
    /// </summary>
    private bool isPick;
    private List<PickTings> handObj;
    /// <summary>
    /// 触发器碰到除物品外的所有东西
    /// </summary>
    private List<GameObject> allThings;
    private void Start()
    {
        ani = transform.root.GetComponent<Animator>();
        handObj = new List<PickTings>();
        allThings = new List<GameObject>();
        
    }
    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(GameObject  other)
    {
      
        other.transform.parent = transform;
        other.transform.localPosition = Vector3.zero;
        
        other.GetComponent<Rigidbody>().isKinematic = true;
        RemoveLight();
     
    }
    private void Update()
    {
        IsObjectInHand();
        ani.SetBool("snackAttack", isPick);
        //扔
        if (isPick && Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrowThings(transform.GetChild(0).gameObject);
            ani.SetTrigger("Push");

        }
        if (isPick && Input.GetKey(KeyCode.LeftShift))
        {
            //TODO.显示箭头
        }
        //放下
        if (isPick && Input.GetKeyDown(KeyCode.Tab))
        {
            LayDownThings(transform.GetChild(0).gameObject);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")|| ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            //拿东西
            if (other.tag=="Thing"&& Input.GetKeyDown(KeyCode.Space)&&!isPick)
            {
                PickUp(handObj[0].gameObject);

                RemoveAllLight();
            }
           
        }
        if (other.tag == "Plant")
        {
            //取食材
            if (!isPick&&Input.GetKeyDown(KeyCode.Space)&&other.gameObject.GetComponentsInChildren<Transform>().Length==2)
            {
                if (other.name.Contains("Tomato"))
                {
                    EventCenter.Broadcast<int>(EventType.CreateTomaTo,0);
                }
                
            }
            //放到台子上
            if (isPick && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2)
            {
                handObj[0].gameObject.transform.parent = other.gameObject.transform.GetChild(0);
                handObj[0].gameObject.transform.localPosition = Vector3.zero;
            }
        }
    }
    /// <summary>
    /// 触碰飞来的游戏物体
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Plant")
        {
            allThings.Add(other.gameObject);
            allThings[0].AddComponent<HighlighterFlashing>();
            allThings[0].AddComponent<Highlighter>();
        }
        if (other.tag == "Thing" )
        {
            if ( !isPick && other.GetComponent<Rigidbody>().velocity.z > 3 || other.GetComponent<Rigidbody>().velocity.z < -3)
            {
                PickUp(other.gameObject);
            }
           
        }
        if (other.tag=="Thing")
        {
           PickTings  pickTings=other.gameObject.AddComponent<PickTings>();
            handObj.Add(pickTings);
            handObj.Sort();
            if (!isPick)
            {
                handObj[0].gameObject.AddComponent<HighlighterFlashing>();
                handObj[0].gameObject.AddComponent<Highlighter>();
            }
        }
    }
    /// <summary>
    /// 移除灯光
    /// </summary>
    private void RemoveLight()
    {
        Destroy(handObj[0].GetComponent<HighlighterFlashing>());
        Destroy(handObj[0].GetComponent<Highlighter>());
    }
    private void RemoveAllLight()
    {
        for (int i = 0; i < handObj.Count; i++)
        {
            Destroy(handObj[i].GetComponent<HighlighterFlashing>());
            Destroy(handObj[i].GetComponent<Highlighter>());
        }
    }
    /// <summary>
    /// 物体离开清除脚本
    /// 从队列中移除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Thing")
        { 
            for (int i = 0; i < handObj.Count; i++)
            {
                if (other.gameObject==handObj[i].gameObject)
                {
                    if (i == 0)
                    {
                        RemoveLight();
                    }
                    Destroy(handObj[i].GetComponent<PickTings>());

                    handObj.Remove(handObj[i]);
                    return;
                }
            }

        }
        if (other.tag == "Plant")
        {
            Destroy(allThings[0].GetComponent<HighlighterFlashing>());
            Destroy(allThings[0].GetComponent<Highlighter>());
            allThings.Remove(allThings[0]);
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
       
        other.transform.parent=GameObject.Find("CanPickUpThings").transform;
    }
    /// <summary>
    /// 放下东西
    /// </summary>
    private void LayDownThings(GameObject other)
    {
        other.GetComponent<Rigidbody>().isKinematic = false;
       
        other.transform.parent = GameObject.Find("CanPickUpThings").transform;
    }
    /// <summary>
    /// 判断手上东西的种类
    /// </summary>
    private bool InHandObjectType(GameObject other)
    {
      FoodIngredient foodType   =  other.AddComponent<FoodIngredient>();
        return false;
        //ToDo.........
    }
  
}