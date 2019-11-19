using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerInstance{
    public static readonly TimerInstance instance = new TimerInstance();
    
    //给予游戏时间倒计时
    public Timer levelTimer;
    //加菜时间;
    public Timer menuTimer;
    //毁单时间
    public Timer destoryFoodMenuTimer;

    private TimerInstance()
    {
        menuTimer=Timer.Register(LevelInstance._instance.menuTimer, null, null, true, true, null);
        destoryFoodMenuTimer= Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null);
        levelTimer = Timer.Register(LevelInstance._instance.levelTime, null, null, false, true, null);
    }

    public void StartTimer(){
    }
}
