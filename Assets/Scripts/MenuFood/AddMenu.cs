using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMenu : MonoBehaviour {
    int count = 0;
<<<<<<< HEAD:Assets/Scripts/AddMenu.cs
    RectTransform tran;
=======
    Transform tran;
    
>>>>>>> 2d74089f283aaf40dd1a452178e8f5e6ed6791e2:Assets/Scripts/MenuFood/AddMenu.cs
    void Awake()
    {
        tran = GameObject.Find("MenuPoint").GetComponent<RectTransform>();
    }
    public void AddMenuPanel()
    {
<<<<<<< HEAD:Assets/Scripts/AddMenu.cs
        if (count >= 6) return;
        //使用对象池生成菜单
        GameObject menu = ObjectPool.instance.CreateObject(GameConst.Menu);
        InitMenu(menu);
=======
       
        if (count >= LevelInstance._instance.maxMenuNum) return;
        GameObject menu= Resources.Load<GameObject>("Menu");
        menu = Instantiate(menu,tran);
        menu.transform.SetParent(tran);
        menu.transform.localScale=new Vector3(1,1,1);
>>>>>>> 2d74089f283aaf40dd1a452178e8f5e6ed6791e2:Assets/Scripts/MenuFood/AddMenu.cs
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
