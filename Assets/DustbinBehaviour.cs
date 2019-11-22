#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):DustbinBehaviour.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):DustbinBehaviour
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;


public class DustbinBehaviour : MonoBehaviour {
   
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Thing")&&!other.transform.parent.name.Contains("Hand")&&!other.name.Contains("Frying"))
        {
            Debug.Log(other.name);
            other.transform.position = Vector3.zero;
            ObjectPool.instance.RecycleObj(other.gameObject);
        }
        
        if (other.tag.Contains("Thing") && other.transform.parent.name.Contains("Hand")&&other.name.Contains("Frying")&&other.transform.GetChild(0).GetChild(0)&&Input.GetKeyDown(KeyCode.Space))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(other.transform.GetChild(0).GetChild(0).gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Frying"))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}