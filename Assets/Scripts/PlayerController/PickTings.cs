#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PickTings.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PickTings
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System;

public class PickTings : MonoBehaviour, IComparable{
   // public int index;
   // public GameObject game;
   // public Vector3 gameV3;

    public int CompareTo(object obj)
    {
        PickTings other = obj as PickTings;
        if (transform.position.y > other.transform.position.y)
        {
            return -1;
        }
        else if (transform.position.y < other.transform.position.y)
        {
            return 1;
        }
        else
            return 0;
    }
}