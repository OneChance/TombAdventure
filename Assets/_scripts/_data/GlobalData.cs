using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour
{
	public int currentFloor = 1;
	public Dictionary<int,List<SceneInfo>> tombs;
	public Enemy currentEnemy;
	public List<Character> characterList;
	public bool victory = true;
	public string currentEnemyName;
	public Vector3 playerPos; //record the pos of player when battle start
	public Baggrid currentItem;
	public string tombName = StringCollection.KINGOFLU; //应该在地图场景根据选择完成此设置
	public Vector3 preDigPos;
	public int tombLevel =1; //应该在地图场景根据选择完成此设置

	void Awake ()
	{
		//如果在地图场景选择从记录继续，那加载，否则新生成  (应该在地图场景根据选择完成此设置)
		tombs = new Dictionary<int,List<SceneInfo>>();
		//从服务器中加载已记录的地图信息	
		//服务器没有记录
		tombs.Add(tombLevel,new List<SceneInfo>());
	}
}
