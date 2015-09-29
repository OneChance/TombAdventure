using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DataHelper : MonoBehaviour
{
	public static List<Character> GetCharacterFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{

		List<Character> characterList = new List<Character> ();

		Dictionary<string, object> player = (Dictionary<string, object>)role ["info"];
		
		string name = (string)player ["name"];

		int money = int.Parse (((UInt32)role ["money"]).ToString ());

		string proname = player ["pro"].ToString ();
		int img = int.Parse (player ["img"].ToString ());

		Pro pro = ProFactory.getPro (proname, img.ToString ());

		List<Equipment> eList = getEquipsFromServer (role, siList);

		List<Baggrid> bgList = new List<Baggrid> ();

		Character c = new Character (money, 0, 0, 0, 0, 0, 0, name, true, 0, 0, pro, 0, 0, eList, -1);

		UpdatePlayerAttr (c, player);

		c.tombLogs = getAdventureLog (role);
		c.BgList = bgList;
		c.dbid = 1; //玩家是1,雇佣兵为2,3,4
		
		BagDataBind (c, role, siList);
	
		characterList.Add (c);

		getAssistsFromServer (role, siList, characterList);


		
		return characterList;
	}

	public static void UpdatePlayerAttr (Character c, Dictionary<string,object> playerInfo)
	{
		c.level = int.Parse (((UInt16)playerInfo ["level"]).ToString ());
		c.exp = int.Parse (((UInt16)playerInfo ["exp"]).ToString ());
		c.Health = int.Parse (((UInt16)playerInfo ["health"]).ToString ());
		c.MaxHealth = int.Parse (((UInt16)playerInfo ["maxhealth"]).ToString ());
		c.stamina = int.Parse (((UInt16)playerInfo ["stamina"]).ToString ());
		c.maxStamina = int.Parse (((UInt16)playerInfo ["maxstamina"]).ToString ());
		
		c.strength = int.Parse (((UInt16)playerInfo ["strength"]).ToString ());
		c.archeology = int.Parse (((UInt16)playerInfo ["archeology"]).ToString ());
		c.def = int.Parse (((UInt16)playerInfo ["def"]).ToString ());
		c.dodge = int.Parse (((UInt16)playerInfo ["dodge"]).ToString ());
	}

	public static Dictionary<string, object> BaseSceneInfoToServer (SceneInfo sceneInfo, int currentFloor)
	{
		Dictionary<string, object> scene = new Dictionary<string, object> ();
		
		List<object> blockData = transElementDataListFromClient (sceneInfo.blockData);
		List<object> itemData = transElementDataListFromClient (sceneInfo.itemData);
		List<object> enemyData = transElementDataListFromClient (sceneInfo.enemyData);

		scene.Add ("dbid", currentFloor);
		scene.Add ("grounds", blockData);
		scene.Add ("gitems", itemData);
		scene.Add ("genemys", enemyData);
		scene.Add ("gdigs", emptyElementDataList ());

		List<object> entrys = new List<object> ();

		ElementData nextEntry = sceneInfo.nextEntry;
		nextEntry.dbid = 1;

		entrys.Add (transElementDataFromClient (nextEntry));//nextentry
		entrys.Add (emptyElementData ());//preentry 这个变量原来是用于保存通往上一级的出口的,不过好像这个位置都是在0,0处，所以没有用了

		scene.Add ("entrys", entrys);

		scene.Add ("digtonextpos", emptyVector3 ());
		int isTomb = sceneInfo.isTomb ? 1 : 0;
		
		scene.Add ("istomb", isTomb);

		return scene;
	}

	public static Dictionary<int,TombLog> getAdventureLog (Dictionary<string, object> role)
	{

		Dictionary<int,TombLog> tombLogs = new Dictionary<int,TombLog> ();

		List<object> tombs = (List<object>)role ["tombs"];

		for (int i = 0; i < tombs.Count; i++) {
			TombLog tombLog = new TombLog ();
			Dictionary<string, object> tombInfo = (Dictionary<string, object>)tombs [i];
			tombLog.dbid = int.Parse (tombInfo ["dbid"].ToString ());

			List<object> floors = (List<object>)tombInfo ["floors"];

			List<SceneInfo> sceneinfos = new List<SceneInfo> ();

			for (int j = 0; j < floors.Count; j++) {
				Dictionary<string, object> floorInfo = (Dictionary<string, object>)floors [i];

				SceneInfo sceneInfo = new SceneInfo ();

				List<ElementData> blockData = transElementDataListFromServer ((List<object>)floorInfo ["grounds"]);
				List<ElementData> itemData = transElementDataListFromServer ((List<object>)floorInfo ["gitems"]);
				List<ElementData> enemyData = transElementDataListFromServer ((List<object>)floorInfo ["genemys"]);
				List<ElementData> digData = transElementDataListFromServer ((List<object>)floorInfo ["gdigs"]);

				sceneInfo.blockData = blockData;
				sceneInfo.itemData = itemData;
				sceneInfo.enemyData = enemyData;
				sceneInfo.digData = digData;

				List<object> entrys = (List<object>)floorInfo ["entrys"];

				sceneInfo.nextEntry = transElementDataFromServer (entrys [0]);
				sceneInfo.preEntry = transElementDataFromServer (entrys [1]);
				sceneInfo.digToNextPos = transVector3FromServer (floorInfo ["digtonextpos"]);

				int isTombInt = int.Parse (floorInfo ["istomb"].ToString ());

				sceneInfo.isTomb = (isTombInt == 0 ? false : true);

				sceneinfos.Add (sceneInfo);
			}

			tombLog.sceneinfos = sceneinfos;
			tombLogs.Add (tombLog.dbid, tombLog);
		}

		return tombLogs;
	}
	
	public static Vector3 transVector3FromServer (object v3)
	{
		Dictionary<string, object> pos = (Dictionary<string, object>)v3;
		return new Vector3 (float.Parse (pos ["x"].ToString ()), float.Parse (pos ["y"].ToString ()), float.Parse (pos ["z"].ToString ()));
	}

	public static Dictionary<string, object> emptyVector3 ()
	{
		Dictionary<string, object> v3d = new Dictionary<string, object> ();
		v3d.Add ("x", 0d);
		v3d.Add ("y", 0d);
		v3d.Add ("z", 0d);
		return v3d;
	}
	
	public static Dictionary<string, object> transVector3FromClient (Vector3 v3)
	{
		Dictionary<string, object> v3d = new Dictionary<string, object> ();

		v3d.Add ("x", v3.x);
		v3d.Add ("y", v3.y);
		v3d.Add ("z", v3.z);

		return v3d;
	}

	public static ElementData transElementDataFromServer (object elementServer)
	{

		ElementData element = new ElementData ();

		Dictionary<string, object> elementInfo = (Dictionary<string, object>)elementServer;
		element.dbid = int.Parse (elementInfo ["dbid"].ToString ());

		List<object> vecs = (List<object>)elementInfo ["vecs"];

		element.pos = transVector3FromServer (vecs [0]);
		element.objName = elementInfo ["objname"].ToString ();
		element.eulerAngles = transVector3FromServer (vecs [1]);
		element.order = int.Parse (elementInfo ["order"].ToString ());

		if (int.Parse (elementInfo ["dig_deep"].ToString ()) > 0) {

			DigData digData = new DigData (element);

			digData.deep = int.Parse (elementInfo ["dig_deep"].ToString ());
			digData.currentDeep = int.Parse (elementInfo ["dig_currentDeep"].ToString ());
			digData.texType = int.Parse (elementInfo ["dig_texture"].ToString ());

			element = digData;
		}

		return element;
	}

	public static Dictionary<string, object> emptyElementData ()
	{
		Dictionary<string, object> elementServer = new Dictionary<string, object> ();
		
		elementServer.Add ("dbid", 0);

		List<object> vecs = new List<object> ();

		vecs.Add (emptyVector3 ());
		vecs.Add (emptyVector3 ());

		elementServer.Add ("vecs", vecs);
		elementServer.Add ("objname", "");
		elementServer.Add ("order", 0);
		elementServer.Add ("dig_deep", 0);
		elementServer.Add ("dig_currentDeep", 0);
		elementServer.Add ("dig_texture", 0);
		
		return elementServer;
	}
	
	public static Dictionary<string, object> transElementDataFromClient (ElementData element)
	{
		Dictionary<string, object> elementServer = new Dictionary<string, object> ();

		elementServer.Add ("dbid", element.dbid);

		List<object> vecs = new List<object> ();
		
		vecs.Add (transVector3FromClient (element.pos));
		vecs.Add (transVector3FromClient (element.eulerAngles));

		elementServer.Add ("vecs", vecs);
		elementServer.Add ("objname", element.objName);
		elementServer.Add ("order", element.order);

		if (element is DigData) {
			DigData digData = (DigData)element;
			elementServer.Add ("dig_deep", digData.deep);
			elementServer.Add ("dig_currentDeep", digData.currentDeep);
			elementServer.Add ("dig_texture", digData.texType);
		} else {
			elementServer.Add ("dig_deep", 0);
			elementServer.Add ("dig_currentDeep", 0);
			elementServer.Add ("dig_texture", 0);
		}

		return elementServer;
	}
	
	public static List<ElementData> transElementDataListFromServer (List<object> elements)
	{
		List<ElementData> elementsData = new List<ElementData> ();

		for (int k=0; k<elements.Count; k++) {
			elementsData.Add (transElementDataFromServer (elements [k]));
		}

		return elementsData;
	}

	public static List<object> emptyElementDataList ()
	{
		List<object> elementsData = new List<object> ();
		
		return elementsData;
	}
	
	public static List<object> transElementDataListFromClient (List<ElementData> elements)
	{
		List<object> elementsData = new List<object> ();
		
		for (int k=0; k<elements.Count; k++) {
			ElementData e = elements [k];
			elementsData.Add (transElementDataFromClient (e));
		}
		
		return elementsData;
	}

	public static void getAssistsFromAssistList (List<object> assistL, Dictionary<int, ServerItemData> siList, List<Character> characterList)
	{

		for (int i = 0; i < assistL.Count; i++) {
			Dictionary<string, object> assistInfo = (Dictionary<string, object>)assistL [i];
			int iid = int.Parse (assistInfo ["iid"].ToString ());
			int dbid = int.Parse (assistInfo ["dbid"].ToString ());
			int player = int.Parse (assistInfo ["player"].ToString ());
			int onlineState = int.Parse (assistInfo ["onlinestate"].ToString ());
			
			ServerItemData assistProInfo = null;

			if (player == 0) {
				assistProInfo = siList [iid];
			} else {
				assistProInfo = new ServerItemData ();
				assistProInfo.pro = assistInfo ["playerpro"].ToString ();
				assistProInfo.prefabname = assistInfo ["iid"].ToString ();
				assistProInfo.name = assistInfo ["playername"].ToString ();
			}
			
			Pro pro = ProFactory.getPro (assistProInfo.pro, assistProInfo.prefabname);
			
			if (assistProInfo != null) {
				Character c = new Character (0, 0, 0, 0, 0, 0, 0, assistProInfo.name, false, 0, 0, pro, 0, 0, null, -1);

				UpdatePlayerAttr (c, assistInfo);
				
				c.dbid = dbid;
				c.iid = iid;

				if (player == 1) {
					c.IsOnLinePlayer = true;
					c.playerId = int.Parse(assistInfo ["playerid"].ToString ());

					if (onlineState == 1) {
						c.onLine = true;
					}
				}

				characterList.Add (c);
			}
		}
	}

	public static void getAssistsFromServer (Dictionary<string, object> role, Dictionary<int, ServerItemData> siList, List<Character> characterList)
	{
		List<object> assistL = (List<object>)role ["assists"];
		getAssistsFromAssistList (assistL, siList, characterList);
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

	public static void UpdateBag (Character c, List<object> bag, Dictionary<int, ServerItemData> siList)
	{

		c.BgList.Clear ();

		for (int i=0; i<bag.Count; i++) {
			
			Dictionary<string, object> bg_server = (Dictionary<string, object>)bag [i];
			
			int itemid = int.Parse (((UInt16)bg_server ["iid"]).ToString ());
			
			ServerItemData sid = siList [itemid];
			
			int num = int.Parse (bg_server ["num"].ToString ());

			int bgDBID = int.Parse (bg_server ["dbid"].ToString ());
			
			Baggrid bg = new Baggrid (ItemFactory.getItemFromSID (sid, bg_server), num, bgDBID);
			
			c.BgList.Add (bg);
		}
	}

	public static void BagDataBind (Character c, Dictionary<string, object> role, Dictionary<int, ServerItemData> siList)
	{

		List<object> bgList_server = (List<object>)role ["bggrids"];

		UpdateBag (c, bgList_server, siList);

		c.money = int.Parse (((UInt32)role ["money"]).ToString ());
	}

	public static List<Tomb> getTombInfoFromServer (List<object> tombs)
	{

		List<Tomb> tombList = new List<Tomb> ();
		for (int i=0; i<tombs.Count; i++) {
			Tomb tomb = new Tomb ();
			Dictionary<string, object> tombInfo = (Dictionary<string, object>)tombs [i];
			tomb.tombLevel = int.Parse (tombInfo ["level"].ToString ());
			tomb.tombName = tombInfo ["name"].ToString ();
			tomb.dbid = int.Parse (tombInfo ["dbid"].ToString ());
			tombList.Add (tomb);
		}
		return tombList;
	}

	public static void FillPlayerInfoRowTitle (GameObject panel)
	{
		panel.transform.FindChild ("NameLable").GetComponent<Text> ().text = StringCollection.NAME;
		panel.transform.FindChild ("Query").FindChild ("Text").GetComponent<Text> ().text = StringCollection.QUERYPLAYER;
		panel.transform.FindChild ("G_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Geomancer"];
		panel.transform.FindChild ("S_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Settler"];
		panel.transform.FindChild ("E_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Exorcist"];
		panel.transform.FindChild ("D_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Doctor"];
		
		Transform title = panel.transform.FindChild ("Rows").FindChild ("Title");	
		title.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Name"];
		title.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Pro"];
		title.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Level"];
	}

	public static List<object> getProListByChooseFromPanel (GameObject panel)
	{
		List<object> proList = new List<object> ();
		
		if (panel.transform.FindChild ("G_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Geomancer");
		}
		if (panel.transform.FindChild ("S_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Settler");
		}
		if (panel.transform.FindChild ("E_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Exorcist");
		}
		if (panel.transform.FindChild ("D_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Doctor");
		}
		
		return proList;
	}

}
