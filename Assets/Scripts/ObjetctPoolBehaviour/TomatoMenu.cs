#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):TomatoMenu.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):TomatoMenu
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System;

public enum ThingType{
   
    Potato,
    Tomato,
    Drumstick,
    Cabbage
}
public class TomatoMenu : MonoBehaviour {

   public ThingType type;
    private void OnEnable()
    {
        //if((ThingType)Enum.Parse(typeof(ThingType), gameObject.name)==ThingType.Tomato)
        //{
        //    type = ThingType.Tomato;
        //   // Debug.Log(type);
        //}
        //if ((ThingType)Enum.Parse(typeof(ThingType), gameObject.name) == ThingType.Potato)
        //{
        //    type = ThingType.Potato;
        //}
        //if ((ThingType)Enum.Parse(typeof(ThingType), gameObject.name) == ThingType.Drumstick)
        //{
        //    type = ThingType.Drumstick;
        //}
        //if ((ThingType)Enum.Parse(typeof(ThingType), gameObject.name) == ThingType.Cabbage)
        //{
        //    type = ThingType.Cabbage;
        //}
        
    }
    private void Start()
    {
      //  gameObject.AddComponent<FoodIngredient>();
    }
    private void OnDestroy()
    {
        ObjectPool.instance.RecycleObj(gameObject);
    }


}