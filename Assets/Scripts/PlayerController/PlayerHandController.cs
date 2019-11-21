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
using System;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHandController : MonoBehaviourPunCallbacks, IPunObservable
{


    private GameObject knife;
    /// <summary>
    /// 食物种类
    /// </summary>
    // private FoodType foodType;
    private Animator ani;

    private List<GameObject> handObj;
    /// <summary>
    /// 触发器碰到除物品外的所有东西
    /// </summary>
    private List<GameObject> allThings;
    private bool isCute;
    private GameObject UserObj;
    private GameObject inHandObj;
    //手上容器
    public Transform handContainer;
    // public static PlayerHandController Instance;
    private CreateTingsManager thingManager;    // 生成东西
    private GameObject canPickUpThings;
    private bool isPut;

    private void Awake()
    {
        // Instance = this;
        thingManager = transform.parent.GetComponent<CreateTingsManager>();
        ani = transform.parent.GetComponent<Animator>();


        handObj = new List<GameObject>();
        allThings = new List<GameObject>();
        // knife = transform.Find("Chef/Skeleton/Base/RightHand/Knife").gameObject;
    }

    private void Start()
    {
       
        knife = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).GetChild(0).gameObject;
        knife.SetActive(false);
        handContainer = transform.parent.Find("Hand");
        canPickUpThings = handContainer.parent.parent.parent.gameObject;
    }

    [PunRPC]
    /// <summary>
    /// 东西端在手上
    /// </summary>
    /// <param name="other"></param>
    private void PickUp(int index)
    {

        Debug.Log(PhotonView.Find(index).name + "++");
        PhotonView.Find(index).GetComponent<PhotonView>().TransferOwnership(photonView.Owner);
       
        //Debug.Log(PhotonView.Find(index).transform.parent.GetComponentsInChildren<Transform>().Length);
        inHandObj = PhotonView.Find(index).gameObject;
        inHandObj.transform.parent = handContainer;
        inHandObj.transform.localPosition = Vector3.zero;
        inHandObj.transform.localEulerAngles = Vector3.zero;
        if (inHandObj.gameObject.name.Contains("Frying"))
        {
          
            if (!inHandObj.GetComponent<Rigidbody>())
            {
                inHandObj.AddComponent<Rigidbody>();
            }
            inHandObj.transform.localEulerAngles = new Vector3(0, 90, 0);
            inHandObj.transform.localPosition = new Vector3(0, -0.00479f, 0);
        }
        if (inHandObj.gameObject.name.Contains("FireExtinguisher"))
        {
            Debug.Log(PhotonView.Find(index).name);
            inHandObj.transform.localEulerAngles = new Vector3(0, 180, 0);
            inHandObj.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (inHandObj.GetComponent<Rigidbody>())
        {
            inHandObj.GetComponent<Rigidbody>().isKinematic = true;
        }
        Invoke("IsPut", 0.3f);
        if (PhotonView.Find(index).transform.GetComponentsInChildren<Transform>().Length >= 2 && !inHandObj.gameObject.name.Contains("FireExtinguisher"))
        {
            if (PhotonView.Find(index).transform.GetChild(0).name.Contains("Pro"))
                return;
            for (int i = 0; i < PhotonView.Find(index).transform.childCount; i++)
            {
                Debug.Log(PhotonView.Find(index).transform.childCount);
                PhotonView.Find(index).transform.GetChild(i).GetComponent<PhotonView>().TransferOwnership(photonView.Owner);
            }
        }
        Debug.Log(PhotonView.Find(index).name+ "拿");
       


    }

    private void IsPut()
    {
        isPut = true;
    }
    private void FixedUpdate()
    {
        
        inHandObj = GetOnHand();
        knife.SetActive(isCute);
        ani.SetBool("Cute", isCute);
       
        if (!photonView.IsMine)
        {
            return;
        }

        ani.SetBool("snackAttack", inHandObj != null);
        
        if (handObj.Count>0&&!handObj[0].activeSelf)
        {
            Debug.Log("aaa");
            handObj.Remove(handObj[0]);
        }
        if (inHandObj != null)
        {
            //扔
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (inHandObj.GetComponent<IHand>().Throw(this, null))
                {
                    // 同步扔
                    photonView.RPC("ThrowThings", RpcTarget.All);
                }
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //TODO.自己显示箭头
            }
            //放下
            if (inHandObj && Input.GetKeyDown(KeyCode.Tab) && inHandObj.GetComponent<IHand>().PutDown(this, null))
            {


                photonView.RPC("LayDownThings", RpcTarget.All, canPickUpThings.GetComponent<PhotonView>().ViewID, handContainer.GetChild(0).GetComponent<PhotonView>().ViewID);
            }
        }

        if (ani.GetFloat("Walk") > 0)
        {
            isCute = false;
        }
        for (int i = 0; i < handObj.Count; i++)
        {
            if (handObj[i])
            {
                if (!handObj[i].gameObject.GetComponent<Collider>())
                {
                    handObj.Remove(handObj[i]);
                   // Destroy(handObj[i].GetComponent<PickTings>());
                   
                }
            }
            
        }

        // ------使用喷火器
        if (inHandObj && inHandObj.name.Contains("FireExtinguisher") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("喷tmd");
            inHandObj.GetComponent<ExtinguisherBehaviour>().UseExtgui(true);
        }
        if (inHandObj && inHandObj.name.Contains("FireExtinguisher") && Input.GetKeyUp(KeyCode.E))
        {
            inHandObj.GetComponent<ExtinguisherBehaviour>().UseExtgui(false);
        }

    }




    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            //拿东西
            if (other.tag == "Thing" && Input.GetKeyDown(KeyCode.Space) && inHandObj == null)
            {
                Debug.Log("拿东西" + handObj[0].gameObject.name);
                photonView.RPC("PickUp", RpcTarget.All, handObj[0].gameObject.GetComponent<PhotonView>().ViewID);

               // RemoveAllLight();
            }
        }
        if (other.tag == "Plant")
        {

            string name = null;

            //取食材
            if (inHandObj == null && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2)
            {
                foreach (FoodIngredientType suit in Enum.GetValues(typeof(FoodIngredientType)))
                {
                    if (suit == (FoodIngredientType)Enum.Parse(typeof(FoodIngredientType), other.name))
                    {
                        if (other.name.Contains("(Clone)"))
                        {
                            name = other.name.Substring(0, other.name.LastIndexOf("("));
                        }
                        name = other.name;
                        // 如果手上没东西
                        if (inHandObj == null)
                        {
                            isPut = true;

                            GameObject go = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient", handContainer.position);
                            Debug.Log(go + "---------------"+ go.GetComponent<PhotonView>().ViewID);

                            go.GetComponent<FoodIngredient>().photonView.RPC("InitFoodIngredient", RpcTarget.All, name);
                            go.GetComponent<FoodIngredient>().photonView.RPC("SetParent", RpcTarget.All, photonView.ViewID);
                            
                           
                        }
                        break;
                    }
                }

            }


            //---------------切的状态 完成
            if (inHandObj == null && other.name.Contains("CuttingBoard") && Input.GetKeyDown(KeyCode.E))
            {
                // 如果子对象
                try
                {
                    int viewID = other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<FoodIngredient>().photonView.ViewID;
                    photonView.RPC("DoFoodCut", RpcTarget.All, viewID);
                }
                catch { }
            }
            if (!isCute && ani.GetFloat("Walk") != 0)
            {
                if (UserObj != null)
                {
                    photonView.RPC("BreakCurrentAction", RpcTarget.All);
                }
            }
            //放到台子上

            if (inHandObj != null && Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponentsInChildren<Transform>().Length == 2 && isPut == true)
            {
                try
                {
                  //  Debug.Log(other.name + "长度" + other.gameObject.GetComponentsInChildren<Transform>().Length);
                    int targetID = other.gameObject.transform.GetChild(0).GetComponent<PhotonView>().ViewID;
                    //放到台子上
                    Debug.Log("放到台子上");
                    photonView.RPC("PutOnTable", RpcTarget.All, targetID, handContainer.GetChild(0).GetComponent<PhotonView>().ViewID);
                    isPut = false;
                }
                catch
                {
                    return;
                }


            }

            if (other.transform.GetComponentsInChildren<Transform>().Length >= 3)
            {
                //物体是盘子
                if (other.transform.GetChild(0).GetChild(0).name.Contains("Plate") && inHandObj && Input.GetKeyDown(KeyCode.Space))
                {
                    if (inHandObj == null)
                        return;
                    if (!inHandObj.GetComponent<FoodIngredient>())
                        return;
                    Debug.Log(other.transform.GetChild(0).GetChild(0).name);
                    if (other.transform.GetChild(0).GetChild(0).GetComponent<PlateBehaviour>().CanInPlate(inHandObj.GetComponent<FoodIngredient>(), inHandObj))
                    {
                        Debug.Log("放入盘子");
                        photonView.RPC("SetParents",RpcTarget.All,inHandObj.GetComponent<PhotonView>().ViewID, other.transform.GetChild(0).GetChild(0).GetComponent<PhotonView>().ViewID);
                        //TODO。。。。。 改变物体贴图
                    }
                }

            }
        }

    }

    private void StopCut()
    {
        isCute = false;
    }

    /// <summary>
    /// 触碰飞来的游戏物体
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
      
        if (other.tag == "Plant")
        {
            Debug.Log(other.name);
            allThings.Add(other.gameObject);
          //  allThings[0].AddComponent<HighlighterFlashing>();
           // allThings[0].AddComponent<Highlighter>();
        }
        if (other.tag == "Thing")
        {
            if (!other.isTrigger)
            {
                // Debug.Log(other.name+"加脚本");
                //  PickTings pickTings = other.gameObject.AddComponent<PickTings>();
                Debug.Log("添加"+ other.gameObject);
                handObj.Add(other.gameObject);
               // handObj.Sort();
             //   if (inHandObj)
               // {
                   // handObj[0].gameObject.AddComponent<HighlighterFlashing>();
                   // handObj[0].gameObject.AddComponent<Highlighter>();
              //  }
                if (other.GetComponent<Rigidbody>())
                {
                    if (inHandObj == null && other.GetComponent<Rigidbody>().velocity.z > 3 || other.GetComponent<Rigidbody>().velocity.z < -3)
                    {

                        photonView.RPC("PickUp", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
                    }
                }

            }
        }
    }
    /// <summary>
    /// 物体离开清除脚本
    /// 从队列中移除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (other.tag == "Thing")
        {
            if (!other.isTrigger)
            {
                for (int i = 0; i < handObj.Count; i++)
                {
                  
                    if (other.gameObject == handObj[i])
                    {
                        handObj.Remove(other.gameObject);
                        Debug.Log("移除");
                        //if (i == 0)
                        //{
                        //    RemoveLight();
                        //}
                        //  Destroy(handObj[i].GetComponent<PickTings>());

                        //  Debug.Log("移除"+ handObj[i].name);
                        //   Debug.Log("剩下"+handObj[0].name);
                    }
                }
                //if (handObj.Count != 0)
                //{
                //    for (int i = 0; i < handObj.Count; i++)
                //    {
                //        if (handObj[i] == null)
                //        {
                //            // Destroy(handObj[i].GetComponent<PickTings>());
                //            return;
                //        }

                //    }

                //}
            }
        }   

        if (other.tag == "Plant")
        {
          //  Destroy(allThings[0].GetComponent<HighlighterFlashing>());
         //   Destroy(allThings[0].GetComponent<Highlighter>());
            allThings.Remove(allThings[0]);
        }
    }

    [PunRPC]
    /// <summary>
    /// 放下东西
    /// </summary>
    private void LayDownThings(int emptyIndex, int handThingIndex)
    {
        Debug.Log(PhotonView.Find(emptyIndex));
        PhotonView.Find(handThingIndex).GetComponent<Rigidbody>().isKinematic = false;
        PhotonView.Find(handThingIndex).transform.parent = PhotonView.Find(emptyIndex).transform;
       
    }

        


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //切的动作
            // stream.SendNext(isCute);

        }
        else
        {
            // isCute = (bool)stream.ReceiveNext();
        }
    }


    /// <summary>
    /// 获取手上的东西，如果返回null则没有东西
    /// </summary>
    private GameObject GetOnHand()
    {
        try
        {
            handContainer.GetChild(0).GetComponent<IHand>();
            return handContainer.GetChild(0).gameObject;
        }
        catch
        {
            return null;
        }
    }


    #region photon同步调用  
    [PunRPC]
    private void SetParents(int childIndex,int parentIndex)
    {

       PhotonView.Find(childIndex).transform.parent = PhotonView.Find(parentIndex).transform;
        PhotonView.Find(childIndex).transform.localPosition = Vector3.zero;
        PhotonView.Find(childIndex).transform.localEulerAngles = Vector3.zero;
        Destroy(PhotonView.Find(childIndex).transform.GetComponent<Rigidbody>());
        PhotonView.Find(childIndex).tag = "Untagged";
        //foodsList.Add(inPlateObj);
    }
    /// <summary>
    /// 扔东西
    /// </summary>
    [PunRPC]
    public void ThrowThings()
    {
        inHandObj.GetComponent<Rigidbody>().isKinematic = false;
        inHandObj.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        inHandObj.transform.parent =canPickUpThings.transform;
        ani.SetTrigger("Push");
    }


    /// <summary>
    /// 进行切的操作
    /// </summary>
    [PunRPC]
    public void DoFoodCut(int viewID)
    {
        if (PhotonView.Find(viewID).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Cut, StopCut))
        {
            UserObj = PhotonView.Find(viewID).gameObject;
            isCute = true;
        }
    }


    /// <summary>
    /// 把东西放到桌子上
    /// </summary>
    [PunRPC]
    public void PutOnTable(int targetID, int handID)
    {
        GameObject target = PhotonView.Find(targetID).gameObject;
        GameObject handObj1 = PhotonView.Find(handID).gameObject;
        handObj1.transform.SetParent(target.transform);
        handObj1.transform.localPosition = Vector3.zero;
        if (handObj1.name.Contains("Frying"))
        {
            Destroy(handObj1.GetComponent<Rigidbody>());
        }
        if (handObj1.name.Contains("Frying"))
        {
            handObj.Remove(handObj1.gameObject);
        }
        // handObj.transform.localEulerAngles = Vector3.zero;
        // handObj.GetComponent<Rigidbody>().isKinematic = false;

    }


    /// <summary>
    /// 打断当前动作
    /// </summary>
    [PunRPC]
    public void BreakCurrentAction()
    {
        if (UserObj)
        {
            isCute = false;
            UserObj.GetComponent<FoodIngredient>().BreakCurrentAction();
            UserObj = null;

        }
    }

}
    #endregion


