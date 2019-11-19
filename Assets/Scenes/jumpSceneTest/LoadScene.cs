using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class LoadScene : MonoBehaviour {
    private IEnumerator Start() {
        // 播放过场动画
        // 加载场景数据
        // 进行场景加载跳转
        PhotonNetwork.LoadLevel("boat");
        yield return 0;
    }

}