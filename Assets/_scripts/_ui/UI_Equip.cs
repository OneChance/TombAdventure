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
		itemInfo = GameObject.FindGameObjectWithTag("UI").transform.FindChild("ItemInfo").gameObject;
		itemInfo.SetActive (true);
		itemInfo.transform.FindChild("Pic").GetComponent<Image>().sprite = Resources.Load <Sprite>("_images/_ui/"+e.prefabName);
		itemInfo.transform.FindChild ("ItemName").GetComponent<Text> ().text = e.name;
		itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = e.note;
		Text buttonText = itemInfo.transform.FindChild("UseButton").FindChild("Text").GetComponent<Text>();
		buttonText.text = StringCollection.NOEQUIP;
		gData.currentItem = new Baggrid(e,1);
	}
}
