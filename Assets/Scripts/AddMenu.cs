using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddMenu : MonoBehaviour {
    int count = 0;
    Transform tran;
    void Awake()
    {
        tran = GameObject.Find("MenuPoint").GetComponent<Transform>();
    }
    public void AddMenuPanel()
    {
        if (count >=MenuManage.menuManage.maxMenuNum) return;
        GameObject menu= Resources.Load<GameObject>("Menu");
        menu = Instantiate(menu,tran);
        menu.transform.SetParent(tran);
        menu.transform.localScale=new Vector3(1,1,1);
        count += 1;
        
        //Debug.Log(tran);
    }
}
