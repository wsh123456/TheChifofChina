using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class TurnScene : MonoBehaviour {
    private IEnumerator Start() {
        // PhotonNetwork.LoadLevel("boat");
        yield return 0;
        // while(true){
        //     if(PhotonNetwork.LevelLoadingProgress >= 1)
        //         break;
        // }
        yield return new WaitForSeconds(3);
        Debug.Log(PhotonNetwork.LevelLoadingProgress);
    }
}