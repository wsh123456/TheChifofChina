using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
using Photon.Realtime;


public class AddMenu : MonoBehaviourPunCallbacks,IPunObservable{
    public List<MenuUI> foodMenu=new List<MenuUI>();
    
    public PlateBehaviour plateBehaviour;
    
    Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
    // Dictionary<string, FoodIngredientModel> levelIngredient = new Dictionary<string, FoodIngredientModel>();
    void Awake()
    {
        levelFood = LevelInstance._instance.levelFood;
       // plateBehaviour = ObjectPool.instance.CreateObject(GameConst.Plate).GetComponent<PlateBehaviour>();
    }

    private void Start() {
    }

    // 添加菜单
    public void AddMenuPanel(out string menuStr)
    {
        if(!PhotonNetwork.IsMasterClient){
            menuStr = "";
            return;
        }
        menuStr = MenuManage.menuManage.RandomMenu();//随机生成一个成菜

        MenuUI menuPrb = PhotonNetwork.Instantiate("Prefabs/Menu", Vector3.zero, Quaternion.identity).GetComponent<MenuUI>();
        menuPrb.photonView.RPC("Init", RpcTarget.All, photonView.ViewID, menuStr);
    }


    /// <summary>
    /// 得到盘子食材的状态
    /// </summary>
    /// <param name="foodPlateIngredientType"> 盘子食材的状态 </param>
    /// <param name="plate"> 盘子</param>
    /// <returns></returns>
    public List<string> GetPlaneFoodIngredientType(List<string>foodPlateIngredientType,GameObject plate)
    {
        PlateBehaviour pb= plate.GetComponent<PlateBehaviour>();
       
        foodPlateIngredientType = new List<string>();
        for (int i = 0; i < pb.foodsList.Count; i++)
        {
            foodPlateIngredientType.Add(pb.foodsList[i].GetComponent<FoodIngredient>().previousState.ToString());
        }
        return foodPlateIngredientType;
    }


    /// <summary>
    /// 获取当前菜材料的名字
    /// </summary>
    /// <param name="foodIngredientStr"></param>
    /// <param name="menuStr">成菜名字 </param>
    /// <param name="foodStr"> </param>
    /// <param name="levelFood"></param>
    /// <returns></returns>    
    public Dictionary<string, string> GetFoodIngredientStr(Dictionary<string, string> foodIngredientStr,string menuStr,  Dictionary<string, FoodModel> levelFood )
    {
        foodIngredientStr = new Dictionary<string, string>();
        for (int i = 0; i < levelFood[menuStr].foodIngredient.Count; i++)
        {
            foodIngredientStr.Add(levelFood[menuStr].foodIngredient[i].name, levelFood[menuStr].foodIngredient[i].state);
        }
        return foodIngredientStr;
    }


    /// <summary>
    /// 对比菜单中的状态和盘子中食物的状态
    /// </summary>
    /// <param name="foodIngredientStr">菜的名字 </param>
    /// <param name="foodStr"> 本关食材 </param>
    /// <param name="foodPlateIngredientType"> 食物盘子食材状态 </param>
    /// <returns></returns>
    public void CheckMenuPlateStates(string menuStr, Dictionary<string, string> foodIngredientStr,  List<string> foodPlateIngredientType,GameObject plate)
    {
        PlateBehaviour pb = plate.GetComponent<PlateBehaviour>();
        for (int i = 0; i < foodPlateIngredientType.Count; i++)
        {
            foreach (string v in foodIngredientStr.Values)
            {
                if (v == foodPlateIngredientType[i])
                {
                    foreach (KeyValuePair<string, FoodModel> item in levelFood)
                    {
                        if(item.Value.foodIngredient[i].name == pb.foodsList[i].name)
                        {
                            if (MenuManage.menuManage.menuList!=null)
                            {
                                MenuManage.menuManage.price += item.Value.price;
                                MenuManage.menuManage.menuList.Remove(menuStr);
                            }
                        }
                    }
                }
            }
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
    }
}

