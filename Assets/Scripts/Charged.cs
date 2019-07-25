using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charged : MonoBehaviour {

    public bool chargable;
	// Use this for initialization
	void Start () {
        chargable = true;
	}
	
	public void setChargable(bool hasCharged)
    {
        chargable = hasCharged;
    }

    public bool getChargable()
    {
        return chargable;
    }
}
