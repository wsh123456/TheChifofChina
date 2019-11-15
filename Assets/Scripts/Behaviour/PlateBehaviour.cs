using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class PlateBehaviour : MonoBehaviour
{
    private LevelInstance Levelins;
    //声明一个列表存储盘子里的食材
    public List<GameObject> foodsList = new List<GameObject>();
    //声明isClean，默认true
    public bool isClean = true;
    public List<string> foodName;
    private void Start()
    {
        Levelins = LevelInstance._instance;
        menu = new Dictionary<string, List<string>>();
        Menu();
    }
    /// <summary>
    ///  添加新食材到盘子里
    /// </summary>
    /// <param name="food">食材游戏对象</param>
    /// <param name="currPlate">当前放食材的盘子游戏对象</param>
    /// <returns>返回当前盘子所装的食材游戏对象</returns>
    public List<GameObject> AddFoodInPlate(GameObject food, GameObject currPlate)
    {
        if (!isClean)
        {
            Debug.Log("该盘子是脏盘子，无法放食材");
            return null;
        }
        else
        {
            //获取当前盘子的行为脚本里的foodsList
            //foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
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
    /// 获取盘子里食材的string类型列表
    /// </summary>
    /// <returns>返回string类型食材列表</returns>
    public List<string> GetFoodList()
    {
        List<string> foodList = new List<string>();
        for (int i = 0; i < transform.childCount; i++)
        {
            foodList.Add(transform.GetChild(i).GetComponent<FoodIngredient>().GetIType().ToString());
            Debug.Log(foodsList[i].name);
        }
        return foodList;
    }

    /// <summary>
    /// 定义一个清空食物的方法
    /// </summary>
    /// <param name="currPlate">要清空的盘子</param>
    private void ClearFoods(GameObject currPlate)
    {
        //获取当前盘子的foodsList列表
        //foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
        if (foodsList.Count <= 0)
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
    private Dictionary<string, List<string>> menu;

    //拿到菜谱 
    private void Menu()
    {
        foreach (var item in Levelins.levelFood)
        {
            List<string> CName = new List<string>();
            foreach (var i in item.Value.foodIngredient)
            {
                CName.Add(i.name);
            }
            menu.Add(item.Key, CName);
        }
    }

    /// <summary>
    /// 可以放到盘子里
    /// </summary>
    /// <returns></returns>
    public bool CanInPlate(FoodIngredient food, GameObject inPlateObj)
    {
      
        if (transform.childCount > 0)
        {    //判断重读
            for (int i = 0; i < transform.childCount; i++)
            {
                if (food.GetIType() == transform.GetChild(i).GetComponent<FoodIngredient>().GetIType())
                {
                    return false;
                }

            }
            foreach (var item in menu)
            {
                if (item.Value.Contains(food.GetIType().ToString()) && food.DoAction(FoodIngredientState.InPlate, null))
                {
                    transform.parent = inPlateObj.transform;
                    inPlateObj.transform.localPosition = Vector3.zero;
                    inPlateObj.transform.localEulerAngles = Vector3.zero;
                    Destroy(inPlateObj.transform.GetComponent<Rigidbody>());
                    inPlateObj.tag = "Untagged";
                    
                    return true;
                }

            }
                    return false;
        }
        else
        {
            //判断状态
            if (food.curState == FoodIngredientState.Normal)
            {

                return false;
            }
            return false;
        }
    }
    /// <summary>
    /// 持续检测盘子中的食物
    /// </summary>
    private void UpdateCheckPlate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            
        }
    }
}
