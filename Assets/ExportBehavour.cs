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
using Photon.Pun;

public class ExportBehavour : MonoBehaviourPunCallbacks {

    private float createPlate = 3;
    private float timer = 1f;
    private Transform createPlatePoint;
    private void Start()
    {
        createPlatePoint = GameObject.FindWithTag("PlateReturn").transform;
        Debug.Log(Resources.Load<Material>("Prefabs/Materials/DirtyPlate"));
    }
    //检测菜


    //检测菜单


    //对比出菜
    /// <summary>
    /// 检测菜 与菜单相同 发放金币
    /// </summary>
    private void CheckGreens()
    {

    }

    /// <summary>
    /// 检测盘子
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Plate")&&!other.transform.parent.name.Contains("Hand"))
        {
            if (other.GetComponentsInChildren<Transform>().Length>1)
            {
                for (int i = 0; i < other.transform.childCount; i++)
                {
                    Destroy(other.transform.GetChild(0).gameObject);
                }
            }
            ObjectPool.instance.RecycleObj(other.gameObject);
            
            StartCoroutine("CreatePlate");
        }
    }
    /// <summary>
    /// 生成盘子
    /// </summary>
    /// <returns></returns>

    private IEnumerator CreatePlate()
    {
        yield return new  WaitForSeconds(timer);
      
        while (true)
        {
            createPlate -= timer;
            if (createPlate<0)
            {
                GameObject go = ObjectPool.instance.CreateObject("Plate","Prefabs/Plate", createPlatePoint.position);
                go.GetComponent<PhotonView>().TransferOwnership(photonView.ViewID);
                photonView.RPC("SetParents",RpcTarget.All, go.GetComponent<PhotonView>().ViewID,createPlatePoint.GetComponent<PhotonView>().ViewID);
                break;
            }
        }
    }
    [PunRPC]
    private void SetParents(int childIndex,int parentIndex)
    {
        PhotonView.Find(childIndex).transform.SetParent(PhotonView.Find(parentIndex).transform);
        PhotonView.Find(childIndex).transform.localPosition = Vector3.zero;
        PhotonView.Find(childIndex).GetComponent<MeshRenderer>().material=Resources.Load<Material>("Prefabs/Materials/DirtyPlate");
        PhotonView.Find(childIndex).GetComponent<PlateBehaviour>().Check(PlateBehaviour.PlateState.Dirty);
        PhotonView.Find(childIndex).GetComponent<Rigidbody>().isKinematic = true;
     }
}
