using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class DirtyPlateBehaviour : PlateBehaviour
{
    /// <summary>
    /// 销毁脏盘子
    /// </summary>
    /// <param name="currDirtyPlate">脏盘子对象</param>
    public void DestroyDirtyPlate(GameObject currDirtyPlate)
    {
        ObjectPool.instance.RecycleObj(currDirtyPlate);
    }
}
