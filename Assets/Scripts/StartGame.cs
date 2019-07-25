using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private Fader fader;
    private MoveCamera cameraMain;
	// Use this for initialization
	void Start () {
        fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
        cameraMain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveCamera>();
	}
	
	// Update is called once per frame
	public void startGameButton () {
        fader.setFaderOpacity(1);
        StartCoroutine(cameraMain.move(new Vector3(0, transform.position.y + 200, -10)));
        OptionsValues.optionValues.confirm.Play();
        OptionsValues.optionValues.menuMusic.Stop();
        OptionsValues.optionValues.gameMusic.Play();
	}

    void Update()
    {
        if (fader.getFadedOut())
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
