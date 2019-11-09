using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class PlateBehaviour : MonoBehaviour 
{
    //声明一个列表存储盘子里的食材
    public List<GameObject> foodsList =new List<GameObject>();
    //声明isClean，默认true
    public bool isClean = true;
    
    /// <summary>
    ///  添加新食材到盘子里
    /// </summary>
    /// <param name="food">食材游戏对象</param>
    /// <param name="currPlate">当前放食材的盘子游戏对象</param>
    /// <returns>返回当前盘子所装的食材游戏对象</returns>
    public List<GameObject> AddFoodInPlate(GameObject food,GameObject currPlate)
    {
        if (!currPlate.GetComponent<PlateBehaviour>().isClean)
        {
            Debug.Log("该盘子是脏盘子，无法放食材");
            return null;
        }
        else
        {
            //获取当前盘子的行为脚本里的foodsList
            foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
            //将要添加的食材添加到foodsList列表里
            foodsList.Add(food);

            //设置食材的父对象，以及位置
            food.transform.position = currPlate.transform.position;
            food.transform.SetParent(currPlate.transform);
            //返回foodsList
            return foodsList;
        }
    }

    /// <summary>
    /// 清空盘子里的食材
    /// </summary>
    /// <param name="currPlate">要清空的盘子</param>
    public void ClearFoodsInPlate(GameObject currPlate)
    {
        //清空食物
        ClearFoods(currPlate);
    }

    /// <summary>
    /// 回收盘子
    /// </summary>
    /// <param name="currPlate">需要回收的盘子</param>
    public void DestoryPlate(GameObject currPlate)
    {
        //清空食物
        ClearFoods(currPlate);
        //回收盘子
        ObjectPool.instance.RecycleObj(currPlate);
    }





    /// <summary>
    /// 清空食物
    /// </summary>
    /// <param name="currPlate">要清空的盘子</param>
    private void ClearFoods(GameObject currPlate)
    {
        //获取当前盘子的foodsList列表
        foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
        if (foodsList.Count<=0)
        {
            return;
        }
        //回收食材
        for (int i = 0; i < foodsList.Count; i++)
        {
            ObjectPool.instance.RecycleObj(foodsList[i]);
        }
        //清空foodsList列表
        foodsList.Clear();
    }
}
