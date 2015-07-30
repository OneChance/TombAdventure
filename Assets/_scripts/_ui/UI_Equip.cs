using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Equip : MonoBehaviour {

	public Equipment e;
	private GameObject itemInfo;
	private GlobalData gData;
	
	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}	

	public void OnClick(){

		if (gData.currentItem != null && gData.currentItem.Item.useable) {
			return;
		}

		itemInfo = GameObject.FindGameObjectWithTag("UI").transform.FindChild("ItemInfo").gameObject;
		itemInfo.SetActive (true);
		itemInfo.transform.FindChild("Pic").GetComponent<Image>().sprite = Resources.Load <Sprite>(e.prefabName);
		itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = e.note;
		Text buttonText = itemInfo.transform.FindChild("UseButton").FindChild("Text").GetComponent<Text>();
		buttonText.text = StringCollection.NOEQUIP;
		gData.currentItem = new Baggrid(e,1);
	}
}
