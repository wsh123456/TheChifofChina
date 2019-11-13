using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddMenu : MonoBehaviour {
    public Transform tran;
    public Image food;
    public List<string> foodMenu=new List<string>();
    public List<string> menuList;
    public PlateBehaviour plateBehaviour;
    public List<GameObject> foodsList = new List<GameObject>();
    public Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
    Dictionary<string, FoodIngredientModel> levelIngredient = new Dictionary<string, FoodIngredientModel>();
    List<string> foodIngredientStr = new List<string>();//存放每道菜的材料list<string>
    List<string> foodStr = new List<string>();//存放本关菜谱list<string>
    List<FoodIngredientState> getMenuFoodIngredientType = new List<FoodIngredientState>();
    string menuStr;
    string foodSprite;
    //string foodSprite;
    void Awake()
    {
        menuStr = MenuManage.menuManage.RandomMenu();
        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        plateBehaviour = ObjectPool.instance.CreateObject(GameConst.Plate).GetComponent<PlateBehaviour>();
        //menuStr = MenuManage.menuManage.RandomMenu();
        foodSprite = MenuManage.menuManage.FoodSprite(menuStr);
        tran = transform.Find("MenuPoint").transform;
        foodMenu = MenuManage.menuManage.foodMenu;
        //foreach (KeyValuePair<string, FoodIngredientModel> item in levelIngredient)
        //{
        //    Debug.Log(item.Key+ "食材");
        //}
        //foreach (KeyValuePair<string, FoodModel> item in levelFood)
        //{
        //    Debug.Log(item.Key);
        //}
        //Debug.Log(levelFood["FriedChickenChips"].foodIngredient.Count);
        //for (int i = 0; i < foodIngredient.Count; i++)
        //{
        //    Debug.Log(foodIngredient[i]);
        //}
        //Debug.Log(meterialStr);
    }
    public void AddMenuPanel( out string menuStr )
    {
        
        //Debug.Log(LevelInstance._instance.levelFood);
        menuStr = MenuManage.menuManage.RandomMenu();
        //Debug.Log(levelFood[menuStr].foodIngredient.Count);
        //Debug.Log(levelFood.Values);
        //Debug.Log(levelFood[menuStr].foodIngredient.Count);
        //Debug.Log("menuStr: " + menuStr);
        foodStr.Add(menuStr);
        foodIngredientStr= GetFoodIngredientStr(menuStr, foodStr, levelFood);
        foreach (var item in foodIngredientStr)
        {
            Debug.Log(item);
        }
        
        //getMenuFoodIngredientType = GetMenuFoodIngredientType(foodIngredientStr, levelIngredient);
        //foreach (var item in getMenuFoodIngredientType)
        //{
        //    Debug.Log(item);
        //}
        //Debug.Log(foodStr[0]);
        //  Debug.Log(levelFood[foodStr[0]].foodIngredient[0].name);
        if (levelFood[menuStr].foodIngredient.Count == 1)
        {
            //Debug.Log(menuStr);
            //Debug.Log("一个材料的菜" + menuStr);
            GameObject menu = Resources.Load<GameObject>("Prefabs/OneIngredient");
            GameObject menuPrb = Instantiate(menu);
            menuPrb.transform.SetParent(tran);
            menuPrb.transform.localScale = new Vector3(1, 1, 1);
            menuPrb.transform.localRotation = new Quaternion(0, 0, 0, 0);
            menuPrb.transform.localPosition = new Vector3(50, 0, 0);
            //menuStr = ""; 
        }
        if (levelFood[menuStr].foodIngredient.Count == 2)
        {
            //Debug.Log("俩个材料的菜" + menuStr);
            //menuStr = MenuManage.menuManage.RandomMenu();
            //Debug.Log(menuStr);
            GameObject menu = Resources.Load<GameObject>("Prefabs/TwoIngredient");
            //Debug.Log(menuTwo);
            GameObject menuPrb = Instantiate(menu);
            menuPrb.transform.SetParent(tran);
            menuPrb.transform.localScale = new Vector3(1, 1, 1);
            menuPrb.transform.localRotation = new Quaternion(0, 0, 0, 0);
            menuPrb.transform.localPosition = new Vector3(50, 0, 0);
            //menuStr = "";
            
        }
        
    }
    public List<FoodIngredientState> GetMenuFoodIngredientType(List<string> foodIngredientStr,Dictionary<string, FoodIngredientModel> levelIngredient)
    {
        List<FoodIngredientState> foodIngredientType = new List<FoodIngredientState>();
        //levelIngredient[foodStr[0]].foodIType
        for (int i = 0; i < foodIngredientStr.Count; i++)
        {
            //foodIngredientType.Add(levelIngredient[foodIngredientStr[i]].);
        }
        return foodIngredientType;
    }
    public List<string> GetFoodIngredientStr(string menuStr, List<string> foodStr, Dictionary<string, FoodModel> levelFood)
    {
        List<string> foodingredientstr = new List<string>();
        for (int i = 0; i < levelFood[menuStr].foodIngredient.Count; i++)
        {
            foodingredientstr.Add(levelFood[menuStr].foodIngredient[i].name);
        }
        return foodingredientstr;
    }

}

