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
		tombs = new List<Tomb> ();
		
		Tomb tomb = new Tomb ();
		tomb.tombName = StringCollection.KINGOFLU;
		tomb.tombLevel = 1;
		tomb.sceneList = new List<SceneInfo>();
		
		tombs.Add (tomb);
		
		//用列表初始化map
		Transform tombsO = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("Tombs");
		for (int i=0; i<tombsO.childCount; i++) {
			tombsO.GetChild (i).GetComponent<Image> ().sprite = Resources.Load <Sprite> ("_images/_game/tomb_" + tombs [i].tombLevel);
			tombsO.GetChild (i).FindChild ("Text").GetComponent<Text> ().text = tombs [i].tombName;
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

		//(当玩家购买探险日志时，为日志生成唯一的id，此后的探险记录，都与这个id绑定，获取记录也是根据这个id去获取)
		int logId = gData.characterList [0].logId;
		
		bool haveLog = false;
		
		if (logId == -1) {
			
		} else {
			
		}

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
		if (type.Equals ("new")) {
			gData.currentTomb = currentTomb;

			DontDestroyOnLoad (gData);
			Application.LoadLevel ("main");

		} else if (type.Equals ("log")) {

		}
	}
}
