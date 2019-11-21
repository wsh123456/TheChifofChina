using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;


public class MenuUI :MonoBehaviourPunCallbacks,IPunObservable{
    private Image cook;
    private Timer destoryFoodMenuTimer;
    private Image food;
    private Image meterial;
    private string foodSprite;
    private Dictionary<string, FoodModel> levelFood;
    private string menuStr;
    private string meterialStr;
    private AddMenu addMenu;
    private Dictionary<string, FoodIngredientModel> levelIngredient;

    private Transform materialPoint;

    void Awake() {
        materialPoint = transform.Find("materialPoint");
        food = transform.Find("FoodPoint/MenuFood/food"). GetComponent<Image>();
        levelFood = LevelInstance._instance.levelFood;
        levelIngredient = LevelInstance._instance.levelIngredient;
        addMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<AddMenu>();       
    }
        // Update is called once per frame
    void Start()
    {
        destoryFoodMenuTimer= Timer.Register(LevelInstance._instance.destoryFoodMenuTimer, null, null, false, true, null);
    }
    void Update ()
    {
        ReMoveMenu();
    }

    void ReMoveMenu()
    {
        if (destoryFoodMenuTimer.isDone){
            MenuManage.menuManage.currentMenuNum -= 1;
            destoryFoodMenuTimer.Cancel();
            ObjectPool.instance.RecycleObj(gameObject);
            MenuManage.menuManage.menuList.Remove(menuStr);
        }
    }


    #region PunRPC method

    [PunRPC]
    public void Init(int viewID, string menuStr){
        MenuManage.menuManage.menuList.Add(menuStr);

        // 读取菜的图片，记录现在这个是什么菜
        this.menuStr = MenuManage.menuManage.menuStr;
        foodSprite = levelFood[menuStr].normalUI;
        food.sprite = Resources.Load<Sprite>(foodSprite);
        transform.SetParent(PhotonView.Find(viewID).transform);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        transform.localPosition = new Vector3(50, 0, 0);

        // 设置子菜单
        for (int i = 0; i < levelFood[menuStr].foodIngredient.Count; i++)
        {
            string meterialStr = levelFood[menuStr].foodIngredient[i].name;//菜材料的名字
            AddMenuMterialUI(meterialStr);
        }
    }

    /// <summary>
    /// 添加成菜材料的UI
    /// </summary>
    /// <param name="meterialStr">成菜对应的字符串 </param>
    public void AddMenuMterialUI(string meterialStr)
    {
        GameObject menu = Resources.Load<GameObject>("Prefabs/mterial");
        GameObject menuPrb = Instantiate(menu);
        Image meterial =menuPrb.transform.Find("materialPannel/material").GetComponent<Image>();

        menuPrb.transform.SetParent(materialPoint);
        menuPrb.transform.localScale = new Vector3(1, 1, 1);
        menuPrb.transform.localRotation = new Quaternion(0, 0, 0, 0);
        menuPrb.transform.localPosition = new Vector3(50, 0, 0);
        meterial.sprite = Resources.Load<Sprite>(levelIngredient[meterialStr].normalUI);
    }



        
    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

    }
}

