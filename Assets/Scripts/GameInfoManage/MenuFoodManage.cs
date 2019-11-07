using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
    public class MenuFoodManage
    {
   
   
        ArrayList menuFood;
    void Awake()
    {
        menuFood = new ArrayList();
        
    }
    /// <summary>
    /// 添加菜谱
    /// </summary>
    /// <param name="value">菜</param>
     public void Add(object value)
    {
        menuFood.Add(value);
    }
    public void AddCookBook(int time)
    {

    }
}
    
