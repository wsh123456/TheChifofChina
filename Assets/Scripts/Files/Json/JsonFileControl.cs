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
	public static List<FoodModel> LoadFoodIngredient(List<string> foodMenu){
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

		Debug.Log(jsonText);
		Dictionary<string, FoodModel> resultList = JsonMapper.ToObject<Dictionary<string, FoodModel>>(jsonText);

		List<FoodModel> result = new List<FoodModel>(); 
		foreach(var item in resultList){
			Debug.Log(item.Value.foodIngredient[0].name);
			result.Add(item.Value);
		}
		return result;
	}


	// 获取点上的数据
	// private PointJsonModel GetPointData(MovePoint p, Road road, int index){
	// 	PointJsonModel point = new PointJsonModel();
	// 	point.point_street = (int)road;
	// 	point.point_index = index;

	// 	try{
	// 		BuildingBase building = (p as Point_Build).GetBuilding();

	// 		// Debug.Log((p as Point_Build).building);
	// 		// if(building != ){
	// 		HouseJsonModel house = new HouseJsonModel();
	// 		house.type = (int)building.GetBuildingType();
	// 		house.level = building.currentLevel;
	// 		house.belong = gameController.playerList.IndexOf(building.owner);
	// 		house.state = (int)building.state;
	// 		point.point_house = house;
	// 		// }
	// 	}catch{}

	// 	StageJsonModel stage = new StageJsonModel();
	// 	// stage.type = "qdqf0";
	// 	point.point_stage = stage;

	// 	GodJsonModel god = new GodJsonModel();
	// 	// god.type = "asdqwd";
	// 	// god.left_days = "dqef";
	// 	point.point_god = god;

	// 	return point;
	// }
}




