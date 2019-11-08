using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TimerContorller : MonoBehaviour
{
    public Timer levelTimer;//总体游戏计时器
    Text timerNum;//计时器数字
    float persent;//计时器进度条UI百分比
    Image timerBarImage;//计时器进度条
    TimeSpan timeSpan;//将秒换为分
    public Timer menuTimer;
    public AddMenu addMenu;
    public Timer destoryFoodMenuTimer;
    void Start()
    {
        
        timerNum = GameObject.Find("Timer_banner/Timer_Number").GetComponent<Text>();
        timerBarImage = GameObject.Find("Timer_bar_banner/Timer_bar_image").GetComponent<Image>();
        levelTimer = Timer.Register(LevelInstance._instance.levelTime, null, null, false, false, null);//给予计时器240s的倒计时
        menuTimer = Timer.Register(LevelInstance._instance.menuTimer, null, null, true, false, null);//上菜时间
        destoryFoodMenuTimer = Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, false, null);//毁单时间
    }
    // Update is called once per frame
    void Update()
    {
        timeSpan = new TimeSpan(0, 0, Convert.ToInt32(levelTimer.GetTimeRemaining()));//将秒换为分
        timerNum.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();//打印倒计时
        timerBarImage.fillAmount = levelTimer.GetRatioRemaining();//控制时间进度条的变化
        //Debug.Log(menuTimer.GetTimeRemaining());
        //AddMenuP();
    }
  
}
