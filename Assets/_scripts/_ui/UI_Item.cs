using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
	
	private GameObject itemInfo;
	private GlobalData gData;
	private Baggrid bg;
	public bool fromShop = false;
	private UI_Bag uiBag;

	void Awake ()
	{
		uiBag = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (delegate() {
			this.OnClick (); 
		});
	}
		
	public void OnClick ()
	{

		if (uiBag != null) {
			uiBag.SendMessage ("ClearFocus");
		}

		itemInfo = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("ItemInfo").gameObject;
		itemInfo.SetActive (true);
		gData.currentItem = bg;
		itemInfo.transform.FindChild ("Pic").GetComponent<Image> ().sprite = Resources.Load <Sprite> (bg.Item.prefabName);
		itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = bg.Item.note;

		Text buttonText = itemInfo.transform.FindChild ("UseButton").FindChild ("Text").GetComponent<Text> ();

		if (gData.isShop) {

			InputField tradeNum = itemInfo.transform.FindChild ("TradeNum").GetComponent<InputField> ();
			
			if (tradeNum) {
				tradeNum.text = "";
			}

			if (fromShop) {
				buttonText.text = StringCollection.BUY;
			} else {
				buttonText.text = StringCollection.SELL;
				tradeNum.text = bg.Num.ToString (); //默认最大数量
			}
		} else {

			//只有城市场景的商店模式需要这个对象
			if (itemInfo.transform.FindChild ("TradeNum") != null) {
				itemInfo.transform.FindChild ("TradeNum").gameObject.SetActive (false);
			}

			if (bg.Item.ct == (int)Item.CommonType.CONSUME) {
				buttonText.text = StringCollection.ITEMUSE;
			} else if (bg.Item.ct == (int)Item.CommonType.EQUIPMENT) {
				buttonText.text = StringCollection.ITEMEQUIP;
			} else if (bg.Item.ct == (int)Item.CommonType.MERCENARY) {
				buttonText.text = StringCollection.ADDTOTEAM;
			}
		}
	}

	public Baggrid Bg {
		get {
			return this.bg;
		}
		set {
			bg = value;
		}
	}

}
