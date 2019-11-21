#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):WashPlateBehaviour.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):WashPlateBehaviour
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class WashPlateBehaviour : MonoBehaviourPunCallbacks {

    /// <summary>
    /// 
    /// </summary>
    private GameObject game;
    private bool isCanStop;
    private GameObject progressBar; // 进度条 
    private GameObject washPoint;
    private GameObject canvas;
    private void Awake()
    {
        washPoint = GameObject.FindWithTag("WashPoint");
        progressBar = Instantiate(Resources.Load<GameObject>(UIConst.PROGRESS_BAR));
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
    private void Start()
    {
        progressBar.transform.SetParent(canvas.transform);
        progressBar.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!game)
            return;
        Debug.Log(game.GetComponent<PlateBehaviour>().weshTime);
        Debug.Log(game.GetComponent<PlateBehaviour>().currentTime);
        photonView.RPC("SetProgressBar", RpcTarget.All,game.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    private void SetProgressBar(int index)
    {
        progressBar.transform.Find("Slider").GetComponent<Slider>().value = PhotonView.Find(index).GetComponent<PlateBehaviour>().weshTime / PhotonView.Find(index).GetComponent<PlateBehaviour>().currentTime;

    }
    private void OnTriggerStay(Collider other)
    {
        //洗盘子
        if (other.name.Contains("Plate"))
        {
        Debug.Log(other.name+"洗");
        Debug.Log(other.GetComponent<PlateBehaviour>().curstate == PlateBehaviour.PlateState.Dirty);
        if (!other.transform.parent.name.Contains("Hand")&&other.transform.parent== transform.GetChild(0)
            &&other.GetComponent<PlateBehaviour>().curstate == PlateBehaviour.PlateState.Dirty && Input.GetKeyDown(KeyCode.E))
            {
                game = other.gameObject;
                int viewID = game.GetComponent<PlateBehaviour>().photonView.ViewID;
                photonView.RPC("ShowPrograssBar", RpcTarget.All);
                photonView.RPC("StartWashtar",RpcTarget.All,viewID);
                isCanStop = true;
         
            }

        }
        if (other.name.Contains("Player"))
           {
                Debug.Log(other.GetComponent<Animator>().GetFloat("Walk")+"跑");
                if (isCanStop == true && other.GetComponent<Animator>().GetFloat("Walk") >0)
                {
                    game.GetComponent<PlateBehaviour>().photonView.RPC("BreakOperat",RpcTarget.All);
                }
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isCanStop == true && other.name.Contains("Player")&&game)
        {
            game.GetComponent<PlateBehaviour>().photonView.RPC("BreakOperat", RpcTarget.All);
        }
    }
   
    /// <summary>
    /// 开始洗盘子
    /// </summary>
    /// <param name="index"></param>
    [PunRPC]
    private void StartWashtar(int index)
    {
        Debug.Log("=============");
     PhotonView.Find(index).GetComponent<PlateBehaviour>().StartWash(FinishOperation);
    }

    [PunRPC]
    private void ShowPrograssBar()
    {
        progressBar.gameObject.SetActive(true);
    }
    [PunRPC]
    private void HidePrograssBar()
    {
        progressBar.gameObject.SetActive(false);
    }
    /// <summary>
    /// 洗盘子结束回调
    /// </summary>
    private void FinishOperation()
    {
        isCanStop = false;
        Debug.Log("洗盘子完成");
        photonView.RPC("HidePrograssBar", RpcTarget.All);
        game = null;
    }
}