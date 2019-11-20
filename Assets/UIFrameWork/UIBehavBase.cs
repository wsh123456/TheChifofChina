using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFrameWork{
    public class UIBehavBase : UIMonoBehaviours
    {
        protected UIModuleBase curModuleName;

        protected virtual void Awake() {
            // 先获取当前元件所在的模块
            GetCurrentModule();
            // 进行组件的初始化
            InitUIComponents();
            // 对元件进行注册
            Debug.Log("注册元件: " + curModuleName);
            curModuleName.RegisterBehav(name, this);
        }

        protected virtual void OnEnable(){
        }

        protected virtual void OnDisable() {
            // 对元件进行注销
            curModuleName.UnRegisterBehav(name);
        }


        // 获取当前的模块
        protected virtual void GetCurrentModule(){
            curModuleName = transform.GetComponentInParent<UIModuleBase>();
        }

    }
}