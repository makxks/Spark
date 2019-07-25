using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

    private Fader fader;
    private MoveCamera cameraMain;
    // Use this for initialization
    void Start()
    {
        fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
        cameraMain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveCamera>();
    }

    // Update is called once per frame
    public void backToMenuButton()
    {
        OptionsValues.optionValues.cancel.Play();
        OptionsValues.optionValues.gameMusic.Stop();
        OptionsValues.optionValues.menuMusic.Play();
        fader.setFaderOpacity(1);
        StartCoroutine(cameraMain.move(new Vector3(0, transform.position.y - 500, -10)));
    }

    void Update()
    {
        if (fader.getFadedOut())
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
