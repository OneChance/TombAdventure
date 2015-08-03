using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowHint : MonoBehaviour
{
	
	public static GameObject hintObj;

	void Start ()
	{
		hintObj = gameObject;
		hintObj.transform.FindChild ("Confirm").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CONFIRM;
		hintObj.SetActive (false);
	}

	public static void Hint (string content)
	{
		hintObj.transform.FindChild ("Text").GetComponent<Text> ().text = content;
		hintObj.SetActive (true);
	}

	public void Close ()
	{
		hintObj.SetActive (false);
	}
}
