using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class TimerContorller : MonoBehaviourPunCallbacks,IPunObservable
{
    Text timerNum;//计时器数字
    float persent;//计时器进度条UI百分比
    Image timerBarImage;//计时器进度条
    TimeSpan timeSpan;//将秒换为分
    Timer levelTimer;
    public AddMenu addMenu;

    // 开始游戏计时标志
    private bool isStart = false;
    public bool IsStart{
        get{return isStart;}
        set{
            if(value){
                StartCoroutine("StartTimmer");
            }
            isStart = value;
        }
    }

    void Start()
    {
        levelTimer = TimerInstance.instance.levelTimer;
        levelTimer.Pause();
        timerNum = GameObject.Find("Timer_banner/Timer_Number").GetComponent<Text>();
        timerBarImage = GameObject.Find("Timer_bar_banner/Timer_bar_image").GetComponent<Image>();

        ShowTimer();
    }

    // 开始计时器
    private IEnumerator StartTimmer(){
        levelTimer.Resume();
        yield return 0;
        while(true){
            ShowTimer();
            yield return new WaitForFixedUpdate();
        }
    }

    // 在UI上显示时间
    private void ShowTimer(){
        timeSpan = new TimeSpan(0, 0, Convert.ToInt32(levelTimer.GetTimeRemaining()));//将秒换为分
        timerNum.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();//打印倒计时
        timerBarImage.fillAmount = levelTimer.GetRatioRemaining();//控制时间进度条的变化
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(IsStart);
        }else{
            IsStart = (bool)stream.ReceiveNext();
        }
    }
}
