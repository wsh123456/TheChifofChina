using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddMenu : MonoBehaviour {
    public Transform tran;
    public Transform mterialTran;
    public Image food;
    public List<string> foodMenu=new List<string>();
    
    public List<GameObject> foodsList = new List<GameObject>();
    public List<string> menuDic;
    List<string> foodStr = new List<string>();//存放本关菜谱list<string>
    //List<FoodIngredientState> getMenuFoodIngredientType;

    public PlateBehaviour plateBehaviour;
    
    Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
    Dictionary<string, FoodIngredientModel> levelIngredient = new Dictionary<string, FoodIngredientModel>();
    Dictionary<string, string> foodIngredientStr;//菜的名字、状态的字典
    string menuStr;//菜单名字
    string foodSprite;//菜单贴图
    void Awake()
    {
        //menuStr = MenuManage.menuManage.RandomMenu();

        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        plateBehaviour = ObjectPool.instance.CreateObject(GameConst.Plate).GetComponent<PlateBehaviour>();
        //menuStr = MenuManage.menuManage.RandomMenu();
        
        tran = transform.Find("MenuPoint").transform;
        
        foodMenu = MenuManage.menuManage.foodMenu;
    }
    public void AddMenuPanel( out string menuStr )
    {
        AddMenuPanelFuc(out menuStr);
        foreach (string v in foodIngredientStr.Values)
        {
            Debug.Log(v);
        }

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
        //levelIngredient[foodStr[0]].foodIType
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
                //Debug.Log(v);
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
    /// <summary>
    /// 动态添加菜单中食物的Panel
    /// </summary>
    /// <param name="menuStr">当前成品菜的名字 </param>
    public void AddMenuPanelFuc(out string menuStr)
    {
        menuStr = MenuManage.menuManage.RandomMenu();//随机生成一个成菜
        MenuManage.menuManage.menuList.Add(menuStr);
        foodSprite = MenuManage.menuManage.FoodSprite(menuStr);
        foodStr.Add(menuStr);
        foodIngredientStr = GetFoodIngredientStr(foodIngredientStr, menuStr, levelFood);
        //Dictionary<string, FoodIngredientState> foodIngredientStr = new Dictionary<string, FoodIngredientState>();//存放每道菜的材料字典
        //foodIngredientStr = GetFoodIngredientStr(foodIngredientStr, menuStr, levelFood);
        //Debug.Log(foodIngredientStr);
        int count = levelFood[menuStr].foodIngredient.Count;//当前菜的配菜数量
        GameObject menu = Resources.Load<GameObject>("Prefabs/Menu");
        GameObject menuPrb = Instantiate(menu);    
        menuPrb.transform.SetParent(tran);
        menuPrb.transform.localScale = new Vector3(1, 1, 1);
        menuPrb.transform.localRotation = new Quaternion(0, 0, 0, 0);
        menuPrb.transform.localPosition = new Vector3(50, 0, 0);
        for (int i = 0; i < levelFood[menuStr].foodIngredient.Count; i++)
        {
           int sibling = menuPrb.transform.GetSiblingIndex();
          string meterialStr = levelFood[menuStr].foodIngredient[i].name;//菜材料的名字
            Debug.Log(meterialStr);
            //Debug.Log(sibling);
            AddMenuMterialUI(sibling,meterialStr);
        }
    }
    /// <summary>
    /// 动态添加菜单中材料的panel并加载贴图
    /// </summary>
    /// <param name="sibling">当前属于第几个菜 </param>
    /// <param name="meterialStr"> 菜材料的名字 </param>
    public void AddMenuMterialUI(int sibling, string meterialStr)
    {
        GameObject menu = Resources.Load<GameObject>("Prefabs/mterial");
        GameObject menuPrb = Instantiate(menu);
        Image meterial =menuPrb.transform.Find("materialPannel/material").GetComponent<Image>();
        mterialTran = tran.transform.GetChild(sibling).Find("materialPoint");
        //Debug.Log(mterialTran);
        //mterialTran = transform.Find("MenuPoint/Menu(Clone)/materialPoint").transform;
        menuPrb.transform.SetParent(mterialTran);
        menuPrb.transform.localScale = new Vector3(1, 1, 1);
        menuPrb.transform.localRotation = new Quaternion(0, 0, 0, 0);
        menuPrb.transform.localPosition = new Vector3(50, 0, 0);
        meterial.sprite = Resources.Load<Sprite>(levelIngredient[meterialStr].normalUI);
    }
    public void FoodIngredientToFindFood()
    {
        
    }
}

