#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):Panel_CountDown.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):Panel_CountDown
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Panel_CountDown :MonoBehaviourPunCallbacks,IPunObservable {

    public Sprite readyBack;
    public Sprite ready;
    public Sprite goBack;
    public Sprite go;

    private Image background;
    private Image readyState;

    private void Awake() {
        background = transform.Find("BackGround").GetComponent<Image>();
        readyState = transform.Find("BackGround/Number").GetComponent<Image>();
    }


    public void StartCountDown(){
        photonView.RPC("_StartCountDown", RpcTarget.All);
    }
    [PunRPC]
    private void _StartCountDown(){
        StartCoroutine("CountDown");
    }


    private IEnumerator CountDown(){
        yield return new WaitForSeconds(1f);
        background.gameObject.SetActive(true);
        readyState.gameObject.SetActive(true);
        background.sprite = readyBack;
        readyState.sprite = ready;
        yield return new WaitForSeconds(1f);
        background.sprite = goBack;
        readyState.sprite = go;
        yield return new WaitForSeconds(1f);
        background.gameObject.SetActive(false);
        readyState.gameObject.SetActive(false);

        Hashtable hashtable = new Hashtable();
        hashtable.Add("FinishCountDown", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}
}