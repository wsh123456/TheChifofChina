using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UIFrameWork{
    public class UIModuleBase : MonoBehaviour {
        public UIControllerBase controller;
        public Dictionary<string, UIBehavBase> manageredUI;  // 模块内注册的事件
        public ModuleType moduleType = ModuleType.Aaa;  // 模块的类型
        
        private Transform[] allChild; 
        
        protected virtual void Awake() {
            manageredUI = new Dictionary<string, UIBehavBase>();
            Debug.Log("声明事件字典");
            if(moduleType != ModuleType.Once){
                UIManagerBase._instance.RegisterModule(this.name, this);
            }
            // 注册相关UI控件并绑定behav脚本
            AddScript();
        }
        
        public UIBehavBase FindWidget(string widgetName){
            if(!ContainsBehav(widgetName)){
                return null;
            }
            return manageredUI[widgetName];
        }
        
        private void OnDestroy() {
            UIManagerBase._instance.UnRegisterModule(this.name);        
        }
        
        private bool ContainsBehav(string widgetName){
            if(manageredUI.ContainsKey(widgetName)){
                return true;
            }
            return false;
        }
        
        // 注册并绑定脚本
        private void AddScript(){   
            allChild = transform.GetComponentsInChildren<Transform>();
            for(int i = 0; i < allChild.Length; i++)
			{
                // 如果是以该token标记结尾，绑定该token的UIBehav脚本
                if(allChild[i].name.StartsWith("#")){
                    // 如果存在并继承UIBehav,为对象添加脚本
                    allChild[i].gameObject.AddComponent(typeof(UIBehavBase));
                }
            }
        }
        
        // 注册元件
        public void RegisterBehav(string widgetName, UIBehavBase widget){
            if(!manageredUI.ContainsKey(widgetName)){
                manageredUI.Add(widgetName, widget);
            }
            else {
            }
        }
        
        // 移除元件
        public void UnRegisterBehav(string widgetName){
            if(!manageredUI.ContainsKey(widgetName)){
                manageredUI.Remove(widgetName);
            }
            else {
            }
        }
	}
}