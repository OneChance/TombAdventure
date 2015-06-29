using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleInit : MonoBehaviour
{

	public GameObject[] enemyPos;
	public GameObject[] characterPos;
	
	private GlobalData gData;
	private Sprite enemySprite;

	void Awake(){
		//gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		object obj = Resources.Load("enemy_1", typeof(GameObject));
		GameObject enmey = obj as GameObject;
		enemySprite = enmey.GetComponent<SpriteRenderer>().sprite;
	}

	// Use this for initialization
	void Start ()
	{
		int enemyNum = Random.Range (1, 4);
		int characterNum = 1;
		//init pos
		for (int i=0; i<enemyNum; i++) {
			enemyPos [i].SetActive (true);
			enemyPos[i].GetComponent<Image>().sprite = enemySprite;
		}
		for (int i=0; i<characterNum; i++) {
			characterPos [i].SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
