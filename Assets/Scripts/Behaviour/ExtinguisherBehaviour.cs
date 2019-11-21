using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System;

/// <summary>
/// 灭火器
/// </summary>
public class ExtinguisherBehaviour : MonoBehaviourPunCallbacks,IPunObservable,IHand{

    private GameObject smoke;   // 灭火器烟雾游戏对象
    private ParticleSystem co2;     // 喷射出的特效
    private bool isUse;
    public bool IsUse{
        get{return isUse;}
        set{
            Debug.Log("灭火器使用");
            if(value){
                co2.Play();
            }else{
                co2.Stop();
            }
            isUse = value;
        }
    }

    private void Awake()
    {
        //加载灭火器烟雾预设体
        GameObject smokePreFab = Resources.Load<GameObject>(AssetConst.Prefab_PATH+GameConst.Smoke);
        //生成灭火器烟雾预设体
        smoke = Instantiate(smokePreFab);
        //初始化烟雾
        Init();
        co2 = smoke.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 是否使用灭火器
    /// </summary>
    /// <param name="isUse">true为使用,false为不使用</param>
    public void UseExtgui(bool isUse)
    {
        IsUse = isUse;
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


    public bool Pick(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }

    public bool Throw(PlayerHandController player, Action<GameObject> callback)
    {
        return false;
    }

    public bool PutDown(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.IsWriting)
        {
            stream.SendNext(IsUse);
        }
        else
        {
            IsUse = (bool)stream.ReceiveNext();
        }
    }
}
