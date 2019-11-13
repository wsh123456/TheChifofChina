using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UseObjectPool : MonoBehaviour
{
    GameObject g1;
    GameObject g2;
    GameObject g3;
    PlateBehaviour pb;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //g1 = ObjectPool.instance.CreateObject(GameConst.Cube);
            //g3 = ObjectPool.instance.CreateObject(GameConst.Cube);
            g2 = ObjectPool.instance.CreateObject(GameConst.Plate);

            pb = g2.GetComponent<PlateBehaviour>();
            pb.isClean = false;
            pb.AddFoodInPlate(g1, g2);
            Debug.Log("-------------");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            pb.isClean = true;
            pb.AddFoodInPlate(g1, g2);
            pb.AddFoodInPlate(g3, g2);
            pb.GetFoodList();
            Debug.Log("-------------");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pb.ClearFoodsInPlate(g2);
            pb.GetFoodList();
            Debug.Log("清空");
            Debug.Log("-------------");
            pb.DestoryPlate(g2);
            Debug.Log("销毁盘子");
            Debug.Log("-------------");
        }
    }
}
