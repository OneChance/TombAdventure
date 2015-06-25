using UnityEngine;
using System.Collections;

public class UI_Input : MonoBehaviour
{

	public Transform itemMenu;
	public Transform player;
	public float moveDistance = 0.5f;
	private bool menuUnfold = false;

	public void left ()
	{
		player.Translate (Vector2.left * moveDistance);
	}

	public void right ()
	{
		player.Translate (Vector2.right * moveDistance);
	}

	public void up ()
	{
		player.Translate (Vector2.up * moveDistance);
	}

	public void down ()
	{
		player.Translate (Vector2.down * moveDistance);
	}

	public void Menu ()
	{

		if (!menuUnfold) {
			itemMenu.gameObject.SetActive (true);
			itemMenu.position = new Vector3 (itemMenu.position.x, itemMenu.position.y + itemMenu.GetComponent<RectTransform> ().sizeDelta.y, itemMenu.position.z);
			menuUnfold = true;
		} else {

			itemMenu.gameObject.SetActive (false);
			itemMenu.position = new Vector3 (itemMenu.position.x, itemMenu.position.y - itemMenu.GetComponent<RectTransform> ().sizeDelta.y, itemMenu.position.z);

			menuUnfold = false;
		}
	}

	public void Item ()
	{
		Debug.Log ("Item");
	}
}
