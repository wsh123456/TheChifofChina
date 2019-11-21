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
    
    private GameCanvasController canvas;
    private bool isInitPlayer = false;

    private void Awake() {
        _ins = this;
        // 找到玩家生成的点
        initPoints = GameObject.FindGameObjectsWithTag("PlayerPoint");
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameCanvasController>();
    }

    private void Start() {
        // 设置标记自己加载完场景了
        Hashtable hashtable = new Hashtable();
        hashtable.Add("LoadLevelDone", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        PhotonNetwork.SendRate = 70;
        PhotonNetwork.SerializationRate = 70;
    }


    // 检测是否所有玩家都加载并进入场景
    private bool CheckAllFinish(string key){
        object value;
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            if(!PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue(key, out value)){
                return false;
            }
        }
        return true;
    }


    private void InitPlayer(){
        
        Transform point = initPoints[PhotonNetwork.LocalPlayer.ActorNumber-1].transform;
        GameObject player = PhotonNetwork.Instantiate("ChefPlayer", point.position, Quaternion.identity); 
        curPlayer = player.GetComponent<PlayerController>();
        curPlayer.photonView.RPC("SetParent", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber-1);
        // 换头
        object headIndex;
        if(!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("ChefHeadIndex", out headIndex)){
            headIndex = 0;
        }
        curPlayer.ChangeChiefHead((int)headIndex);

        Hashtable hashtable = new Hashtable();
        hashtable.Add("FinishInitPlayer", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

        isInitPlayer = true;
    }

    // 开始同步倒计时
    private void StartCountDown(){
        // 房主进行倒计时，同步结果给其他人
        canvas.StartCountDown();
    }

    // 同步开始游戏
    private void StartGame(){
        // 房主通知所有人可以开始游戏
        // 开启菜单列表
        // 开启倒计时同步
        canvas.StartGame();
        
        // 开始场景动作(船)
        // todo：

        // 开启玩家控制
        Hashtable hashtable = new Hashtable();
        hashtable.Add("PlayerCanMove", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }


    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps){
        // 不是房主，不执行
        if(!PhotonNetwork.IsMasterClient){
            return;
        }
        object temp;

        // 房主判断所有人加载完毕
        if(changedProps.TryGetValue("LoadLevelDone", out temp)){
            if(CheckAllFinish("LoadLevelDone")){
                // 通知所有人生成英雄
                Hashtable hashtable = new Hashtable();
                hashtable.Add("CanInitPlayer", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
        }

        // 判断所有人生成玩家成功后
        if(changedProps.TryGetValue("FinishInitPlayer", out temp)){
            if(CheckAllFinish("FinishInitPlayer")){
                // 通知所有人开始倒计时
                Hashtable hashtable = new Hashtable();
                hashtable.Add("StartCountDown", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
        }

        // // 判断所有人倒计时结束
        if(changedProps.TryGetValue("FinishCountDown", out temp)){
            if(CheckAllFinish("FinishCountDown")){
                // 通知所有人可以开始操作了
                Hashtable hashtable = new Hashtable();
                hashtable.Add("StartGame", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged){
        object temp;
        // object temp1;
        if(propertiesThatChanged.TryGetValue("CanInitPlayer", out temp)){
            if((bool)temp && !isInitPlayer){
                InitPlayer();
            }
        }

        // // 通知所有人开始倒计时
        if(propertiesThatChanged.TryGetValue("StartCountDown", out temp)){
            if((bool)temp){
                StartCountDown();
            }
        }

        // 通知所有人可以开始操作
        if(propertiesThatChanged.TryGetValue("StartGame", out temp)){
            if(temp != null){
                StartGame();
            }
        }
    }


}