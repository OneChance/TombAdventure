using KBEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UI_Login : MonoBehaviour
{

	private InputField username;
	private InputField password;
	private GlobalData gData;
    public Dictionary<UInt64, Dictionary<string, object>> roleList;

	// Use this for initialization
	void Start ()
	{
		gData = UnityEngine.GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Transform canvas = UnityEngine.GameObject.FindGameObjectWithTag ("UI").transform;
		canvas.FindChild ("UsernameLable").GetComponent<Text> ().text = StringCollection.USERNAME;
		canvas.FindChild ("PasswordLable").GetComponent<Text> ().text = StringCollection.PASSWORD;
		canvas.FindChild ("Login").FindChild ("Text").GetComponent<Text> ().text = StringCollection.LOGIN;
		canvas.FindChild ("Reg").FindChild ("Text").GetComponent<Text> ().text = StringCollection.REG;
		username = canvas.FindChild ("Username").GetComponent<InputField> ();
		password = canvas.FindChild ("Password").GetComponent<InputField> ();   	
	}

	public void Login ()
	{
		if (username.text.Trim ().Equals ("")) {
			ShowHint.Hint (StringCollection.NEEDUSERNAME);
			return;
		}

		if (password.text.Trim ().Equals ("")) {
			ShowHint.Hint (StringCollection.NEEDPASSWORD);
			return;
		}

		//去服务器验证
		KBEngine.Event.fireIn ("login", username.text.Trim (), password.text.Trim (), System.Text.Encoding.UTF8.GetBytes ("tomb_adventure"));
	}

	public void Reg ()
	{

	}

	public void ItemDown(Dictionary<UInt64, Dictionary<string, object>> itemList){

        Dictionary<int, ServerItemData> siList = new Dictionary<int, ServerItemData>();

        if (itemList != null && itemList.Count > 0)
        {
            //从服务器加载
            foreach (UInt64 key in itemList.Keys)
            {

                ServerItemData sid = new ServerItemData();

                Dictionary<string, object> info = itemList[key];

                int dbid = int.Parse(((UInt64)info["dbid"]).ToString());
                string name = (string)info["name"];
                int usetype = int.Parse(((UInt16)info["usetype"]).ToString());
                int rangetype = int.Parse(((UInt16)info["rangetype"]).ToString());
                int commontype = int.Parse(((UInt16)info["commontype"]).ToString());
                int objtype = int.Parse(((UInt16)info["objtype"]).ToString());
                string prefabname = (string)info["prefabname"];
                string note = (string)info["note"];
                string targetnote = (string)info["targetnote"];
                int price = int.Parse(((UInt16)info["price"]).ToString());
                int strength = int.Parse(((UInt16)info["strength"]).ToString());
                int archeology = int.Parse(((UInt16)info["archeology"]).ToString());
                int def = int.Parse(((UInt16)info["def"]).ToString());
                int dodge = int.Parse(((UInt16)info["dodge"]).ToString());
                int epos = int.Parse(((UInt16)info["epos"]).ToString());
                int level = int.Parse(((UInt16)info["level"]).ToString());
                int stamina = int.Parse(((UInt16)info["stamina"]).ToString());
                string pro = (string)info["pro"];
                int levelexpadd = int.Parse(((UInt16)info["levelexpadd"]).ToString());
                int attack = int.Parse(((UInt16)info["attack"]).ToString());
 
                sid.name = name;
                sid.usetype = usetype;
                sid.rangetype = rangetype;
                sid.commontype = commontype;
                sid.objtype = objtype;
                sid.prefabname = prefabname;
                sid.note = note;
                sid.targetnote = targetnote;
                sid.price = price;
                sid.strength = strength;
                sid.archeology = archeology;
                sid.def = def;
                sid.dodge = dodge;
                sid.epos = epos;
                sid.level = level;
                sid.stamina = stamina;
                sid.pro = pro;
                sid.levelexpadd = levelexpadd;
                sid.attack = attack;

                siList.Add(dbid, sid);
            }
        }
        else {
            //从本地加载
            
        }

        gData.siList = siList;

        if (roleList != null && roleList.Count > 0)
        {
            Dictionary<string, object> uinfo = roleList[0];

            Dictionary<string, object> playerInfo = (Dictionary<string, object>)uinfo["info"];

            string name = (string)playerInfo["name"];
            int iid = int.Parse(((UInt16)(playerInfo["iid"])).ToString());
            int level = int.Parse(((UInt16)(playerInfo["level"])).ToString());
            int exp = int.Parse(((UInt16)(playerInfo["exp"])).ToString());

            //根据iid去查询数据
            ServerItemData sid = siList[iid];

            //如果有玩家信息,加载并跳转城市场景

            //从服务器端获取玩家数据初始化
            gData.isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
            gData.characterList = new List<Character>();

            //Equipment e = new Equipment(1, 2, 0, 0, Equipment.EquipPos.BODY, "2", "学者的思考", 1, 200);
            List<Equipment> eList = new List<Equipment>();
            //eList.Add(e);

            Character c = new Character(2000, -1, -1, 0, 0, name, true, 50, 0, ProFactory.getPro(sid.pro, "1"), level, exp, eList, -1);

            //HealthItem item = new HealthItem(Item.RangeType.SINGLE, 10, "1", "单体治疗药剂", 50);
            List<Baggrid> bgList = new List<Baggrid>();
            //Baggrid bg = new Baggrid(item, 2);
            //bgList.Add(bg);

            //		Equipment e2 = new Equipment(2,3,Equipment.EquipPos.BODY,"3","学者的幻想",1,500);
            //		Baggrid bg2 = new Baggrid (e2, 1);
            //		bgList.Add (bg2);		
            c.BgList = bgList;
            gData.characterList.Add(c);
            //Character c2 = new Character(0, 40, 100, 0, 0, "unity", false, 100, 100, ProFactory.getPro("Settler", "1"), 1, 0, null, -1);
            //gData.characterList.Add(c2);

            Application.LoadLevel("city");
        }
        else
        {
            //如果没有,跳转角色创建场景
            Application.LoadLevel("create");
        }
    }
}
