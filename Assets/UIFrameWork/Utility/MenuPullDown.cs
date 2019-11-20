#region 模块信息
// **********************************************************************
// Copyright (C) 2019 QIANFENG EDUCATION
//
// 文件名(File Name):MenuPullDown.cs
// 公司(Company):#COMPANY#
// 作者(Author):#AUTHOR#
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):#UNITYVERSION#
// 创建时间(CreateTime):#DATE#
// 修改者列表(modifier):无
// 模块描述(Module description):MenuPullDown
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPullDown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject menu;

    private void Awake()
    {
        menu = transform.Find("MenuBG").gameObject;
    }
    private void Start()
    {
        menu.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.Find("#ClockImage"))
        {
            return;
        }
        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/t_ui_main_menu_button_03_highlight_d");
        menu.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.Find("#ClockImage"))
        {
            return;
        }
        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/t_ui_main_menu_button_03_d");
        menu.SetActive(false);
    }
}