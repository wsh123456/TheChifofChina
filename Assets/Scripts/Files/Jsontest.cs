#region 模块信息
// **********************************************************************
// 测试
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;

public class Jsontest : MonoBehaviour {
    private void Awake() {
        LevelInstance._instance.LoadLevel(1);
    }
}