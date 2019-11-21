using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIFrameWork
{
    public class UIManagerBase{
        // 存所有的module，绑定module事件
        public Dictionary<string, UIModuleBase> manageredModule;
    
        public readonly static UIManagerBase _instance = new UIManagerBase();
        private UIManagerBase(){
            manageredModule = new Dictionary<string, UIModuleBase>();
        }

        // 注册模块
        public void RegisterModule(string moduleName, UIModuleBase module){
            // 如果当前模块还没有被注册，将其注册
            if(!manageredModule.ContainsKey(moduleName)){
                manageredModule.Add(moduleName, module);
            }
        }

        // 移除注册模块
        public void UnRegisterModule(string moduleName){
            // 如果当前模块存在，将其删掉
            if(manageredModule.ContainsKey(moduleName)){
                manageredModule.Remove(moduleName);
            }
        }

        // 显示模块,隐藏其他模块
        public void ShowModule(string moduleName){
            if(!ContainsModule(moduleName)){
                Debug.Log(moduleName);
                return;
            }

            foreach(var item in manageredModule){
                if(item.Key == moduleName){
                    item.Value.gameObject.SetActive(true);
                }else if(item.Value.moduleType == ModuleType.Aaa){
                    item.Value.gameObject.SetActive(false);
                }
            }
            FindModule(moduleName);
        }

        // 显示/隐藏某一个模块
        public void SetModuleActive(string moduleName, bool active){
            if(!ContainsModule(moduleName)){
                return;
            }
            manageredModule[moduleName].gameObject.SetActive(active);
        }

        // 获取注册过的模块
        public UIModuleBase FindModule(string moduleName){
            UIModuleBase temp;

            if(ContainsModule(moduleName)){
                temp = manageredModule[moduleName];
                try{
                    return temp.GetComponent<UIModuleBase>();
                }catch{
                    return null;
                }
            }
            return null;
        }

        // 是否有这个module
        private bool ContainsModule(string moduleName){
            if(!manageredModule.ContainsKey(moduleName)){
                Debug.LogWarning("未找到注册过的 \"" + moduleName + "\" 模块");
                return false;
            }
            return true;
        }
    }

    public enum ModuleType
    {
        Aaa,//批量
        Bbb,//独立的
        Once//用于动态生成的重复UIModule,不在Manager中注册Module模块
    }
}