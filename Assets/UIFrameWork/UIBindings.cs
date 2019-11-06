using UnityEngine;
using System.Collections;
using System;

namespace UIFrameWork
{
    public class UIBindings{
        public static T Bind<T>(UIModuleBase moduleBase) where T : UIControllerBase{
            T controller = moduleBase.gameObject.AddComponent<T>();
            controller.module = moduleBase;
            return controller;
        }
    }
    
}
