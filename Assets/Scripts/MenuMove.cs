using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMove : MonoBehaviour {

    private Vector3 optionsPos;
    private Vector3 mainPos;
    private Vector3 exitPos;
    private Vector3 controlPos;
    private MoveCamera cameraMain;
    public bool inControls;

    void Start()
    {
        inControls = false;
        optionsPos = new Vector3(512, 384, -100);
        mainPos = new Vector3(0, 0, -10);
        exitPos = new Vector3(512, -384, -100);
        controlPos = new Vector3(-512, 384, -100);

        cameraMain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveCamera>();
    }

    public void toOptions()
    {
        StartCoroutine(cameraMain.move(optionsPos));
        setInControls(false);
        OptionsValues.optionValues.confirm.Play();
    }

    public void toMain()
    {
        StartCoroutine(cameraMain.move(mainPos));
        OptionsValues.optionValues.cancel.Play();
    }

    public void toExit()
    {
        StartCoroutine(cameraMain.move(exitPos));
        OptionsValues.optionValues.confirm.Play();
    }

    public void toControl()
    {
        StartCoroutine(cameraMain.move(controlPos));
        setInControls(true);
        OptionsValues.optionValues.confirm.Play();
    }

    public void setInControls(bool inControlp)
    {
        inControls = inControlp;
    }

    public bool getInControls()
    {
        return inControls;
    }
}
