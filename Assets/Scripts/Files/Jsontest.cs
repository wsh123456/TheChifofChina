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
        GameObject a = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient");
        // GameObject b = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient");
        // GameObject c = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient");
        // GameObject d = ObjectPool.instance.CreateObject("FoodIngredient", "FoodIngredient/FoodIngredient");
        a.GetComponent<FoodIngredient>().InitFoodIngredient(test[3]);
        // b.GetComponent<FoodIngredient>().InitFoodIngredient(test[1]);
        // c.GetComponent<FoodIngredient>().InitFoodIngredient(test[2]);
        // d.GetComponent<FoodIngredient>().InitFoodIngredient(test[3]);
    }
}