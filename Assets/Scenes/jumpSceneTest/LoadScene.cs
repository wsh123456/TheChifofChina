using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    // private Text progress;
    // private AsyncOperation async = null;
    // private bool loadEnd = false;
    // private float textAlpha;
    // public float blinkSpeed = 0.01f;

    // private void Awake() {
    //     progress = transform.Find("Panel/Text").GetComponent<Text>();
    // }


    private IEnumerator Start() {
        // 播放过场动画
        // 加载场景数据
        // 进行场景加载跳转
        // textAlpha = progress.color.a;
        // StartCoroutine(MyLoadScene("boat"));
        PhotonNetwork.LoadLevel("DongtaiLoad");
        
        yield return 0;
    }

    // private void Update() {
    //     if(textAlpha <= 0.1f){
    //         blinkSpeed = 0.01f;
    //     }else if(textAlpha >= 0.95f){
    //         blinkSpeed = -0.01f;
    //     }
    //     textAlpha += blinkSpeed;
    //     if(loadEnd){
    //         Debug.Log("aaaaaaa" + textAlpha);
    //         progress.color = new Color(progress.color.r, progress.color.g, progress.color.b, textAlpha);
    //     }
    // }

    // // 加载场景
    // public IEnumerator MyLoadScene(string scene){
    //     float pValue = 0;
    //     yield return 0;
    //     async = SceneManager.LoadSceneAsync(scene);
    //     async.allowSceneActivation = false;
    //     yield return new WaitForSeconds(3);
    //     while(!async.isDone){
    //         if(async.progress < 0.9f){
    //             pValue = async.progress;
    //             progress.text = pValue.ToString();
    //         }else{
    //             pValue = 1.0f;
    //             loadEnd = true;
    //             progress.text = "Press any key to start the game :)";
    //         }
    //         if(pValue >= 0.9f){
    //             Debug.Log("dqefrgthygrfdfsd");
    //             if(Input.anyKeyDown){
    //                 loadEnd = false; 
    //                 async.allowSceneActivation = true;
    //             }
    //         }

    //         yield return 0;
    //     }
    // }

}