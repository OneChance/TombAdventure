using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Geomancer : Pro
{
	public Geomancer (string image_name)
	{
		this.proname = StringCollection.GEOMANCER;
		this.prefabname = "Geomancer_" + image_name;
		this.strengthFactor = 2;
		this.archeologyFactor = 6;
		this.defFactor = 2;
		this.dodgeFactor = 1;
		this.staminaAdd = 20;
		this.healthAdd = 50;
        this.itemid = 1;
	}
}
