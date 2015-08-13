using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Settler : Pro
{
	public Settler (string image_name)
	{
		this.proname = StringCollection.SETTLER;
		this.prefabname = "Settler_" + image_name;
		this.strengthFactor = 6;
		this.archeologyFactor = 0;
		this.defFactor = 6;
		this.dodgeFactor = 1;
		this.staminaAdd = 50;
		this.healthAdd = 70;
        this.itemid = 2;
	}
}
