using UnityEngine;
using System.Collections;

public class ProFactory
{
	
	public static Pro getPro (string proname, string img_name)
	{
		if (proname.Equals ("Geomancer")) {
			return new Geomancer (img_name);
		} else if (proname.Equals ("Settler")) {
			return new Settler (img_name);
		} else if (proname.Equals ("Exorcist")) {
			return new Exorcist (img_name);
		} else if (proname.Equals ("Doctor")) {
			return new Doctor (img_name);
		} else {
			return null;
		}
	}

	public static Pro getProById (int itemid, string img_name)
	{
		switch (itemid) {
		case 1:
			return new Geomancer (img_name);
			break;
		case 2:
			return new Settler (img_name);
			break;
		case 3:
			return new Exorcist (img_name);
			break;
		case 4:
			return new Doctor (img_name);
			break;
		default:
			return null;
		}
	}
}
