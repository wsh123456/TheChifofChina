#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):PlayerInfoModule.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):PlayerInfoModule
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIFrameWork;

public class PlayerInfoModule : UIModuleBase {

    protected override void Awake()
    {
        moduleType = ModuleType.Once;
        base.Awake();
        controller = UIBindings.Bind<PlayerInfoController>(this);
        controller.ModuleInit();
    }

}