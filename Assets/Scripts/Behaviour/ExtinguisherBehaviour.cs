using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灭火器
/// </summary>
public class ExtinguisherBehaviour : MonoBehaviour {

    GameObject smoke;//灭火器烟雾游戏对象
    private void Awake()
    {
        //加载灭火器烟雾预设体
        GameObject smokePreFab = Resources.Load<GameObject>(AssetConst.Prefab_PATH+GameConst.Smoke);
        //生成灭火器烟雾预设体
        smoke = Instantiate(smokePreFab);
        //初始化烟雾
        Init();
    }

    /// <summary>
    /// 是否使用灭火器
    /// </summary>
    /// <param name="isUse">true为使用,false为不使用</param>
    public void UseExtgui(bool isUse)
    {
        ParticleSystem particleSystem = smoke.GetComponent<ParticleSystem>();
        //判断是否在使用
        if (isUse)
        {
            //如果在使用，播放烟雾粒子特效
            particleSystem.Play();
        }
        else
        {
            //如果不使用，停止播放烟雾粒子特效
            particleSystem.Stop();
        }
    }

    /// <summary>
    /// 初始化烟雾位置，旋转，及缩放s
    /// </summary>
    private void Init()
    {
        smoke.transform.SetParent(transform);
        smoke.transform.localPosition = new Vector3(0f, 0.008f, 0.005f);
        smoke.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        smoke.transform.eulerAngles = transform.eulerAngles;
    }
}
