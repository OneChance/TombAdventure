using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Geomancer : Pro {
	public Geomancer(string image_name){
		this.proname = "风水师";
		this.prefabname = "Geomancer_"+image_name;
		this.strengthFactor = 1;
	}
}
