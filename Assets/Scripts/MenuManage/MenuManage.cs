using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManage : MonoBehaviour {
    public static MenuManage menuManage;
    public int minMenuNum = 0;
    public int currentMenuNum = 0;
    public int maxMenuNum = 6;
    public Timer menuTimer;
    public Timer levelTimer;
    public Timer destoryFoodMenuTimer;
    public Dictionary<string, FoodModel> levelFood;
    public List<string> foodMenu;
    public List<Dictionary<string, string>> menu;
    public List<string> menuList = new List<string>();
    public  string menuStr;
    public float price=0;
    AddMenu addMenu;
    //public List<Timer> MenuTimer;
    bool isDestory = true;
    public bool isAddMenu = true;
    public bool isMenuDone = false;
    public bool isMenuAdd = true;
    public Image food;
    void Awake()
    {
        menuManage = this;
        foodMenu = new List<string>();
        levelFood = new Dictionary<string, FoodModel>();
        isMenuAdd = true;
        foodMenu = LevelInstance._instance.foodMenu;
        destoryFoodMenuTimer = TimerInstance.instance.destoryFoodMenuTimer;
        menuTimer = TimerInstance.instance.menuTimer;
        levelTimer = TimerInstance.instance.levelTimer;
        levelFood = LevelInstance._instance.levelFood;
        addMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<AddMenu>();
        menu = new List<Dictionary<string, string>>();
        //Debug.Log("awake");
        //foreach (KeyValuePair<string,FoodModel> item in levelFood)
        //{
        //    Debug.Log(item.Value.foodIngredient[0].state);
        //}
        //menuStr = RandomMenu();

    }
    void Start()
    {
        Initmenu();

    }
    void Update()
    {
        //Debug.Log(levelFood.Count);
        //Debug.Log(destoryFoodMenuTimer.GetTimeRemaining());
        //Debug.Log(levelTimer.GetRatioRemaining());
        //Debug.Log(menuTimer.GetTimeRemaining());
        //Debug.Log(levelFood);
        //Debug.Log(destoryFoodMenuTimer.GetTimeRemaining());
        //menuStr = RandomMenu();
        
        AddMenu();
    }
    public void Initmenu()
    {
        if (currentMenuNum == 0)
            for (int i = 0; i < 2; i++)
            {
                addMenu.AddMenuPanel(out menuStr);
                //menuUI.Add();//
                currentMenuNum += 1;
            }
    }
            
            //RandomMenu();  
            //RandomMenu();
            //Debug.Log("Init");
            //Debug.Log("+2道菜");
    public List<FoodIngredientType> GetFoodType(List<string>foodList,Dictionary<string, FoodIngredientModel> levelFood)
    {
        List<FoodIngredientType> foodType = new List<FoodIngredientType>();
        for (int i = 0; i < foodList.Count; i++)
        {
            foodType.Add( levelFood[foodList[i]].foodIType);
        }
        return foodType;
    }
   public void AddMenu()
    {
        if (menuTimer.GetTimeRemaining()<0.00000001f)
        {
            
            if (currentMenuNum == maxMenuNum)
            {
                isMenuDone = false;
               
            }
            else
            {
                isMenuDone = true;      
            }
            if (isMenuDone)
            {
                currentMenuNum += 1;
                //Debug.Log("AddMenuPanel");
                addMenu.AddMenuPanel(out menuStr);
                isMenuAdd = true;
                 
            }
            isMenuDone = false;
        }
        //Debug.Log("+2道菜");
        //Debug.Log(currentMenuNum);

    }
    public  string RandomMenu()
    {
        string menuStr="";
        int random = Random.Range(0,5);
        //Debug.Log(random);
        if (isMenuAdd)
        {
            //Debug.Log(foodMenu[RandomLevelFoodKey()]);
             menuStr = foodMenu[random];
              
        }
        //Debug.Log(menuStr
           // +"111111111111111111111111111111111111");
        return menuStr;
    }
    public string FoodSprite(string menuStr)
    {
       
        //Debug.Log(menuStr);
       string foodSprite = levelFood[menuStr].normalUI;
       
        return foodSprite;
    }
    
}

