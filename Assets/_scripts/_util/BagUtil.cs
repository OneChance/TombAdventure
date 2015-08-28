using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BagUtil
{

	public static void AddItem (List<Baggrid> bgList, Baggrid bg)
	{

		bool have = false;

		if (bg.Item.ct == (int)Item.CommonType.EQUIPMENT || bg.Item.ct == (int)Item.CommonType.MERCENARY) {
			for (int i=0; i<bg.Num; i++) {
				bgList.Add (new Baggrid (bg.Item, 1,-1));
			}
		} else {
			for (int i=0; i<bgList.Count; i++) {
				if (bgList [i].Item.name.Equals (bg.Item.name)) {
					
					if (bgList [i].Num == 99) {
						continue;
					}
					
					bgList [i].Num += bg.Num;
					
					if (bgList [i].Num > 99) {
						bg.Num = bgList [i].Num - 99;
						bgList [i].Num = 99;
					} else {
						have = true;
					}
				}
			}
			
			if (!have) {
				bgList.Add (new Baggrid (bg.Item, bg.Num,-1));
			}
		}
	}

}
