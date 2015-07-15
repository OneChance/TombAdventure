using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridOp : MonoBehaviour {
	void Awake ()
	{
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}
	
	public void OnClick(){
		Debug.Log("grid click");
	}
}
