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
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            g2 = ObjectPool.instance.CreateObject(GameConst.Plate);
            g1 = ObjectPool.instance.CreateObject("Cube");
            g3 = ObjectPool.instance.CreateObject(GameConst.Plate);
            PlateBehaviour ph = new PlateBehaviour();
            ph.AddFoodInPlate(g1, g2);
            ph.AddFoodInPlate(g3, g2);

            for (int i = 0; i < ph.foodsList.Count; i++)
            {
                Debug.Log(ph.foodsList[i].name);
            }
            Debug.Log("-----------------");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            g2 = ObjectPool.instance.CreateObject(GameConst.Plate);
            g3 = ObjectPool.instance.CreateObject(GameConst.Plate);

            g2.GetComponent<PlateBehaviour>().isClean = false;
            g2.GetComponent<PlateBehaviour>().AddFoodInPlate(g3, g2);

            for (int i = 0; i < g2.GetComponent<PlateBehaviour>().foodsList.Count; i++)
            {
                Debug.Log(g2.GetComponent<PlateBehaviour>().foodsList[i].name);
            }
        }
    }


    
}
