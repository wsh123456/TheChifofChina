using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool
{
    #region SingleTon
    public static readonly ObjectPool instance = new ObjectPool();
    //私有构造
    private ObjectPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabCache = new Dictionary<string, GameObject>();
    }
    #endregion

    //声明总对象池
    private Dictionary<string, List<GameObject>> pool;
    //声明预设体缓冲池
    private Dictionary<string, GameObject> prefabCache;

	#region Methods
    /// <summary>
    /// 生成对象
    /// </summary>
    /// <param name="objName">对象名字</param>
    /// <returns>返回对象预设体</returns>
    public GameObject CreateObject(string objName)
    {
        //定义一个临时对象
        GameObject tempObj = null;
        //判断是否存在子对象池，以及子池中有对象
        if (pool.ContainsKey(objName) && pool[objName].Count > 0)
        {
            //子池子有对象
            //复制
            tempObj = pool[objName][0];
            //将子池中第0个对象移除
            pool[objName].RemoveAt(0);
            //将取出的对象设为激活
            tempObj.SetActive(true);
            //将对象返回
            return tempObj;
        }

        //预设体对象
        GameObject prefab = null;
        //判断对象池是否有该预设体
        if (prefabCache.ContainsKey(objName))
        {
            //如果有，直接赋值预设体
            prefab = prefabCache[objName];
        }
        else
        {
            //如果没有，就从Resources加载预设体
            prefab = PrefabLoadManager.instance.LoadPrefabByName<GameObject>(objName);
            //添加到预设体缓冲池中
            prefabCache.Add(objName,prefab);
        }
        
        //生成预设体
        tempObj = UnityEngine.Object.Instantiate(prefab);

        
        //todo... 对象的行为脚本名字全名
        string typeFullName = objName +GameConst.Menu ;
        //如果行为脚本有命名空间，名字前添加命名空间
       // if (!string.IsNullOrEmpty(namespace))
        //{}
        //给预设体添加行为脚本
        tempObj.AddComponent(Type.GetType(typeFullName));

        //将对象返回
        return tempObj;
    }


    /// <summary>
    /// 传入路径，生成对象
    /// </summary>
    public GameObject CreateObject(string objName, string path)
    {
        //定义一个临时对象
        GameObject tempObj = null;
        //判断是否存在子对象池，以及子池中有对象
        if (pool.ContainsKey(objName) && pool[objName].Count > 0)
        {
            //子池子有对象
            //复制
            tempObj = pool[objName][0];
            //将子池中第0个对象移除
            pool[objName].RemoveAt(0);
            //将取出的对象设为激活
            tempObj.SetActive(true);
            //将对象返回
            return tempObj;
        }

        //预设体对象
        GameObject prefab = null;
        //判断对象池是否有该预设体
        if (prefabCache.ContainsKey(objName))
        {
            //如果有，直接赋值预设体
            prefab = prefabCache[objName];
        }
        else
        {

            //如果没有，就从Resources加载预设体
            prefab = PrefabLoadManager.instance.LoadPrefabByPath<GameObject>(path);

            //添加到预设体缓冲池中
            prefabCache.Add(objName,prefab);
        }
        

        //生成预设体
        tempObj = UnityEngine.Object.Instantiate(prefab);

        
        //todo... 对象的行为脚本名字全名
        string typeFullName = objName +GameConst.Menu ;
        //如果行为脚本有命名空间，名字前添加命名空间
       // if (!string.IsNullOrEmpty(namespace))
        //{}
        //给预设体添加行为脚本
        tempObj.AddComponent(Type.GetType(typeFullName));

        //将对象返回
        return tempObj;
    }


    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="obj"></param>
    public void RecycleObj(GameObject obj)
    {
        if(obj == null){
            return;
        }
        //先将要回收的对象设为非激活
        obj.SetActive(false);
        //获取对象名字
        string objName = obj.name.Replace("(Clone)", "");
        //判断对象池是否含有该对象的子池
        if (pool.ContainsKey(objName))
        {
            //如果有就直接加入到子池中
            pool[objName].Add(obj);
        }
        else
        {
            //如果没有就新建子池，并添加进去
            pool.Add(objName,new List<GameObject>() {obj });
        }
    }
	#endregion
}
