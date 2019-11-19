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
    private AddMenu addMenu;
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
        menu = new List<Dictionary<string, string>>();

        isMenuAdd = true;
        foodMenu = LevelInstance._instance.foodMenu;
        destoryFoodMenuTimer = TimerInstance.instance.destoryFoodMenuTimer;
        menuTimer = TimerInstance.instance.menuTimer;
        levelTimer = TimerInstance.instance.levelTimer;
        levelFood = LevelInstance._instance.levelFood;

        addMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameCanvasController>().foodMenu;
        
    }

    // 开始游戏，由房主计算出菜同步给其他玩家
    public void StartGame()
    {
        Initmenu();
    }


    void Update()
    {
        AddMenu();
    }


    public void Initmenu()
    {
        if (currentMenuNum == 0){
            for (int i = 0; i < 2; i++){
                addMenu.AddMenuPanel(out menuStr);
                currentMenuNum += 1;
            }
        }
    }

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
                addMenu.AddMenuPanel(out menuStr);
                isMenuAdd = true;
            }
            isMenuDone = false;
        }
    }


    public string RandomMenu()
    {
        // 房主执行

        string menuStr="";
        int random = Random.Range(0,5);
        //Debug.Log(random);
        if (isMenuAdd)
        {
             menuStr = foodMenu[random];
              
        }
        return menuStr;
    }


    public string FoodSprite(string menuStr)
    {
       
        //Debug.Log(menuStr);
       string foodSprite = levelFood[menuStr].normalUI;
       
        return foodSprite;
    }
    
}

