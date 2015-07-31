using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Login : MonoBehaviour
{

	private InputField username;
	private InputField password;
	private GlobalData gData;

	// Use this for initialization
	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Transform canvas = GameObject.FindGameObjectWithTag ("UI").transform;
		canvas.FindChild ("UsernameLable").GetComponent<Text> ().text = StringCollection.USERNAME;
		canvas.FindChild ("PasswordLable").GetComponent<Text> ().text = StringCollection.PASSWORD;
		canvas.FindChild ("Login").FindChild ("Text").GetComponent<Text> ().text = StringCollection.LOGIN;
		canvas.FindChild ("Reg").FindChild ("Text").GetComponent<Text> ().text = StringCollection.REG;


		username = canvas.FindChild ("Username").GetComponent<InputField> ();
		password = canvas.FindChild ("Password").GetComponent<InputField> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Login ()
	{
		if (username.text.Trim ().Equals ("")) {
			Debug.Log (StringCollection.NEEDUSERNAME);
			return;
		}

		if (password.text.Trim ().Equals ("")) {
			Debug.Log (StringCollection.NEEDPASSWORD);
			return;
		}

		bool vali = false;

		//去服务器验证
		vali = true;
		//如果成功
		if (vali) {

			bool haveInfo = true;

			if (haveInfo) {
				//如果有玩家信息,加载并跳转城市场景

				//从服务器端获取玩家数据初始化
				gData.isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
				gData.characterList = new List<Character> ();
				
				Equipment e = new Equipment (1, 2, 0, 0, Equipment.EquipPos.BODY, "2", "学者的思考", 1, 200);
				List<Equipment> eList = new List<Equipment> ();
				eList.Add (e);
				
				Character c = new Character (2000, 30, 100, 0, 0, "zhouhui", true, 200, 200, ProFactory.getPro ("Geomancer", "1"), 1, 0, eList, -1);
				
				HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂", 50);
				List<Baggrid> bgList = new List<Baggrid> ();
				Baggrid bg = new Baggrid (item, 2);
				bgList.Add (bg);
				
				//		Equipment e2 = new Equipment(2,3,Equipment.EquipPos.BODY,"3","学者的幻想",1,500);
				//		Baggrid bg2 = new Baggrid (e2, 1);
				//		bgList.Add (bg2);
				
				c.BgList = bgList;				
				gData.characterList.Add (c);				
				Character c2 = new Character (0, 40, 100, 0, 0, "unity", false, 100, 100, ProFactory.getPro ("Settler", "1"), 1, 0, null, -1);
				gData.characterList.Add (c2);

				Application.LoadLevel ("city");

			} else {
				//如果没有,跳转角色创建场景
				Application.LoadLevel ("create");
			}

		} else {
			Debug.Log (StringCollection.INVALIDACCOUNT);
		}
	}

	public void Reg ()
	{

	}
}
