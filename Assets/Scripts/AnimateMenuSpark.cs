using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMenuSpark : MonoBehaviour {

    private float moveSpeed;
    private float xPos;
    private float yPos;
    private float xPosModifier;
    private float yPosModifier;
    private float angle;
	
    void Start()
    {
        moveSpeed = 0.05f;
        xPos = transform.position.x;
        yPos = transform.position.y;
        xPosModifier = 30;
        yPosModifier = 15;
        angle = 0;
        StartCoroutine(animate());
    }
    
    private IEnumerator animate()
    {
        while (true)
        {
            angle += moveSpeed;
            xPos = Mathf.Sin(angle) * xPosModifier;
            yPos = 8 + Mathf.Cos(angle) * yPosModifier;
            transform.position = new Vector3(xPos, yPos, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
}
