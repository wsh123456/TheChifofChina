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
    private GameObject[] initPoints;
    private void Awake() {
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
        Debug.Log("qqqq" + initPoints.Length);
        Transform point = initPoints[PhotonNetwork.LocalPlayer.ActorNumber-1].transform;
        GameObject player = PhotonNetwork.Instantiate("ChefPlayer", point.position, Quaternion.identity); 
        player.transform.SetParent(point);
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
        if(propertiesThatChanged.TryGetValue("CanInitPlayer", out temp)){
            if((bool)temp){
                InitPlayer();
            }
        }
    }


}