using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UseObjectPool : MonoBehaviour 
{
   public static void CreatePrefab()
    {
        ObjectPool.instance.CreateObject("");
    }
}
