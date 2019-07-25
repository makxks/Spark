using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreText : MonoBehaviour {

    private Text text;
    private Score score;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "Final Score: " + score.getScore();
	}
}
