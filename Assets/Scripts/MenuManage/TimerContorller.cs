using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TimerContorller : MonoBehaviour
{
    Text timerNum;//计时器数字
    float persent;//计时器进度条UI百分比
    Image timerBarImage;//计时器进度条
    TimeSpan timeSpan;//将秒换为分
    Timer levelTimer;
    public AddMenu addMenu;
    void Start()
    {
        levelTimer = TimerInstance.instance.levelTimer;
        timerNum = GameObject.Find("Timer_banner/Timer_Number").GetComponent<Text>();
        timerBarImage = GameObject.Find("Timer_bar_banner/Timer_bar_image").GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        timeSpan = new TimeSpan(0, 0, Convert.ToInt32(levelTimer.GetTimeRemaining()));//将秒换为分
        timerNum.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();//打印倒计时
        timerBarImage.fillAmount = levelTimer.GetRatioRemaining();//控制时间进度条的变化
        //Debug.Log(menuTimer.GetTimeRemaining());
        
    }
}
