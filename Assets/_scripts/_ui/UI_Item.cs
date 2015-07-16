using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour {
	
	private GameObject itemInfo;
	private GlobalData gData;
	public Baggrid bg;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}
		
	public void OnClick(){
		itemInfo = GameObject.FindGameObjectWithTag("BagContainer").transform.FindChild("ItemInfo").gameObject;
		itemInfo.SetActive (true);
		gData.currentItem = bg;
		itemInfo.transform.FindChild("Pic").GetComponent<Image>().sprite = Resources.Load <Sprite>("_images/_ui/"+bg.Item.prefabName);
		itemInfo.transform.FindChild ("ItemName").GetComponent<Text> ().text = bg.Item.name;
		itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = bg.Item.note;
	}
}
