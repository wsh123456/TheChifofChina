using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机、水、船移动脚本
/// </summary>
public class ViewMove : MonoBehaviour
{
    private Transform waterAndBoat;//水和船
    private Transform mainCamera;//主摄像机
    private const float speed = 0.0191f;//场景移动速度

    private void Awake()
    {
        //初始化组件
        waterAndBoat = GameObject.Find("Design1").GetComponent<Transform>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    private IEnumerator Start()
    {
        //摄像机、水、船移动距离
        Vector3 moveDis = new Vector3(-1.9f, 0, 0);
        while (true)
        {
            yield return new WaitForFixedUpdate();

            //摄像机、水、船移移动
            waterAndBoat.localPosition += moveDis * speed;
            mainCamera.localPosition += moveDis * speed;
        }
    }
}
