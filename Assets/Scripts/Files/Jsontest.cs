#region 模块信息
// **********************************************************************
// 测试
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Jsontest : MonoBehaviour {
    private void Awake() {
        List<FoodIngredientModel> test = JsonFileControl.LoadFoodIngredient(new List<string>{"Cabbage","Tomato","Potato","Chicken"});
        GameObject a = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient",Vector3.zero);
        // a.GetComponent<FoodIngredient>().InitFoodIngredient(test[3]);
    }
}