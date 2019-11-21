#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlantController.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlantController
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PlantController: MonoBehaviourPunCallbacks,IPunObservable{
    private bool isFire = false;
    private GameObject firePrefab;      // 火焰预制体
    private GameObject fire;
    private Transform canvas;

    private Transform setPoint;
    private Camera mainCamera;


    public bool IsFire{
        get{return isFire;}
        set{
            if(value){
                StartFire();
            }else{
                EndFire();
            }
            isFire = value;
        }
    }

    private void Awake() {
        mainCamera = Camera.main;
        setPoint = transform.GetChild(0);
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;

        fire = Instantiate(Resources.Load<GameObject>(UIConst.PROGRESS_BAR));
        fire.GetComponent<PhotonView>().TransferOwnership(photonView.ViewID);
        
    }


    private void Start() {
        fire.transform.SetParent(canvas);
        fire.gameObject.SetActive(false);
        StartFire();
    }


    private void Update() {
        if(!photonView.IsMine)
            return;
        if(fire){
            fire.transform.position = Camera.main.WorldToScreenPoint(setPoint.position);
            fire.transform.localScale = Vector3.one;
        }
    }


    // 起火
    private void StartFire(){
        Debug.Log("起火");

        FoodIngredient[] foods = transform.GetComponentsInChildren<FoodIngredient>();
        for(int i = 0; i < foods.Length; i++){
            foods[i].DoAction(FoodIngredientState.Break, null);
        }
    }

    // 灭火
    private void EndFire(){
        Debug.Log("灭火");
        Destroy(fire);
    }

    // 火焰蔓延
    private void SpreadFire(){
        Debug.Log("火焰蔓延");
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.IsWriting)
        {
            stream.SendNext(IsFire);
        }
        else
        {
            IsFire = (bool)stream.ReceiveNext();
        }
    }
}