using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 预设体加载管理
/// </summary>
public class PrefabLoadManager : MonoBehaviour 
{
    #region SingleTon
    public static readonly PrefabLoadManager instance = new PrefabLoadManager();
    private PrefabLoadManager()
    {

    }
	#endregion

	#region Public
    /// <summary>
    /// 加载指定类型的预设体
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="objectName">对象名字</param>
    /// <returns>返回预设体</returns>
	public T LoadPrefabByName<T>(string objectName) where T:Object
    {
        T tempAsset = Resources.Load<T>(AssetConst.Prefab_PATH+objectName);
        return tempAsset;
    }


    public T LoadPrefabByPath<T>(string path) where T:Object
    {
        T tempAsset = Resources.Load<T>(path);
        return tempAsset;
    }
	#endregion
}
