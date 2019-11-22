using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System;

/// <summary>
/// 
/// </summary>
public class PlateBehaviour : MonoBehaviourPunCallbacks,IHand,IPunObservable
{
    public Transform clearPoint;
    public Action action;
    private LevelInstance Levelins;
    //声明一个列表存储盘子里的食材
    public List<GameObject> foodsList = new List<GameObject>();
    //声明isClean，默认true
    public float weshTime ;
    public float currentTime;
    public float speedTime = 1f;
    public bool isClean = true;
    public List<string> foodName;
    public int[] viewIndex;
    public PlateState curstate= PlateState.Clear;
    private void Start()
    {
        weshTime = 0f;
        currentTime = 10f;
        Levelins = LevelInstance._instance;
        menu = new Dictionary<string, List<FoodModel_FoodIngredient>>();
        clearPoint = GameObject.FindWithTag("ClearPoint").transform;
        Menu();
    }
    /// <summary>
    ///  添加新食材到盘子里
    /// </summary>
    /// <param name="food">食材游戏对象</param>
    /// <param name="currPlate">当前放食材的盘子游戏对象</param>
    /// <returns>返回当前盘子所装的食材游戏对象</returns>
    //public List<GameObject> AddFoodInPlate(GameObject food, GameObject currPlate)
    //{
    //    if (!isClean)
    //    {
    //        Debug.Log("该盘子是脏盘子，无法放食材");
    //        return null;
    //    }
    //    else
    //    {
    //        //获取当前盘子的行为脚本里的foodsList
    //        //foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
    //        //将要添加的食材添加到foodsList列表里
    //        foodsList.Add(food);
    //        //设置食材的父对象，以及位置
    //        food.transform.position = currPlate.transform.position;
    //        food.transform.SetParent(currPlate.transform);
    //        //返回foodsList
    //        return foodsList;
    //    }
    //}

    

    /// <summary>
    /// 获取盘子里食材的string类型列表
    /// </summary>
    /// <returns>返回string类型食材列表</returns>
    public List<string> GetFoodList()
    {
        List<string> foodList = new List<string>();
        if (transform.childCount>0)
        {
        for (int i = 0; i < transform.childCount; i++)
        {
            foodList.Add(transform.GetChild(i).GetComponent<FoodIngredient>().GetIType().ToString());
            Debug.Log(foodsList[i].name);
        }
        return foodList;
        }
        Debug.Log("没有子节点");
        return null;
    }

/// <summary>
/// 调用此方法清除盘子中的食材
/// </summary>
    private void ClearInPlateFoods()
    {
        viewIndex = new int[foodsList.Count];
        for (int i = 0; i < foodsList.Count; i++)
        {
          viewIndex[i]=foodsList[i].GetComponent<FoodIngredient>().photonView.ViewID;
        }
        photonView.RPC("ClearFoods",RpcTarget.All,viewIndex);
    }
    [PunRPC]
    /// <summary>
    /// 定义一个清空食物的方法
    /// </summary>
    /// <param name="currPlate">要清空的盘子</param>
    private void ClearFoods(int[] ps)
    {
        //获取当前盘子的foodsList列表
        //foodsList = currPlate.GetComponent<PlateBehaviour>().foodsList;
        if (viewIndex.Length <= 0)
        {
            return;
        }
        //回收食材
        for (int i = 0; i < viewIndex.Length; i++)
        {
            ObjectPool.instance.RecycleObj(PhotonView.Find(i).gameObject);
        }
        //清空foodsList列表
        foodsList.Clear();
        //回收盘子
        ObjectPool.instance.RecycleObj(PhotonView.Find(photonView.ViewID).gameObject);
    }
    public Dictionary<string, List<FoodModel_FoodIngredient>> menu;

    //拿到菜谱 
    private void Menu()
    {
        foreach (var item in Levelins.levelFood)
        {
            List<FoodModel_FoodIngredient> CName = new List<FoodModel_FoodIngredient>();
            foreach (var i in item.Value.foodIngredient)
            {
                FoodModel_FoodIngredient temp = new FoodModel_FoodIngredient();
                temp.name = i.name;
                temp.state = i.state;

                CName.Add(temp);

            }
            menu.Add(item.Key, CName);
        }
    }
    /// <summary>
    /// 检测盘子状态
    /// </summary>
    public  void Check(PlateState state)
    {
        curstate = state;
    }

    /// <summary>
    /// 开始洗
    /// </summary>
    public void StartWash(Action a)
    {
        action = a;
        StartCoroutine("Clothes");
    }
    /// <summary>
    /// 停止操作
    /// </summary>
    [PunRPC]
    public void BreakOperat()
    {
        StopCoroutine("Clothes");
    }
    /// <summary>
    /// 洗
    /// </summary>
    /// <returns></returns>
    public IEnumerator Clothes()
    {
       
        while (true)
        {
            yield return new WaitForSeconds(speedTime);
            weshTime += speedTime;
            Debug.Log(weshTime);
            if (weshTime>currentTime)
            {
                curstate = PlateState.Clear;
                action();
               
                SetParents();
                weshTime = 0;
                break;
            }
        }
    }
    /// <summary>
    /// 洗碗设置父物体的点
    /// </summary>
    private void SetParents()
    {
        transform.SetParent(clearPoint);
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Prefabs/Materials/PlateMat");
    }
    /// <summary>
    /// 可以放到盘子里
    /// </summary>
    /// <returns></returns>
    public bool CanInPlate(FoodIngredient food,GameObject inPlateObj)
    {
        //判断状态
        if (curstate==PlateState.Dirty)
        {
            Debug.Log("藏盘子");
            return false;
        }
        Debug.Log(food.curState);
        if (food.curState == FoodIngredientState.Normal)
        {
            Debug.Log("状态不允许");
            return false;
        }
        if (transform.childCount > 0)
        {    //判断重复
            for (int i = 0; i < transform.childCount; i++)
            {
                if (food.GetIType() == transform.GetChild(i).GetComponent<FoodIngredient>().GetIType())
                {
                    Debug.Log("重复了");
                    return false;
                }
            }     
        }

        foreach (var item in menu)
        {
          
                //foodsList.Add(inPlateObj);
                //;
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i].name==food.GetIType().ToString()&&item.Value[i].state==food.curState.ToString())
                    {
                        return true;
                    }
                }
            

        }
        Debug.Log("其他错误");
            return false;

    }
 public  enum PlateState
    {
        Clear,
        Dirty
    }
    public bool Pick(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }

    public bool Throw(PlayerHandController player, Action<GameObject> callback)
    {
        return false;
    }

    public bool PutDown(PlayerHandController player, Action<GameObject> callback)
    {
        return true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
             //stream.SendNext(weshTime);

        }
        else
        {
           // weshTime = (int)stream.ReceiveNext();
        }
                
    }
}
