using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddMenu : MonoBehaviour {
    private Transform tran;
    void Awake()
    {
        tran = transform.Find("MenuPoint").transform;
        Debug.Log(tran + " -----------------");
    }
    public void AddMenuPanel()
    {
        Debug.Log(tran);
        if (MenuManage.menuManage.currentMenuNum >=MenuManage.menuManage.maxMenuNum) return;
        GameObject menu= Resources.Load<GameObject>("Menu");
        menu = Instantiate(menu);
        menu.transform.SetParent(tran);
        menu.transform.localScale = new Vector3(1, 1, 1);
        menu.transform.localRotation = new Quaternion(0,0,0,0);
        menu.transform.localPosition = new Vector3(50, 0, 0);

    }
}
