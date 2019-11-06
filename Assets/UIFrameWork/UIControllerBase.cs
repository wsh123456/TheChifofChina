using UnityEngine;
using System.Collections;

namespace UIFrameWork
{
    public class UIControllerBase : MonoBehaviour {
        public UIModuleBase module;

        public virtual void ModuleInit(){}
    }
    
}
