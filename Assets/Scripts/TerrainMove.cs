using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形移动脚本
/// </summary>
public class TerrainMove : MonoBehaviour
{
    private Transform sceneLeft;//左边场景
    private Transform sceneRight;//右边场景
    private float speed = 0.0191f;//场景移动速度
    private Vector3 targetPos;//场景移动的目标地点
    private Vector3 targetResetPos;//场景重置的地点
    private Transform target;//要重置的目标场景

    private void Awake()
    {
        sceneLeft = GameObject.FindWithTag("left").GetComponent<Transform>();
        sceneRight = GameObject.Find("right").GetComponent<Transform>();
        //场景移动的终点
        targetPos =new Vector3(89,1.7f,-1.5f);
        //场景重置的地点
        targetResetPos = new Vector3(-145f,1.7f,-1.5f);
        //默认先更改右边的场景
        target = sceneRight;
    }

    private IEnumerator Start()
    {
        //地形移动距离
        Vector3 moveDis = new Vector3(0.01f, 0, 0);
        while (true)
        {
            yield return new WaitForFixedUpdate();

            //场景移动
            sceneLeft.localPosition += moveDis * speed;//0.01*0.0191
            sceneRight.localPosition += moveDis * speed;

            //判断目标场景距离终点的距离
            if (Mathf.Abs(Vector3.Distance(target.position,targetPos))<0.1)
            {
                //重置目标场景的位置
                target.position= targetResetPos;
                //切换目标
                if (target == sceneRight)
                {
                    target = sceneLeft;
                }
                else
                {
                    target = sceneRight;
                }
            }
        }
    }
}
