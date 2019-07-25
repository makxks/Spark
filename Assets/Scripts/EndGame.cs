using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	public void endGame()
    {
        SparkCharge sparkCharge = GameObject.FindGameObjectWithTag("Sphere").GetComponent<SparkCharge>();
        sparkCharge.endGame();
    }
}
