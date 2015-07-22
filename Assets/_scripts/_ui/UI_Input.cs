using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Input : MonoBehaviour
{

	public Transform itemMenu;
	public Transform player;
	public GameObject bag;
	public GameObject itemInfo;
	public GameObject charInfo;
	public GameObject buttons;

	private GameObject avatar;
	private GameObject head;
	private GameObject hand;
	private GameObject clothes;
	private GameObject foot;
	private GameObject ass_1;
	private GameObject ass_2;
	private GameObject ass_3;
	private GameObject name;
	private GameObject proname;
	private GameObject health;
	private GameObject stamina;
	private GameObject strength;
	private GameObject archeology;
	private List<GameObject> assList;
	private GlobalData gData;
	private List<GameObject> focusList;
	
	void Start(){
		avatar = charInfo.transform.FindChild ("Avatar").gameObject;
		head = charInfo.transform.FindChild ("Head").gameObject;
		hand = charInfo.transform.FindChild ("Hand").gameObject;
		clothes = charInfo.transform.FindChild ("Clothes").gameObject;
		foot = charInfo.transform.FindChild ("Foot").gameObject;
		ass_1 = charInfo.transform.FindChild ("Ass_1").gameObject;
		ass_2 = charInfo.transform.FindChild ("Ass_2").gameObject;
		ass_3 = charInfo.transform.FindChild ("Ass_3").gameObject;
		name = charInfo.transform.FindChild ("Name").gameObject;
		proname = charInfo.transform.FindChild ("Proname").gameObject;
		health = charInfo.transform.FindChild ("Health").gameObject;
		stamina = charInfo.transform.FindChild ("Stamina").gameObject;
		strength = charInfo.transform.FindChild ("Strength").gameObject;
		archeology = charInfo.transform.FindChild ("Archeology").gameObject;

		assList = new List<GameObject> ();
		assList.Add (ass_1);
		assList.Add (ass_2);
		assList.Add (ass_3);
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		focusList = new List<GameObject>();
	}

	public void left ()
	{
		player.SendMessage ("PlayerMove",PlayerAction.MOVEDIRECTION.LEFT);
	}

	public void right ()
	{
		player.SendMessage ("PlayerMove",PlayerAction.MOVEDIRECTION.RIGHT);
	}

	public void up ()
	{
		player.SendMessage ("PlayerMove",PlayerAction.MOVEDIRECTION.UP);
	}

	public void down ()
	{
		player.SendMessage ("PlayerMove",PlayerAction.MOVEDIRECTION.DOWN);
	}

	void Update(){
		for (int i=0; i<focusList.Count; i++) {
			GameUtil.Focus(focusList [i]);
		}
	}

	public void UpdateUIInfo(){
		List<Character> cList = gData.characterList;

		//玩家角色信息
		stamina.GetComponent<Text>().text = cList[0].Stamina.ToString();
		health.GetComponent<Text>().text = cList[0].Health.ToString();
		strength.GetComponent<Text>().text = cList[0].strength.ToString();
		archeology.GetComponent<Text>().text = cList[0].archeology.ToString();
		//助手角色信息
		for(int i=1;i<cList.Count;i++){
				GameObject ass = assList[i-1];
				GameObject info = ass.transform.FindChild("Info").gameObject;
				info.transform.FindChild("S").GetComponent<Text>().text = cList[i].Stamina.ToString();
				info.transform.FindChild("H").GetComponent<Text>().text = cList[i].Health.ToString();
				info.transform.FindChild("Str").GetComponent<Text>().text = cList[i].strength.ToString();
				info.transform.FindChild("Arc").GetComponent<Text>().text = cList[i].archeology.ToString();
		}

	}

	void InitCharInfo(){
		List<Character> cList = gData.characterList;

		if(name.GetComponent<Text>().text.Equals("")){
			//玩家角色信息
			name.GetComponent<Text>().text = cList[0].ObjName;
			proname.GetComponent<Text>().text = cList[0].Pro.proname;

			//生成玩家
			GameObject uiCharP = Resources.Load ("UIChar", typeof(GameObject)) as GameObject;
			GameObject uiCharAvatar = Instantiate (uiCharP, new Vector3(avatar.transform.position.x,avatar.transform.position.y,0), Quaternion.identity) as GameObject;
			uiCharAvatar.GetComponent<Image>().sprite =  Resources.Load<Sprite>("_images/_game/"+cList[0].PrefabName);
			uiCharAvatar.GetComponent<UI_Player>().c = cList[0];
			uiCharAvatar.transform.SetParent(avatar.transform);

			charInfo.transform.FindChild ("StaminaLable").GetComponent<Text>().text = StringCollection.STAMINA;
			charInfo.transform.FindChild ("StrengthLable").GetComponent<Text>().text = StringCollection.STRENGTH;
			charInfo.transform.FindChild ("ArcheologyLable").GetComponent<Text>().text = StringCollection.ARCHEOLOGY;
			charInfo.transform.FindChild ("HealthLable").GetComponent<Text>().text = StringCollection.HEALTH;

			//加载装备

			//助手角色信息
			for(int i=1;i<cList.Count;i++){
				GameObject ass = assList[i-1];
				GameObject info = ass.transform.FindChild("Info").gameObject;
				info.SetActive(true);

				GameObject uiCharAss = Instantiate (uiCharP, new Vector3(ass.transform.position.x,ass.transform.position.y,0), Quaternion.identity) as GameObject;
				uiCharAss.GetComponent<Image>().sprite =  Resources.Load<Sprite>("_images/_game/"+cList[i].PrefabName);
				uiCharAss.transform.SetParent(ass.transform);
				uiCharAss.GetComponent<UI_Player>().c = cList[i];
				info.transform.FindChild("Name").GetComponent<Text>().text = cList[i].ObjName;
				info.transform.FindChild("Pro").GetComponent<Text>().text = cList[i].Pro.proname;

				info.transform.FindChild ("SL").GetComponent<Text>().text = StringCollection.STAMINA;
				info.transform.FindChild ("StrL").GetComponent<Text>().text = StringCollection.STRENGTH;
				info.transform.FindChild ("ArcL").GetComponent<Text>().text = StringCollection.ARCHEOLOGY;
				info.transform.FindChild ("HL").GetComponent<Text>().text = StringCollection.HEALTH;
			}

			UpdateUIInfo();
		}
	}

	public void Equip ()
	{
		charInfo.SetActive (!charInfo.activeInHierarchy);
		if (charInfo.activeInHierarchy) {
			InitCharInfo();
		}
	}

	public void Detect ()
	{
		if(gData.scenes[gData.currentFloor-1].isTomb){
			Debug.Log(StringCollection.ISTOMB);
		}else{
			player.SendMessage ("PlayerDetect");
		}
	}


	void closeBag(){
		itemInfo.SetActive (false);
		bag.SetActive (false);	
	}

	public void Dig ()
	{
		if(gData.scenes[gData.currentFloor-1].isTomb){
			Debug.Log(StringCollection.ISTOMB);
		}else{
			charInfo.SetActive (false);
			closeBag ();
			
			//隐藏UI上除了停止按钮意外的其他元素
			for(int i=0;i<buttons.transform.childCount;i++){
				if(buttons.transform.GetChild(i).name!="DigStop"){
					buttons.transform.GetChild(i).gameObject.SetActive(false);
				}else{
					buttons.transform.GetChild(i).gameObject.SetActive(true);
				}
			}
			
			player.SendMessage ("PlayerDig");
		}
	}

	public void DigStop(){
		//隐藏停止,显示UI上除了停止按钮意外的其他元素
		for(int i=0;i<buttons.transform.childCount;i++){
			if(buttons.transform.GetChild(i).name!="DigStop"){
				buttons.transform.GetChild(i).gameObject.SetActive(true);
			}else{
				buttons.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		player.SendMessage ("StopDig");
	}

	public void Item ()
	{
		itemInfo.SetActive (false);
		bag.SetActive (!bag.activeInHierarchy);
		Character currentC = player.GetComponent<PlayerAction>().characterList[0];
		if(bag.activeInHierarchy)
			bag.SendMessage ("InitBag",currentC);
	}

	public void UseItem(){
		//关闭背包窗口
		closeBag();
		//打开人物属性面板
		charInfo.SetActive(true);
		InitCharInfo();
		//获取道具信息
		Baggrid bg = gData.currentItem;
		Item item = bg.Item;

		if(item.ct == global::Item.CommonType.CONSUME){
			//人物格闪动
			focusList.Add(avatar);
			List<Character> cList = gData.characterList;
			for(int i=1;i<cList.Count;i++){
				GameObject ass = assList[i-1];
				focusList.Add(ass);
			}

			//如果是群体道具，直接应用，否则等待点击玩家后使用
			if(item.rt == global::Item.RangeType.MULTI){
				bg.Item.doSth(cList[0],cList);
				//用完减少数量并移除
				gData.currentItem.Num--;
				gData.currentItem = null;
				ItemUseComplete();
			}

		}else if(item.ct == global::Item.CommonType.EQUIPMENT){
			//装备格闪动
		}
	}

	public void ItemUseComplete(){
		focusList.Clear();
		//通知UI更新
		UpdateUIInfo();
	}
}
