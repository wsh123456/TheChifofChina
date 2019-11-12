using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 船移动路线脚本
/// </summary>
public class BoatMoveRoute : MonoBehaviour {
    private float waitTime = 0f;//每次等待时间
    private float moveTime = 0f;//每次移动时间
    private float moveSpeed ;//默认移动速度
    private Transform BoatFront;//前面小船
    private Transform BoatBehind;//后面小船

    Vector3 targetPos_BoateFront;
    Vector3 targetPos_BoateBehind;
    Vector3 starPos_BoateFront;
    Vector3 starPos_BoateBehind;
    private void Awake()
    {
        BoatFront = GameObject.Find("Design (merge)/Boat/Animated Objects/Raft Two").GetComponent<Transform>();
        BoatBehind = GameObject.Find("Design (merge)/Boat/Animated Objects/Raft One").GetComponent<Transform>();
        //前船和后船的初始位置
        starPos_BoateFront = BoatFront.localPosition;
        starPos_BoateBehind = BoatBehind.localPosition;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            //重置前船和后船的初始位置
            BoatFront.localPosition = starPos_BoateFront;
            BoatBehind.localPosition = starPos_BoateBehind;

            //移动速度
            moveSpeed = 0.35f;

            //等待5s开始第一次移动
            BoateStop(5f);
            yield return new WaitForSeconds(waitTime);

            //开始第一次移动
            //第一次移动时间
            moveTime = 8f;
            while (moveTime>=0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(-0.003f, 0f, -0.01f),Vector3.zero, moveSpeed);
                yield return new WaitForFixedUpdate();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            }

            //重新更改moveSpeed值
            moveSpeed = 0.325f;

            //等待20s开始第二次移动
            BoateStop(20f);
            yield return new WaitForSeconds(waitTime);

            //开始第二次移动
            //第二次移动时间
            moveTime = 6f;
            while (moveTime>=0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(-0.045f, 0f, 0.01f), new Vector3(0.03f, 0f, 0f), moveSpeed);
                yield return new WaitForFixedUpdate();
            }

            //等待10s开始第三次移动
            BoateStop(10f);
            yield return new WaitForSeconds(waitTime);

            //开始第三次移动
            //第三次移动时间
            moveTime = 6f;
            while (moveTime >= 0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(0f, 0f, -0.023f), new Vector3(0f, 0f, 0.025f), moveSpeed);
                yield return new WaitForFixedUpdate();
            }

            //等待6s开始第四次移动
            BoateStop(6f);
            yield return new WaitForSeconds(waitTime);

            //开始第四次移动
            //第四次移动时间
            moveTime = 3f;
            while (moveTime >= 0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(0.007f, 0f, 0f), new Vector3(-0.018f, 0f, 0f), moveSpeed);
                yield return new WaitForFixedUpdate();
            }

            //等待25s开始第五次移动
            BoateStop(25f);
            yield return new WaitForSeconds(waitTime);

            //开始第五次移动
            //第五次移动时间
            moveTime = 3f;
            while (moveTime >= 0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(-0.007f, 0f, 0f), new Vector3(0.018f, 0f, 0f), moveSpeed);
                yield return new WaitForFixedUpdate();
            }

            //等待11s开始第六次移动
            BoateStop(11f);
            yield return new WaitForSeconds(waitTime);

            //开始第六次移动
            //第六次移动时间
            moveTime = 6f;
            while (moveTime >= 0)
            {
                //前船和后船的移动
                BoateMove(new Vector3(0f, 0f, 0.023f), new Vector3(0f, 0f, -0.025f), moveSpeed);
                yield return new WaitForFixedUpdate();
            }

            //等待2s开始第七次移动
            BoateStop(2f);
            yield return new WaitForSeconds(waitTime);

            //更改速度
            moveSpeed = 0.5f;

            //开始第七次移动
            //第七次移动时间
            moveTime = 12f;
            while (moveTime >= 0)
            {
                //前船和后船移动距离
                Vector3 frontPos = -(BoatFront.localPosition + (-new Vector3(0.01f,0.01f,0.1f)));
                Vector3 behindPos = -(BoatBehind.localPosition + (-new Vector3(0, 0.01f, 0)));
                //前船和后船的移动
                BoateMove(frontPos, behindPos, moveSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    /// <summary>
    /// 船停止时等待的时间
    /// </summary>
    /// <param name="time"></param>
    private void BoateStop(float time)
    {
        waitTime = time;
    }

    /// <summary>
    /// 船的移动
    /// </summary>
    /// <param name="BoatFrontMoveDis">前船的移动距离</param>
    /// <param name="BoateBehindMoveDis">后船的移动距离</param>
    /// <param name="speed">移动速度</param>
    private void BoateMove(Vector3 BoatFrontMoveDis, Vector3 BoateBehindMoveDis,float speed)
    {
        //前船移动
        targetPos_BoateFront = BoatFront.localPosition + BoatFrontMoveDis;
        BoatFront.localPosition = Vector3.Lerp(BoatFront.localPosition, targetPos_BoateFront, Time.deltaTime * speed);
        //后船移动
        targetPos_BoateBehind = BoatBehind.localPosition + BoateBehindMoveDis;
        BoatBehind.localPosition = Vector3.Lerp(BoatBehind.localPosition, targetPos_BoateBehind, Time.deltaTime * speed);
        //倒计时
        moveTime = moveTime - Time.deltaTime;
    }
}
