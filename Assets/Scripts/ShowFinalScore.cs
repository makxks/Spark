using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFinalScore : MonoBehaviour {

    private GameObject finalScore;
    private Score score;
    public GameObject GGWrapper;
	// Use this for initialization
	void Start () {
        finalScore = GameObject.FindGameObjectWithTag("FinalScore");
        finalScore.SetActive(false);
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
    }

    public void showFinalScore()
    {
        finalScore.SetActive(true);
        int scoreValue = score.getScore();
        string scoreText = "" + scoreValue;
        //int tableID = 0;
        //string extraData = "";

        //GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) => {
        //    Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        //});

        //GGWrapper.GetComponent<GGServices>().postScoreToLeaderboard(score.getScore());
        
    }
	

}
