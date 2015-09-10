using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Map : MonoBehaviour
{
	private GlobalData gData;
	public GameObject tombIn;
	public Tomb currentTomb;
	public List<Tomb> tombs;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		//从服务器加载tomb列表
		tombs = gData.tombs;


		//用列表初始化map
		Transform tombsO = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("Tombs");
		for (int i=0; i<tombsO.childCount; i++) {
			tombsO.GetChild (i).GetComponent<Image> ().sprite = Resources.Load <Sprite> ("_images/_game/tomb_" + tombs [i].tombLevel);
			tombsO.GetChild (i).FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN[tombs [i].tombName];
			tombsO.GetChild (i).GetComponent<TombInfo> ().tomb = tombs [i];
		}

		Transform tombsIn = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("TombIn");
		tombsIn.FindChild ("New").FindChild ("Text").GetComponent<Text> ().text = StringCollection.NEWAD;
		tombsIn.FindChild ("Log").FindChild ("Text").GetComponent<Text> ().text = StringCollection.LOGAD;
		tombsIn.FindChild ("Back").FindChild ("Text").GetComponent<Text> ().text = StringCollection.BACKTOMAP;
	}

	public void ToCity ()
	{
		DontDestroyOnLoad (gData);
		Application.LoadLevel ("city");
	}

	public void ChooseTomb (TombInfo tombInfo)
	{

		currentTomb = tombInfo.tomb;

		tombIn.SetActive (true);

		bool haveLog = false;

		Dictionary<int,TombLog> tombLogs = gData.characterList[0].tombLogs;

		List<SceneInfo> sceneList = new List<SceneInfo>();

		if(tombLogs.ContainsKey(currentTomb.dbid)){
			haveLog = true;
			sceneList = tombLogs[currentTomb.dbid].sceneinfos;
		}

		currentTomb.sceneList = sceneList;

		if (haveLog) {
			tombIn.transform.FindChild ("Log").gameObject.SetActive (true);
		} else {
			tombIn.transform.FindChild ("Log").gameObject.SetActive (false);
		}
	}

	public void Back ()
	{
		tombIn.SetActive (false);
	}

	public void goTomb (string type)
	{
		if(type.Equals("new")){
			currentTomb.sceneList = new List<SceneInfo>();
		}

		gData.currentTomb = currentTomb;
		DontDestroyOnLoad (gData);
		Application.LoadLevel ("main");
	}
}
