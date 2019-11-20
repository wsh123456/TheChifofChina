using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;

public class CanvasController : Singleton<CanvasController> {

    public Dictionary<string, GameObject> panels;

    protected override void Awake() {
        panels = new Dictionary<string, GameObject>();
        base.Awake();
        for(int i = 0; i < transform.childCount; i++){
            panels.Add(transform.GetChild(i).name, transform.GetChild(i).gameObject);
        }
    }

    private void Start() {
        ShowModule("PlayerInfoPanel");
    }

    public void ShowModule(string moduleName){
        foreach(var item in panels){
            if (item.Value.GetComponent<UIModuleBase>() != null && item.Value.GetComponent<UIModuleBase>().moduleType == ModuleType.Aaa)
            {
                item.Value.SetActive(item.Key == moduleName);
            }
        }
    }

    public void FindAndSetActive(string moduleName, bool active){
        try{
            transform.Find(moduleName).gameObject.SetActive(active);
        }catch{
            Debug.LogError("不存在这个模块");
        }
    }
}