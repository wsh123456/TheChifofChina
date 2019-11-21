#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):ExportBehavour.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):ExportBehavour
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;

public class ExportBehavour : MonoBehaviour {


    //检测菜
        

    //检测菜单


    //对比出菜





    /// <summary>
    /// 检测盘子
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Plate")&&!other.transform.parent.name.Contains("Hand"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                Destroy(other.transform.GetChild(0).gameObject);
            }
            ObjectPool.instance.RecycleObj(other.gameObject);
            StartCoroutine("CreatePlate");
        }
    }
    /// <summary>
    /// 生成盘子
    /// </summary>
    /// <returns></returns>

    //private IEnumerator CreatePlate()
    //{
        
    //}
}