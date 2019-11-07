using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TimerContorller : MonoBehaviour
{
  protected  Timer gameTimer;//总体游戏计时器
    Text timerNum;//计时器数字
    float persent;//计时器进度条UI百分比
    Image timerBarImage;//计时器进度条
    TimeSpan timeSpan;//将秒换为分
    protected Timer menuTimer;
    void Start()
    {
        timerNum = GameObject.Find("Timer_banner/Timer_Number").GetComponent<Text>();
        timerBarImage = GameObject.Find("Timer_bar_banner/Timer_bar_image").GetComponent<Image>();
        gameTimer = Timer.Register(240f, null, null, false, false, null);//给予计时器240s的倒计时
        menuTimer = Timer.Register(15f, null, null, true, false, null);//上菜时间
    }
    // Update is called once per frame
    void Update()
    {
        timeSpan = new TimeSpan(0, 0, Convert.ToInt32(gameTimer.GetTimeRemaining()));//将秒换为分
        timerNum.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();//打印倒计时
        timerBarImage.fillAmount = gameTimer.GetRatioRemaining();//控制时间进度条的变化
        //Debug.Log(menuTimer.GetTimeRemaining());
        AddMenu();
    }
    void AddMenu()
    {
        if (menuTimer.GetTimeRemaining() <= 0.2f)
        {
            Resources.Load("Menu");
            Debug.Log("+1道菜");
        }
    }
}
