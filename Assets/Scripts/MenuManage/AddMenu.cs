using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddMenu : MonoBehaviour {
    public Transform tran;
    public Image food;
    public List<string> foodMenu;
    public List<string> menuList;
    public Dictionary<string, FoodModel> levelFood = new Dictionary<string, FoodModel>();
    Dictionary<string, FoodIngredientModel> levelIngredient = new Dictionary<string, FoodIngredientModel>();
    string meterialStr;
    string menuStr;
    string foodSprite;
    //string foodSprite;
    void Awake()
    {
        menuStr = MenuManage.menuManage.RandomMenu();
        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        //menuStr = MenuManage.menuManage.RandomMenu();
        foodSprite = MenuManage.menuManage.FoodSprite(menuStr);
        tran = transform.Find("MenuPoint").transform;
        meterialStr = levelFood[menuStr].foodIngredient[0].name;
        Debug.Log(meterialStr);
    }
    public void AddMenuPanel( out string menuStr )
    {
        //Debug.Log(LevelInstance._instance.levelFood);
        menuStr = MenuManage.menuManage.RandomMenu();
        //Debug.Log(levelFood[menuStr].foodIngredient.Count);
        //Debug.Log(levelFood.Values);
        //Debug.Log(levelFood[menuStr].foodIngredient.Count);
        Debug.Log("menuStr: " + menuStr);
        if (levelFood[menuStr].foodIngredient.Count == 1)
        {
            //Debug.Log(menuStr);
            Debug.Log("一个材料的菜" + menuStr);
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
            Debug.Log("俩个材料的菜" + menuStr);
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
    
}

