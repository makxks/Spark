using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    
    private MovePause move;
    private bool paused;
    private Controls controls;

    void Start()
    {
        move = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovePause>();
        controls = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
    }

	public void pause()
    {
        if (!controls.getPaused() && !controls.getMovement())
        {
            OptionsValues.optionValues.cancel.Play();
            move.toPause();
            controls.setPaused(true);
        }
    }

    public void unpause()
    {
        if (controls.getPaused())
        {
            OptionsValues.optionValues.confirm.Play();
            move.toGame();
            controls.setPaused(false);
        }
    }
}
