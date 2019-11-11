// 编写人：王舜华
// 功能：使用Litjson对json文件进行读写

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class JsonFileControl{

	/// <summary>
	/// 读取所有关卡数据
	/// </summary>
	public static List<LevelModel> LoadLevelMessage(){
		StreamReader json;
		// 如果没有文件先创建文件
		try{
			json = File.OpenText(Application.dataPath + "/Data/LevelMessage.json");
		}catch{
			File.WriteAllText(Application.dataPath + "/Data/LevelMessage.json", "{}");
			json = File.OpenText(Application.dataPath + "/Data/LevelMessage.json");
		}
		string jsonText = json.ReadToEnd();
		json.Close();

		List<LevelModel> result = JsonMapper.ToObject<List<LevelModel>>(jsonText);
		return result;
	}


	/// <summary>
	/// 读取指定关卡数据
	/// </summary>
	public static LevelModel LoadLevelMessage(int i){
		StreamReader json;
		// 如果没有文件先创建文件
		try{
			json = File.OpenText(Application.dataPath + "/Data/LevelMessage.json");
		}catch{
			File.WriteAllText(Application.dataPath + "/Data/LevelMessage.json", "{}");
			json = File.OpenText(Application.dataPath + "/Data/LevelMessage.json");
		}
		string jsonText = json.ReadToEnd();
		json.Close();

		Dictionary<string, List<LevelModel>> resultList = JsonMapper.ToObject<Dictionary<string, List<LevelModel>>>(jsonText);
		return resultList["Level"][i-1];
	}


	// 读取本关菜单数据数据
	public static List<FoodModel> LoadFood(List<string> foodMenu){
		StreamReader json;
		// 如果没有文件先创建文件
		try{
			json = File.OpenText(Application.dataPath + "/Data/Foods.json");
		}catch{
			Debug.LogError("未找到：" + Application.dataPath + "/Data/Foods.json");
			return null;
		}
		string jsonText = json.ReadToEnd();
		json.Close();

		Dictionary<string, FoodModel> resultList = JsonMapper.ToObject<Dictionary<string, FoodModel>>(jsonText);

		List<FoodModel> result = new List<FoodModel>(); 
		foreach(var item in resultList){
			//Debug.Log(item.Value.foodIngredient[0].name);
			result.Add(item.Value);
		}
		return result;
	}
   

	/// <summary>
	/// 读取具体食材
	/// </summary>
	public static List<FoodIngredientModel> LoadFoodIngredient(List<string> ingredient){
		StreamReader json;
		try{
			json = File.OpenText(Application.dataPath + "/Data/FoodIngredient.json");
		}catch{
			Debug.LogError("未找到：" + Application.dataPath + "/Data/FoodIngredient.json");
			return null;
		}
		string jsonText = json.ReadToEnd();
		json.Close();

		Dictionary<string, FoodIngredientModel> resultList = JsonMapper.ToObject<Dictionary<string, FoodIngredientModel>>(jsonText);
		List<FoodIngredientModel> result = new List<FoodIngredientModel>();
		for(int i = 0; i < ingredient.Count; i++){
			try{
				result.Add(resultList[ingredient[i]]);
			}catch{
				Debug.LogError("未找到：" + ingredient[i]);
			}
		}
		return result;
	}


}




