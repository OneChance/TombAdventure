using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DataHelper : MonoBehaviour
{
	public static List<Character> GetCharacterFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{

		List<Character> characterList = new List<Character> ();

		Dictionary<string, object> player = (Dictionary<string, object>)role ["info"];
		
		string name = (string)player ["name"];

		int money = int.Parse (((UInt32)role ["money"]).ToString ());
		int level = int.Parse (((UInt16)player ["level"]).ToString ());
		int exp = int.Parse (((UInt16)player ["exp"]).ToString ());
		int health = int.Parse (((UInt16)player ["health"]).ToString ());
		int maxhealth = int.Parse (((UInt16)player ["maxhealth"]).ToString ());
		int stamina = int.Parse (((UInt16)player ["stamina"]).ToString ());
		int maxstamina = int.Parse (((UInt16)player ["maxstamina"]).ToString ());

		int strength = int.Parse (((UInt16)player ["strength"]).ToString ());
		int archeology = int.Parse (((UInt16)player ["archeology"]).ToString ());
		int def = int.Parse (((UInt16)player ["def"]).ToString ());
		int dodge = int.Parse (((UInt16)player ["dodge"]).ToString ());

		string proname = player ["pro"].ToString ();
		int img = int.Parse (player ["img"].ToString ());

		Pro pro = ProFactory.getPro (proname, img.ToString ());

		List<Equipment> eList = getEquipsFromServer (role, siList);

		List<Baggrid> bgList = new List<Baggrid> ();

		Character c = new Character (money, health, maxhealth, strength, archeology, def, dodge, name, true, stamina, maxstamina, pro, level, exp, eList, -1);

		c.BgList = bgList;
		c.dbid = 1; //玩家是1,雇佣兵为2,3,4
		
		BagDataBind (c, role, siList);
	
		characterList.Add (c);

		getAssistsFromServer (role, siList, characterList);

		return characterList;
	}

	public static void getAssistsFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList, List<Character> characterList)
	{
		List<object> assistL = (List<object>)role ["assists"];
		for (int i = 0; i < assistL.Count; i++) {
			Dictionary<string, object> assistInfo = (Dictionary<string, object>)assistL [i];
			int iid = int.Parse (assistInfo ["iid"].ToString ());
			int dbid = int.Parse (assistInfo ["dbid"].ToString ());
			int level = int.Parse (assistInfo ["level"].ToString ());
			int commontype = int.Parse (assistInfo ["commontype"].ToString ());
			int stamina = int.Parse (assistInfo ["stamina"].ToString ());
			int maxstamina = int.Parse (assistInfo ["maxstamina"].ToString ());
			int health = int.Parse (assistInfo ["health"].ToString ());
			int maxhealth = int.Parse (assistInfo ["maxhealth"].ToString ());
			int strength = int.Parse (assistInfo ["strength"].ToString ());
			int archeology = int.Parse (assistInfo ["archeology"].ToString ());
			int def = int.Parse (assistInfo ["def"].ToString ());
			int dodge = int.Parse (assistInfo ["dodge"].ToString ());
			int exp = int.Parse (assistInfo ["exp"].ToString ());
			int attack = int.Parse (assistInfo ["attack"].ToString ());

			ServerItemData assistProInfo = siList [iid];

			Pro pro = ProFactory.getPro (assistProInfo.pro, assistProInfo.prefabname);

			if (assistProInfo != null) {
				Character c = new Character (0, health, maxhealth, strength, archeology, def, dodge, assistProInfo.name, false, stamina, maxstamina, pro, level, exp, null, -1);
				c.dbid = dbid;
				c.iid = iid;
				characterList.Add (c);
			}
		}
	}

	public static List<Equipment> getEquipsFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{
		List<Equipment> eList = new List<Equipment> ();
		List<object> equipL = (List<object>)role ["equips"];
		for (int i = 0; i < equipL.Count; i++) {
			Dictionary<string, object> info = (Dictionary<string, object>)equipL [i];
			int iid = int.Parse (((UInt16)info ["iid"]).ToString ());
			int dbid = int.Parse (info ["dbid"].ToString ());
			ServerItemData equipInfo = siList [iid];
			if (equipInfo != null) {
				Equipment e = new Equipment (equipInfo.strength, equipInfo.archeology, equipInfo.def, equipInfo.dodge, equipInfo.epos, iid.ToString (), equipInfo.name, equipInfo.level, equipInfo.price, equipInfo.health, equipInfo.stamina);
				e.dbid = dbid;
				eList.Add (e);
			}
		}
		return eList;
	}

	public static void BagDataBind (Character c, Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{

		List<object> bgList_server = (List<object>)role ["bggrids"];

		c.BgList.Clear ();
		
		for (int i=0; i<bgList_server.Count; i++) {
			
			Dictionary<string, object> bg_server = (Dictionary<string, object>)bgList_server [i];
			
			int itemid = int.Parse (((UInt16)bg_server ["iid"]).ToString ());

			ServerItemData sid = siList [itemid];
			
			int num = int.Parse (((UInt16)bg_server ["num"]).ToString ());

			int bgDBID = int.Parse (bg_server ["dbid"].ToString ());

			Baggrid bg = new Baggrid (ItemFactory.getItemFromSID (sid, bg_server), num, bgDBID);
			
			c.BgList.Add (bg);
		}

		c.money = int.Parse (((UInt32)role ["money"]).ToString ());
	}
}
