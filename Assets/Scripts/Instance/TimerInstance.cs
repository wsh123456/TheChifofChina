using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerInstance{
    public static readonly TimerInstance instance = new TimerInstance();
    public Timer levelTimer = Timer.Register(LevelInstance._instance.levelTime, null, null, false, true, null);
    //给予游戏时间倒计时
    public Timer menuTimer=Timer.Register(LevelInstance._instance.menuTimer, null, null, false, true, null);
    //加菜时间;
    public Timer destoryFoodMenuTimer= Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null);
    //毁单时间
    
    private TimerInstance()
    {
        
        
    }
}
