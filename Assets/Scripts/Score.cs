using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreText;
    private int score;
    private GameObject spark;
    private float scoreModifier;
	// Use this for initialization
	void Start () {
        spark = GameObject.FindGameObjectWithTag("Sphere");
        score = 0;
        scoreText.text = "" + score;
        scoreModifier = ((1258.234f + 55.5f) / 127f) + 205.4f;
	}
	
	// Update is called once per frame
	void Update () {
        increaseScore();
        scoreText.text = "" + score;
	}

    private void increaseScore()
    {
        if((int)(spark.transform.position.z * scoreModifier) > score)
        {
            score = (int)(spark.transform.position.z * scoreModifier);
        }
    }

    public int getScore()
    {
        return score;
    }
}
