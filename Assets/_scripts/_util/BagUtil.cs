using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BagUtil{

	public static void AddItem(List<Baggrid> bgList,Baggrid bg){

		bool have = false;

		for (int i=0; i<bgList.Count; i++) {
			if (bgList [i].Item.name.Equals(bg.Item.name)) {

				if(bgList[i].Num==99){
					continue;
				}

				bgList [i].Num += bg.Num;

				if(bgList[i].Num>99){
					bg.Num = bgList[i].Num - 99;
					bgList[i].Num = 99;
				}else{
					have = true;
				}
			}
		}
		
		if (!have) {
			bgList.Add (new Baggrid (bg.Item,bg.Num));
		}
	}

}
