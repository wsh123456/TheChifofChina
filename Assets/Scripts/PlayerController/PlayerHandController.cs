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

    private LevelInstance Levelins = LevelInstance._instance;

    private GameObject knife;
    /// <summary>
    /// 食物种类
    /// </summary>
    private FoodType foodType;
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
    private bool isCute;

    private GameObject inHandObj;
    private void Start()
    {
        ani = transform.root.GetComponent<Animator>();
        handObj = new List<PickTings>();
        allThings = new List<GameObject>();
        knife = transform.parent.GetChild(3).GetChild(0).gameObject;
        knife.SetActive(false);
    }
    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(GameObject  other)
    {
        inHandObj = other;
        other.transform.parent = transform;
        other.transform.localPosition = Vector3.zero;
        other.transform.localEulerAngles = Vector3.zero;
        other.GetComponent<Rigidbody>().isKinematic = true;
        RemoveLight();
     
    }
    private void Update()
    {
        IsObjectInHand();
        ani.SetBool("snackAttack", isPick);
        //扔

        if (isPick && Input.GetKeyUp(KeyCode.LeftShift)&&foodType.canThrow)
        {
            ThrowThings(transform.GetChild(0).gameObject);
            ani.SetTrigger("Push");

        }
        if (isPick && Input.GetKey(KeyCode.LeftShift))
        {
            //TODO.显示箭头
        }
        //放下
        if (isPick && Input.GetKeyDown(KeyCode.Tab) && foodType.canDropDown)
        {
            LayDownThings(transform.GetChild(0).gameObject);

        }
       
        if (ani.GetFloat("Walk") > 0)
        {
            isCute = false;

        }
        knife.SetActive(isCute);
        ani.SetBool("Cute", isCute);
    }

    private void OnTriggerStay(Collider other)
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")|| ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            //拿东西
            if (other.tag=="Thing"&& Input.GetKeyDown(KeyCode.Space)&&!isPick)
            {
                FoodsType(other.gameObject);
                PickUp(handObj[0].gameObject);
                RemoveAllLight();
            }
           
        }
        if (other.tag == "Plant")
        {
            Debug.Log(other.gameObject.GetComponentsInChildren<Transform>().Length);
            //取食材
            if (!isPick&&Input.GetKeyDown(KeyCode.Space)&&other.gameObject.GetComponentsInChildren<Transform>().Length==2)
            {
                if (other.name.Contains("Tomato"))
                {
                    FoodsType(other.gameObject);
                    EventCenter.Broadcast<int>(EventType.CreateTomaTo,0);
                }
                if (other.name.Contains("Potato"))
                {
                    FoodsType(other.gameObject);
                    EventCenter.Broadcast<int>(EventType.CreateTomaTo, 1);
                }
            }
            //---------------切的状态 完成
            if(!isPick&&other.name=="CuttingBoard"&& Input.GetKeyDown(KeyCode.E)&&
                other.transform.Find("Plant").GetChild(0).gameObject.GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Cut,null))
            {
                isCute = true;
            }
            if (!isCute)
            {
                other.gameObject.GetComponent<FoodIngredient>().BreakCurrentAction();
            }
            //------------------！！！！！

            //放到台子上
            if (isPick && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2)
            {
                Debug.Log("放到台子上");
                handObj[0].gameObject.transform.parent = other.gameObject.transform.GetChild(0);
                handObj[0].gameObject.transform.localPosition = Vector3.zero;
                handObj[0].gameObject.transform.localEulerAngles = Vector3.zero;
            }
            if (other.transform.GetChild(0).GetChild(0) != null)
            {
                //物体是盘子

                if (isPick && Input.GetKeyDown(KeyCode.Space) && other.transform.GetChild(0).GetChild(0).name.Contains("Plate"))
                {
                    //TODO......
                    if (inHandObj == null)
                        return;
                    //状态不等于Normal
                    if (inHandObj.GetComponent<FoodIngredient>().curState != FoodIngredientState.Normal)
                    {
                        //检测重复
                        if (other.transform.GetChild(0).GetChild(0).childCount!=0)
                        {
                            //盘子中的食材
                            for (int i = 0; i < other.transform.GetChild(0).GetChild(0).childCount; i++)
                            {
                                if (inHandObj.GetComponent<FoodIngredient>().GetIType() == other.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.GetComponent<FoodIngredient>().GetIType())
                                {
                                    return;
                                }
                            }
                           
                        }
                        //满足食谱条件
                        if (true)
                        {

                        }
                    }
                }
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
        //if (other.name)
        //{

        //}
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
    /// <summary>
    /// 食物类型
    /// </summary>
    /// <param name="other"></param>
     private void FoodsType(GameObject other)
    {
        if (other.name.Contains("Cube"))
        {
            FoodType type = new FoodType(true, true, true,false);
            foodType = type;
        }
        if (other.name.Contains("Tomato"))
        {
            FoodType type = new FoodType(true,true,true,false);
            foodType = type;
        }
        if (other.name.Contains("Plate"))
        {
            FoodType type = new FoodType(true, true, false, false);
            foodType = type;
        }
        if (other.name.Contains("Extinguisher"))
        {
            Debug.Log("灭火器");
            FoodType type = new FoodType(true, true, false, false);
            foodType = type;
        }
    }
   //拿到菜谱 
    private void Menu()
    {
        foreach (var item in Levelins.levelFood)
        {
            
        }
       
    }
}

public class FoodType
{
    public bool canPick;
    public bool canDropDown;
    public bool canThrow;
    public bool canCut;
    public FoodType(bool canPick, bool canDropDown, bool canThrow, bool canCut)
    {
        this.canPick = canPick;
        this.canDropDown = canDropDown;
        this.canThrow = canThrow;
        this.canCut = canCut;
    }
}

