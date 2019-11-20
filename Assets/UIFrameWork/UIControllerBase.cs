using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

namespace UIFrameWork
{
    public class UIControllerBase : MonoBehaviourPunCallbacks
    {
        public UIModuleBase module;

        public virtual void ModuleInit() { }

        public override void OnConnectedToMaster()
        {
        }

    }
}