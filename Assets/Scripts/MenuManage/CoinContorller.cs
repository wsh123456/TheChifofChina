using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;


public class CoinContorller : MonoBehaviourPunCallbacks,IPunObservable {
    Text coinNum;
    bool isCheck;
    AddMenu addMenu;
	// Use this for initialization
	void Start () {
        coinNum = transform.Find("Coin_banner/Coin_Number").GetComponent<Text>();
        addMenu = GameObject.FindWithTag("Canvas").GetComponent<AddMenu>();

    }
	
	// Update is called once per frame
	void Update () {
        coinNum.text = MenuManage.menuManage.price.ToString();
	}

    // 设置金币
    [PunRPC]
    public void SetIcon(int coin){
        coinNum.text = (int.Parse(coinNum.text) + coin).ToString();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}
    
}
