#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-13
// 修改者列表(modifier):无
// 模块描述(Module description):游戏流程控制
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks {
    // 1.检测所有玩家都加载完成
    // 2.生成玩家使用的角色并同步倒计时
    // 3.同步开始游戏
    public static GameManager _ins;
    public GameObject[] initPoints;
    public PlayerController curPlayer;

    private void Awake() {
        _ins = this;
        // 找到玩家生成的点
        initPoints = GameObject.FindGameObjectsWithTag("PlayerPoint");
    }

    private void Start() {
        // 设置标记自己加载完场景了
        Hashtable hashtable = new Hashtable();
        hashtable.Add("LoadLevelDone", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    /// <summary>
    /// 检测是否所有玩家都加载并进入场景
    /// </summary>
    private bool CheckAllLoadAndStart(){
        object isLoaded;
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            if(!PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("LoadLevelDone", out isLoaded)){
                return false;
            }
        }
        return true;
    }


    private void InitPlayer(){
        Transform point = initPoints[PhotonNetwork.LocalPlayer.ActorNumber-1].transform;
        GameObject player = PhotonNetwork.Instantiate("ChefPlayer", point.position, Quaternion.identity); 
        // player.GetComponent<PlayerController>().photonView.RPC("SetParent", RpcTarget.All, point);
        curPlayer = player.GetComponent<PlayerController>();
        curPlayer.photonView.RPC("SetParent", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber-1);
        // 换头
        curPlayer.ChangeChiefHead(Random.Range(0,6));
    }


    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps){
        // 不是房主，不执行
        if(!PhotonNetwork.IsMasterClient){
            return;
        }
        // 房主判断所有人加载完毕
        if(CheckAllLoadAndStart()){
            // 通知所有人生成英雄
            Hashtable hashtable = new Hashtable();
            hashtable.Add("CanInitPlayer", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged){
        object temp;
        object temp1;
        if(propertiesThatChanged.TryGetValue("CanInitPlayer", out temp)){
            if((bool)temp){
                InitPlayer();
            }
        }

        // if(propertiesThatChanged.TryGetValue("CreatFoodIngredient", out temp) 
        // && propertiesThatChanged.TryGetValue("CreatFoodViewID", out temp1)){
        //     if(temp != null && temp1 != null){
        //         PhotonView.Find((int)temp1).transform.GetComponent<PlayerHandController>().CreateFoodIngredient(PhotonView.Find((int)temp).gameObject);
        //     }
        // }

        // if(propertiesThatChanged.TryGetValue("ThrowThingInHand", out temp) 
        // && propertiesThatChanged.TryGetValue("ThrowThingViewID", out temp1)){
        //     if(temp != null){
        //         PhotonView.Find((int)temp1).transform.GetComponent<PlayerHandController>().ThrowThings(PhotonView.Find((int)temp).gameObject);
        //     }
        // }
    }


}