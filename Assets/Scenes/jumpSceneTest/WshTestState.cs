using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;

public class WshTestState : MonoBehaviour {
    public Text state;

    private void Update() {
        state.text = "state: " + PhotonNetwork.NetworkClientState;
    }
}