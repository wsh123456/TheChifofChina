using UnityEngine;

/// <summary>
/// 单例脚本约束类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T m_instance = null;

    public static T Instance
    {
        get
        {
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        m_instance = this as T;
    }
}