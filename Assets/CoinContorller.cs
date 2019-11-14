using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinContorller : MonoBehaviour {
    Text coinNum;
    bool isCheck;
    AddMenu addMenu;
	// Use this for initialization
	void Start () {
        coinNum = transform.Find("Coin_banner/Coin_Number").GetComponent<Text>();
        addMenu = GameObject.FindWithTag("Canvas").GetComponent<AddMenu>();

    }
	
	// Update is called once per frame
	void Update () {
        coinNum.text = MenuManage.menuManage.price.ToString();
	}
    
}
