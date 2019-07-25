using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePause : MonoBehaviour {

    private Vector3 pausePos;
    private MoveCamera cameraMain;
    private float savedXPos;
    private float savedYPos;
    private float savedZPos;

    void Start()
    {
        pausePos = new Vector3(0, 500, 0);
        savedXPos = 0;
        savedYPos = 8;
        savedZPos = 0;
        cameraMain = GetComponent<MoveCamera>();
    }

    public void toPause()
    {
        if (!cameraMain.getMoving())
        {
            savedXPos = transform.position.x;
            savedYPos = transform.position.y;
            savedZPos = transform.position.z;
            StartCoroutine(cameraMain.move(pausePos));
        }
    }

    public void toGame()
    {
        if (!cameraMain.getMoving())
        {
            StartCoroutine(cameraMain.move(new Vector3(savedXPos, savedYPos, savedZPos)));
        }
    }

}
