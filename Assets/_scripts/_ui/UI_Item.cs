using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour {

	private UI_Battle ui_Battle;

	void Awake ()
	{
		ui_Battle = GameObject.FindGameObjectWithTag ("UI").GetComponent<UI_Battle> ();
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}
		
	public void OnClick(){
		ui_Battle.SendMessage ("UseItem",gameObject.GetComponent<GridContainer>().bg);
	}
}
