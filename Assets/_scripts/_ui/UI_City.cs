using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_City : MonoBehaviour
{
	public GameObject shopBag;
	public GameObject playerBag;
	public GameObject itemInfo;
	public GameObject charInfo;
	private GlobalData gData;
	private Transform buttons;
	private GameObject exitButton;

	void Start ()
	{
		//初始化文本
		exitButton = GameObject.FindGameObjectWithTag("UI").transform.FindChild("Exit").gameObject;
		buttons = GameObject.FindGameObjectWithTag("UI").transform.FindChild("Buttons");
		buttons.FindChild("Bag_B").FindChild("Text").GetComponent<Text>().text = StringCollection.BAG;
		buttons.FindChild("Equip_B").FindChild("Text").GetComponent<Text>().text = StringCollection.EQUIPMENT;
		GameObject.FindGameObjectWithTag("UI").transform.FindChild("ShopBag").FindChild("Leave").FindChild("Text").GetComponent<Text>().text = StringCollection.LEAVESHOP;
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
	}

	public void openShop(string shopType){
		shopBag.SetActive(true);
		shopBag.SendMessage("InitBag",shopType);

		playerBag.SetActive(true);
		playerBag.SendMessage("InitBag",gData.characterList[0]);

		//进入商店模式,隐藏其他按钮
		buttons.gameObject.SetActive(false);
		exitButton.SetActive(false);
		gData.isShop = true;
	}

	public void PlayerBag(){
		playerBag.SetActive(!playerBag.activeInHierarchy);
		if(playerBag.activeInHierarchy){
			playerBag.SendMessage("InitBag",gData.characterList[0]);
		}else{
			itemInfo.SetActive(false);
		}
	}

	public void PlayerEquip(){
		charInfo.SetActive(!charInfo.activeInHierarchy);
		if(!charInfo.activeInHierarchy){
			itemInfo.SetActive(false);
		}else{
			gameObject.SendMessage("InitCharInfo");
		}
	}

	public void leaveShop(){
		shopBag.SetActive(false);
		playerBag.SetActive(false);
		itemInfo.SetActive(false);
		//离开商店模式,显示其他按钮
		buttons.gameObject.SetActive(true);
		exitButton.SetActive(true);
		gData.isShop = false;
	}

	public void WorldMap(){
		DontDestroyOnLoad (gData);
		Application.LoadLevel ("map");
	}
}
