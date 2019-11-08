using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class CubeBehavior : MonoBehaviour 
{
    private void OnEnable()
    {
        StartCoroutine(DelayRecycle());
    }
    private IEnumerator DelayRecycle()
    {
        yield return new WaitForSeconds(2.5f);
        ObjectPool.instance.RecycleObj(gameObject);
    }
}
