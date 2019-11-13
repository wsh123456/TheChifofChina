#region 模块信息
// **********************************************************************
// 作者(Author):王舜华
// 版本号(Version):#VERSION#
// Unity版本	(Unity Version):2017.4.20f
// 创建时间(CreateTime):2019-11-8
// 修改者列表(modifier):无
// 模块描述(Module description):食材状态机
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;


public class FoodIngredientMachine{
    
    private FoodIState curState;    // 当前的状态
    private FoodIState targetState; // 目标状态

    // 设置状态
    public void SetState(FoodIngredient foodI, FoodIngredientState state){

        curState = EnumToState(state);
        foodI.curState = state;
    }

    /// <summary>
    /// 将foodI食材状态转换
    /// </summary>
    public bool ChangeState(FoodIngredient foodI, FoodIngredientState tState){
        curState = EnumToState(foodI.curState);
        targetState = EnumToState(tState);

        // 如果可以转换，则转换，否则不进行操作
        if(curState.CanChange(EnumToState(tState))){

            targetState.Changing(foodI, tState);
            return true;
        }
        return false;
    }


    /// <summary>
    /// 完成转换
    /// </summary>
    public void FinishChange(Action<FoodIngredientState> action){
        curState.ExitState(targetState);
        targetState.EnterState(action);
    }


    /// <summary>
    /// 枚举类型转换为FoodIState
    /// </summary>
    public static FoodIState EnumToState(FoodIngredientState state){
        Type t = Type.GetType("FoodIState_" + state.ToString());
        FieldInfo info = t.GetField("_ins", BindingFlags.Static | BindingFlags.Public);
        //获取字段内容信息
        //Debug.Log(info);
        //Debug.Log(info.Name);
        //Debug.Log(info.GetValue(info.Name));

        return info.GetValue(info.Name) as FoodIState;
    }
}


public class FoodIState
{
    protected List<FoodIState> canToState;      // 这个状态能够转换到什么状态
    public string stateName;        // 状态的名称
    public FoodIngredientState state;

    // 能否转换
    public virtual bool CanChange(FoodIState state){
        // 如果在可转换列表中有这个状态，返回true
        if(canToState.Contains(state)){
            return true;
        }
        return false;
    }

    // 进入这个状态
    public virtual void EnterState(Action<FoodIngredientState> action){
        //回调
        action(this.state);
    }

    // 转换过程
    public virtual void Changing(FoodIngredient foodI, FoodIngredientState tState){
        foodI.DoCurrentAction(tState);
    }        

    // 结束这个状态的提示
    public virtual void ExitState(FoodIState state){
        Debug.Log("由 " + stateName + " 转换为 " + state.stateName);
    }
    
    public virtual void BreakChange(){}      // 打断转换
}


public class FoodIState_Normal:FoodIState
{
    public readonly static FoodIState_Normal _ins = new FoodIState_Normal();
    private FoodIState_Normal(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_Cut._ins);
        canToState.Add(FoodIState_Poach._ins);
        canToState.Add(FoodIState_Break._ins);
        stateName = "初始状态";
        state = FoodIngredientState.Normal;
    }
}


public class FoodIState_NormalCanPlate:FoodIState
{
    public readonly static FoodIState_NormalCanPlate _ins = new FoodIState_NormalCanPlate();
    private FoodIState_NormalCanPlate(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_Cut._ins);
        canToState.Add(FoodIState_Poach._ins);
        canToState.Add(FoodIState_InPlate._ins);
        canToState.Add(FoodIState_Break._ins);
        stateName = "初始状态（可装盘）";
        state = FoodIngredientState.NormalCanPlate;
    }
}


public class FoodIState_Cut:FoodIState
{
    public readonly static FoodIState_Cut _ins = new FoodIState_Cut();
    private FoodIState_Cut(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_Poach._ins);
        canToState.Add(FoodIState_Fried._ins);
        canToState.Add(FoodIState_InPlate._ins);
        canToState.Add(FoodIState_Break._ins);
        stateName = "切块";
        state = FoodIngredientState.Cut;
    }
}


public class FoodIState_Fried:FoodIState
{
    public readonly static FoodIState_Fried _ins = new FoodIState_Fried();
    private FoodIState_Fried(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_InPlate._ins);
        canToState.Add(FoodIState_Break._ins);
        stateName = "炸";
        state = FoodIngredientState.Fried;
    }
}


public class FoodIState_Poach:FoodIState
{
    public readonly static FoodIState_Poach _ins = new FoodIState_Poach();
    private FoodIState_Poach(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_InPlate._ins);
        canToState.Add(FoodIState_Break._ins);
        stateName = "水煮";
        state = FoodIngredientState.Poach;
    }
}


public class FoodIState_InPlate:FoodIState
{
    public readonly static FoodIState_InPlate _ins = new FoodIState_InPlate();
    private FoodIState_InPlate(){
        canToState = new List<FoodIState>();
        canToState.Add(FoodIState_Break._ins);
        stateName = "装盘了";
        state = FoodIngredientState.InPlate;
    }
}


public class FoodIState_Break:FoodIState
{
    public readonly static FoodIState_Break _ins = new FoodIState_Break();
    private FoodIState_Break(){
        canToState = new List<FoodIState>();
        stateName = "毁坏了";
        state = FoodIngredientState.Break;
    }
}