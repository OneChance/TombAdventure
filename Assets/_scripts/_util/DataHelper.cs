using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DataHelper : MonoBehaviour
{
	public static List<Character> GetCharacterFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{

		List<Character> characterList = new List<Character>();

		Dictionary<string, object> player = (Dictionary<string, object>)role ["info"];
		
		string name = (string)player ["name"];
		int proid = int.Parse (((UInt16)player ["iid"]).ToString ());
		int money = int.Parse (((UInt32)role ["money"]).ToString ());
		int level = int.Parse (((UInt16)player ["level"]).ToString ());
		int exp = int.Parse (((UInt16)player ["exp"]).ToString ());
		int health = int.Parse (((UInt16)player ["health"]).ToString ());
		int stamina = int.Parse (((UInt16)player ["stamina"]).ToString ());

		Pro pro = ProFactory.getProById (proid, "1");

		List<Equipment> eList = new List<Equipment> ();
		List<object> equipL = (List<object>)role ["equips"];
		for (int i = 0; i < equipL.Count; i++) {
			Dictionary<string, object> info = (Dictionary<string, object>)equipL [i];
			int eid = int.Parse (((UInt16)info ["eid"]).ToString ());
			ServerItemData equipInfo = siList [eid];
			if (equipInfo != null) {
				Equipment e = new Equipment (equipInfo.strength, equipInfo.archeology, equipInfo.def, equipInfo.dodge, Equipment.getPosByIndex (equipInfo.epos), eid.ToString (), equipInfo.name, equipInfo.level, equipInfo.price);
				eList.Add (e);
			}
		}

		List<Baggrid> bgList = new List<Baggrid> ();

		Character c = new Character (money, health, 0, 0, 0, name, true, 0, 0, pro, level, exp, eList, -1);
		
		c.BgList = bgList;	
	
		characterList.Add(c);

		return characterList;
	}	
}
