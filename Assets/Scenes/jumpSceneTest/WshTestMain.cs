using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WshTestMain : MonoBehaviour {
    public InputField nameInput;
    public Button createBut;
    public Button joinBut;
    public WshTestRoom roomPanel;

    private void Awake() {
        createBut.onClick.AddListener(CreateRoom);
        joinBut.onClick.AddListener(JoinRoom);
    }

    private void JoinRoom(){
        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.JoinRandomRoom();
        // if(PhotonNetwork.JoinRoom("textRoom")){
        // }
        roomPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);

    }

    private void CreateRoom(){
        PhotonNetwork.NickName = nameInput.text;

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = byte.Parse("4");
        PhotonNetwork.CreateRoom("testRoom",room);

        roomPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
}