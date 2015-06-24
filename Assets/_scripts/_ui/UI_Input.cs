using UnityEngine;
using System.Collections;

public class UI_Input : MonoBehaviour
{

	public Transform itemMenu;
	public Transform player;
	
	private bool menuUnfold = false;


	public void left ()
	{
			player.Translate (Vector2.left);
	}

	public void right ()
	{
			player.Translate (Vector2.right);
	}

	public void up ()
	{
			player.Translate (Vector2.up);
	}

	public void down ()
	{
			player.Translate (Vector2.down);
	}	

	public void Menu(){

		if(!menuUnfold){
			itemMenu.gameObject.SetActive(true);
			itemMenu.position = new Vector3(itemMenu.position.x,itemMenu.position.y+itemMenu.GetComponent<RectTransform>().sizeDelta.y,itemMenu.position.z);
			menuUnfold = true;
		}else{

			itemMenu.gameObject.SetActive(false);
			itemMenu.position = new Vector3(itemMenu.position.x,itemMenu.position.y-itemMenu.GetComponent<RectTransform>().sizeDelta.y,itemMenu.position.z);

			menuUnfold = false;
		}
	}

	public void Item(){
		Debug.Log("Item");
	}
}
