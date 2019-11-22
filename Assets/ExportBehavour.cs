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
using System.Collections.Generic;

public class ExportBehavour : MonoBehaviourPunCallbacks {

    private float createPlate = 3;
    private float timer = 1f;
    private Transform createPlatePoint;
    private void Start()
    {
        createPlatePoint = GameObject.FindWithTag("PlateReturn").transform;

    }
    //检测菜


    //检测菜单


    //对比出菜
    /// <summary>
    /// 检测菜 与菜单相同 发放金币
    /// </summary>
    private void CheckGreens(GameObject plateObj)
    {
        greensName = new List<string>();
        Debug.Log(MenuManage.menuManage.menuList.Count+" 菜单列表");

        if (plateObj.transform.childCount > 0)
        {
            for (int m = 0; m < plateObj.transform.childCount; m++)
            {
                greensName.Add(plateObj.transform.GetChild(m).GetComponent<FoodIngredient>().GetIType().ToString());
            }
        }
        for (int i = 0; i < MenuManage.menuManage.menuList.Count; i++)
        {
            Debug.Log(3);
            bool isthisFood = true;
            Debug.Log(4);
            
            List<FoodModel_FoodIngredient> temp = LevelInstance._instance.levelFood[MenuManage.menuManage.menuList[i]].foodIngredient;
            Debug.Log(5);
          ///  List<string> foodsInPlate = plateObj.GetComponent<PlateBehaviour>().GetFoodList();
          //  Debug.Log(temp.Count + " 临时变量" +
            //    "");
            for (int j = 0; j < temp.Count; j++)
            {
                Debug.Log(1);
                // 如果不在盘子里，跳出
                if (!greensName.Contains(temp[j].name))
                {
                    Debug.Log("不在盘子 跳出");
                    isthisFood = false;
                    break;
                }
            }

            // 如果是这个菜，上菜，将菜单中的i销毁
            if (isthisFood)
            {
                Debug.Log("是这个菜");
                GameObject.FindGameObjectWithTag("Canvas").
                    GetComponent<GameCanvasController>().coinMenu.GetIcon((int)LevelInstance._instance.levelFood[MenuManage.menuManage.menuList[i]].price);
                MenuManage.menuManage.addMenu.foodMenu[i].GetComponent<MenuUI>().photonView.RPC("RemoveUI", RpcTarget.All, i);
                break;
            }
        }


    }



    /// <summary>
    /// 菜名
    /// </summary>
    private List<string> greensName;

    /// <summary>
    /// 检测盘子
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Plate")&&!other.transform.parent.name.Contains("Hand"))
        {
            CheckGreens(other.gameObject);
            Debug.Log(2);
            if (other.GetComponentsInChildren<Transform>().Length>1)
            {
                for (int i = 0; i < other.transform.childCount; i++)
                {
                    Debug.Log(3);
                    Destroy(other.transform.GetChild(i).gameObject);
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
