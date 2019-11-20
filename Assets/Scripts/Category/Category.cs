#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):Category.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):Category
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Category:MonoBehaviourPunCallbacks,IHand{
    /// <summary>
    /// 继续炸
    /// </summary>
    protected bool isContinue=false;
    protected bool isCanCook;
    protected bool isCanStop;

    private float overCookTime = 0;
    //  protected bool isCheck=false;
    /// <summary>
    /// 检测锅中食物是否成熟
    /// </summary>
    /// <param name="game"></param>
    //protected virtual void CheckPotIsRipe()
    //{
    //    if (transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().curState == FoodIngredientState.Poach || transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().curState == FoodIngredientState.Fried)
    //    {
    //        Debug.Log("回归初始化");
    //        isCanCook = false;
    //        isCheck = true;
    //        // photonView.RPC("ChangeFoodState",RpcTarget.All ,transform.GetChild(0).gameObject.GetComponent<FoodIngredient>().photonView.ViewID);
    //    }
    //}

    /// <summary>
    /// 装盘
    /// </summary>
    /// <param name=""></param> 
    protected virtual void InPlate(Collider other)
    {
        //装盘
        if (other.gameObject.name == "Plate")
        {
            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).GetComponent<FoodIngredient>().curState != FoodIngredientState.Normal && Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC("SetParent", RpcTarget.All, transform.GetChild(0).GetComponent<PhotonView>().ViewID, other.GetComponent<PhotonView>().ViewID);
                }
            }
        }
    }


    /// <summary>
    /// 炸
    /// </summary>
    /// <param name="锅里的东西"></param>
    protected virtual void Bomb(GameObject game)
    {

        photonView.RPC("DoActionFried", RpcTarget.All, game.GetComponent<PhotonView>().ViewID);

    }


    // 完成烹饪后的回调
    private void CookOver(){
        StartCoroutine("ReadyToFire");
    }

    // 过烹饪，计算起火
    private IEnumerator ReadyToFire(){
        while(overCookTime <= 10f){
            // 如果不在灶台上
            if(!CheckHearth(null)){
                break;
            }
            Debug.Log("过烹饪时间: " + overCookTime);
            yield return new WaitForFixedUpdate();
            overCookTime += Time.fixedDeltaTime;
        }
        if(overCookTime > 10f && CheckHearth(null)){
            // 获取下面的灶台(桌子)着火
            transform.GetComponentInParent<PlantController>().IsFire = true;
            overCookTime = 0;
        }
    }


    [PunRPC]
    protected virtual void DoActionFried(int index)
    {
       PhotonView.Find(index).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Fried, CookOver);
    }

    [PunRPC]
    protected virtual void DoActionPoach(int index)
    {
        PhotonView.Find(index).GetComponent<FoodIngredient>().DoAction(FoodIngredientState.Poach, CookOver);
    }
    //执行操作端锅终止操作
    protected virtual void StopCurrentAction(GameObject game)
    {
        Debug.Log("打断");
        photonView.RPC("BreakCurrentAction", RpcTarget.All);
        isContinue =false;
    }

    //煮
    protected virtual void Boil(GameObject game)
    {
        photonView.RPC("DOAction", RpcTarget.All, game.GetComponent<PhotonView>().ViewID);
    }
    /// <summary>
    /// 可以装盘
    /// </summary>
    /// 食物有碰撞体 食物与锅接触
    protected virtual void CanPutIn(GameObject inPot)
    {
        if (transform.childCount>1)
        {
            return;
        }
        
        photonView.RPC("SetParent",RpcTarget.All,inPot.GetComponent<PhotonView>().ViewID,transform.GetComponent<PhotonView>().ViewID);
    }
    //检测灶台
    protected virtual bool CheckHearth(GameObject game)
    {
        //if (game.name== "CheckHearth")
        //{
        //    if (game.GetComponentsInChildren<Transform>().Length == 1 &&Input.GetKeyDown(KeyCode.Space))
        //    {
        //        photonView.RPC("SetParent", RpcTarget.All, photonView.ViewID, game.GetComponent<PhotonView>().ViewID);
        //    }
        //}
        try
        {
            if (transform.parent.name.Contains("CheckHearth"))
            {
              //  Debug.Log("检测到了灶台");
                return true;
            }
            if (transform.parent.name == "Hand")
            {
                return false;
            }
            return false;
        }
        catch
        {
            return false;
        }

  
        

    }
    [PunRPC]
    public void BreakCurrentAction()
    {
            transform.GetChild(0).GetComponent<FoodIngredient>().BreakCurrentAction();
    }
    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="categoryIndex"></param>
    /// <param name="plantIndex"></param>
    /// 
    [PunRPC]
    protected virtual void SetParent(int categoryIndex,int plantIndex)
    {
        PhotonView.Find(categoryIndex).transform.SetParent(PhotonView.Find(plantIndex).transform);
        PhotonView.Find(categoryIndex).transform.localPosition = Vector3.zero;
        if (PhotonView.Find(categoryIndex).transform.parent==transform)
        {
            PhotonView.Find(categoryIndex).tag = "ThingChange";
           Destroy( PhotonView.Find(categoryIndex).GetComponent<Collider>());
        }
        if (PhotonView.Find(categoryIndex).GetComponent<Rigidbody>())
        {
            PhotonView.Find(categoryIndex).GetComponent<Rigidbody>().isKinematic = true;

        }
        if (PhotonView.Find(categoryIndex).transform.childCount!=0)
        {
            Destroy(PhotonView.Find(categoryIndex).GetComponent<PhotonRigidbodyView>());
            Destroy(PhotonView.Find(categoryIndex).GetComponent<Rigidbody>());
        }
    }

    public bool Pick(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }

    public bool Throw(PlayerHandController player, Action<GameObject> callback)
    {
        return false;
    }

    public bool PutDown(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }
}