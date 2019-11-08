using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMenu : MonoBehaviour {
    int count = 0;
    RectTransform tran;
    void Awake()
    {
        tran = GameObject.Find("MenuPoint").GetComponent<RectTransform>();
    }
    public void AddMenuPanel()
    {
        if (count >= 6) return;
        //使用对象池生成菜单
        GameObject menu = ObjectPool.instance.CreateObject(GameConst.Menu);
        InitMenu(menu);
        count += 1;
        
    }

    /// <summary>
    /// 初始化操作
    /// </summary>
    /// <param name="obj">操作对象</param>
    public void InitMenu(GameObject obj)
    {
        RectTransform menu = obj.GetComponent<RectTransform>();
        //设置父对象
        menu.transform.SetParent(tran);
        //设置缩放，旋转，及位置
        menu.localScale = Vector3.one;
        menu.rotation = tran.rotation;
        menu.anchoredPosition3D = tran.anchoredPosition3D;
    }
}
