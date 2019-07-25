using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private ActiveRings activeRings;
    private bool cameraStartComplete;
    private float moveSpeed;
    private Controls control;

	// Use this for initialization
	void Start () {
        setCameraStartComplete(false);
        activeRings = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActiveRings>();
        transform.position = new Vector3(0, 8, activeRings.getActiveRingSet().transform.position.z - 10);
        control = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
        setFOV(179);
        StartCoroutine(cameraStartZoom());
        moveSpeed = 0.25f;
	}

    public IEnumerator setCameraPositionZ()
    {
        while(transform.position.z < activeRings.getActiveRingSet().transform.position.z - 5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, activeRings.getActiveRingSet().transform.position.z - 5);
        //StopCoroutine(setCameraPositionZ());
    }

    public void setCameraPositionXY()
    {
        float xPos = 0;
        float yPos = 0;
        GameObject activeRing = activeRings.getActiveRing();
        xPos = activeRing.transform.position.x;
        yPos = activeRing.transform.position.y;
        StartCoroutine(moveCameraXY(new Vector3(xPos, yPos, transform.position.z)));
    }

    private IEnumerator moveCameraXY(Vector3 target)
    {
        while(Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 1);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(target.x, target.y, transform.position.z);
    }

    private IEnumerator cameraStartZoom()
    {
        float change = 2.5f;
        while(getFOV() > 80)
        {
            setFOV(getFOV() - change);
            yield return new WaitForEndOfFrame();
        }
        setFOV(80);
        setCameraStartComplete(true);
        control.setStarted(true);
    }

    public bool getCameraStartComplete()
    {
        return cameraStartComplete;
    }

    private void setCameraStartComplete(bool isComplete)
    {
        cameraStartComplete = isComplete;
    }

    private float getFOV()
    {
        return GetComponent<Camera>().fieldOfView;
    }

    private void setFOV(float fieldOfViewValue)
    {
        GetComponent<Camera>().fieldOfView = fieldOfViewValue;
    }

    public float getCameraRotation()
    {
        return transform.rotation.z;
    }

    public void setCameraRotation(float zRotation)
    {
        transform.rotation.SetFromToRotation(transform.localEulerAngles, new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + zRotation));
    }
}
