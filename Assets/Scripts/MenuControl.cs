using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour {

    private float moveSpeed;
    private float moveSpeedX;
    private float moveSpeedY;
    private float rotateSpeed;
    private bool peakedMovement;
    private bool moving;
    private bool horizontalMove;
    private bool started;
    private MenuMove menu;

    private float touchTimer;
    private bool touchTimerOn;
    private float touchDelayTimer;
    private bool touchDelayTimerOn;
    private bool doubleTouch;
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private Vector2 direction;

    private bool delayComplete;
    private float delayTimer;
    private bool holdMoved;
    private bool rotateMoved;

    public GameObject spark;
    public Transform ringTransform;
    // Use this for initialization
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MenuMove>();
        moveSpeed = 8f;
        moveSpeedX = 0.2f;
        moveSpeedY = 0.04f;
        rotateSpeed = 2.5f;
        moving = false;

        holdMoved = false;
        rotateMoved = false;
        touchTimer = 0;
        touchDelayTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.inControls)
        {
            if (!delayComplete)
            {
                delayTimer += Time.deltaTime;
                if (delayTimer > 0.5f)
                {
                    delayComplete = true;
                    delayTimer = 0;
                }
            }
            else
            {
                if (touchTimerOn)
                {
                    touchTimer += Time.deltaTime;

                    if (touchTimer > 0.3f && direction.y < 25f && direction.y > -25f)
                    {
                        holdMoved = true;
                        OptionsValues.optionValues.move.Play();
                        setHorizontalMove(true);
                        StopAllCoroutines();
                        StartCoroutine(sparkMoveHorizontal(spark, spark.transform.localPosition.x));
                        touchTimerOn = false;
                        touchTimer = 0;
                    }
                }

                /*
                if (touchDelayTimerOn)
                {
                    touchDelayTimer += Time.deltaTime;
                    if (touchDelayTimer > 0.2f)
                    {
                        OptionsValues.optionValues.move.Play();
                        StopAllCoroutines();
                        StartCoroutine(sparkJump(spark, spark.GetComponentInParent<Transform>().position.z));
                        touchDelayTimerOn = false;
                        touchDelayTimer = 0;
                    }
                }
                */
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            direction = Vector2.zero;
                            touchStart = touch.position;
                            touchTimerOn = true;

                            /*
                            if (touchDelayTimer > 0 && touchDelayTimer < 0.2f)
                            {
                                doubleTouch = true;
                            }
                            else
                            {
                                doubleTouch = false;
                            }
                            */
                            break;

                        case TouchPhase.Moved:
                            direction = touch.position - touchStart;
                            if (touchStart.x > Screen.width / 2 && !rotateMoved)
                            {
                                if (direction.y > 25f)
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(rotateRing(-1));
                                        rotateMoved = true;
                                    }
                                }
                                else if (direction.y < -25f)
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(rotateRing(1));
                                        rotateMoved = true;
                                    }
                                }
                            }
                            else if (touchStart.x < Screen.width / 2 && !rotateMoved)
                            {
                                if (direction.y < -25f)
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(rotateRing(-1));
                                        rotateMoved = true;
                                    }
                                }
                                else if (direction.y > 25f)
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(rotateRing(1));
                                        rotateMoved = true;
                                    }
                                }
                            }
                            break;

                        case TouchPhase.Ended:
                            if (!holdMoved)
                            {
                                direction = touch.position - touchStart;
                                /*
                                if (doubleTouch && !moving)
                                {
                                    OptionsValues.optionValues.move.Play();
                                    setHorizontalMove(true);
                                    StopAllCoroutines();
                                    StartCoroutine(sparkMoveHorizontal(spark, spark.transform.localPosition.x));
                                    touchDelayTimerOn = false;
                                    touchDelayTimer = 0;
                                }
                                */
                                if (touchTimer < 0.3f && !moving && direction.y < 25f && direction.y > -25f)
                                {
                                    OptionsValues.optionValues.move.Play();
                                    StopAllCoroutines();
                                    StartCoroutine(sparkJump(spark, spark.GetComponentInParent<Transform>().position.z));

                                    /*
                                    touchDelayTimerOn = true;
                                    touchDelayTimer = 0;
                                    */
                                }
                                else if (touchStart.x > Screen.width / 2 && !rotateMoved)
                                {
                                    if (direction.y > 25f)
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(rotateRing(-1));
                                        }
                                    }
                                    else if (direction.y < -25f)
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(rotateRing(1));
                                        }
                                    }
                                }
                                else if (touchStart.x < Screen.width / 2 && !rotateMoved)
                                {
                                    if (direction.y < -25f)
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(rotateRing(-1));
                                        }
                                    }
                                    else if (direction.y > 25f)
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(rotateRing(1));
                                        }
                                    }
                                }
                                touchTimerOn = false;
                                touchTimer = 0;
                                direction = Vector2.zero;

                            }
                            holdMoved = false;
                            rotateMoved = false;
                            break;
                    }


                }
            }
        }
        else
        {
            delayTimer = 0;
            delayComplete = false;
        }
    }

    private IEnumerator sparkJump(GameObject spark, float initialZPos)
    {
        setMovement(true);
        while (spark.transform.position.z < (initialZPos + 40))
        {
            spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, spark.transform.position.z + moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, initialZPos + 40);
        yield return new WaitForSeconds(0.05f);
        while (spark.transform.position.z > initialZPos)
        {
            spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, spark.transform.position.z - moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        setMovement(false);
    }

    private IEnumerator sparkMoveHorizontal(GameObject spark, float initialXPos)
    {
        setHorizontalMove(true);
        setMovement(true);
        float initialYPos = spark.transform.localPosition.y;
        while (spark.transform.localPosition.x < initialXPos + 2.15f)
        {
            spark.transform.localPosition = new Vector3(spark.transform.localPosition.x + moveSpeedX, spark.transform.localPosition.y + moveSpeedY, spark.transform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        spark.transform.localPosition = new Vector3(initialXPos + 2.25f, spark.transform.localPosition.y, spark.transform.localPosition.z);
        yield return new WaitForSeconds(0.05f);
        while (spark.transform.localPosition.x > initialXPos)
        {
            spark.transform.localPosition = new Vector3(spark.transform.localPosition.x - moveSpeedX, spark.transform.localPosition.y - moveSpeedY, spark.transform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        spark.transform.localPosition = new Vector3(initialXPos, initialYPos, spark.transform.localPosition.z);
        setMovement(false);
        setHorizontalMove(false);
    }

    private IEnumerator rotateRing(int leftRight) //-ve for left +ve for right
    {
        moving = true;
        OptionsValues.optionValues.rotate.Play();
        float totalRotation = 0;
        if (leftRight < 0)
        {
            while (totalRotation < 22.5f)
            {
                ringTransform.Rotate(Vector3.forward * rotateSpeed);
                totalRotation += rotateSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else if (leftRight > 0)
        {
            while (totalRotation > -22.5f)
            {
                ringTransform.Rotate(Vector3.forward * -rotateSpeed);
                totalRotation -= rotateSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        moving = false;
        yield break;
    }

    public void setMovement(bool isMoving)
    {
        moving = isMoving;
    }

    public void setHorizontalMove(bool movingH)
    {
        horizontalMove = movingH;
    }

    public bool getHorizontalMove()
    {
        return horizontalMove;
    }
}
