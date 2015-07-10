using UnityEngine;
using System.Collections;

public class ProFactory  {
	public static Pro getPro(string proname,string img_name){
		if (proname.Equals ("Geomancer")) {
			return new Geomancer (img_name);
		} else if (proname.Equals ("Settler")) {
			return new Settler (img_name);
		} else {
			return null;
		}
	}
}
