using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;

public class CanvasController : Singleton<CanvasController> {
    private Dictionary<string, GameObject> panels;
    protected override void Awake() {
        panels = new Dictionary<string, GameObject>();
        base.Awake();
        for(int i = 0; i < transform.childCount; i++){
            panels.Add(transform.GetChild(i).name, transform.GetChild(i).gameObject);
        }
    }

    private void Start() {
        // ShowModule("Panel2");
        // UIManagerBase._instance.ShowModule(moduleName);
    }

    public void ShowModule(string moduleName){
        foreach(var item in panels){
            item.Value.SetActive(item.Key == moduleName);
        }
    }
}