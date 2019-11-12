using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class WshTestRoom : MonoBehaviourPunCallbacks {
    public Button startBut;
    public Transform playerlist;

    private List<Player> players;
    private List<Text> playerInfos;

    private void Awake() {
        startBut.onClick.AddListener(StartGame);

        players = new List<Player>();
        playerInfos = new List<Text>();
    }

    private void ClearList(){
        for(int i = playerlist.childCount - 1; i >=0 ; i--){
            Destroy(playerlist.GetChild(i).gameObject);
        }
        players.Clear();
        playerInfos.Clear();
    }

    private void StartGame(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("LoadScene");
        }
    }

    private void ShowRoom(){
        ClearList();
        Debug.Log(PhotonNetwork.CurrentRoom.Players.Count);
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            Debug.Log(PhotonNetwork.PlayerList[i].NickName);
        }
    }

    public override void OnJoinedRoom(){
        ShowRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        ShowRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        ShowRoom();
    }
}