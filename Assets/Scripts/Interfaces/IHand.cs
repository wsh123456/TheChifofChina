#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-14
// 模块描述(Module description):拿在玩家手里的东西的接口
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public interface IHand{
    // 捡
    bool Pick(PlayerHandController player, Action<GameObject> callback);
    // 扔
    bool Throw(PlayerHandController player, Action<GameObject> callback);
    // 放
    bool PutDown(PlayerHandController player, Action<GameObject> callback);
}