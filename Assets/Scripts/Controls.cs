using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    private float moveSpeed;
    private float moveSpeedX;
    private float moveSpeedY;
    private float rotateSpeed;
    private bool peakedMovement;
    private bool moving;
    private ActiveRings activeRings;
    private SphereTrigger trigger;
    private SparkCharge sparkCharge;
    private float rotateSparkUsed;
    private float jumpSparkUsed;
    private bool horizontalMove;
    private bool started;
    private bool paused;
    private int iteration;
    private float timer;
    private float explodeTimer;
    private Material active;
    private Material timed;
    private bool exploded;
    private bool gameOver;

    private float touchTimer;
    private bool touchTimerOn;
    private bool holding;
    private float touchDelayTimer;
    private bool touchDelayTimerOn;
    private bool doubleTouch;
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private Vector2 direction;
    private bool holdMoved;
    private bool rotateMoved;
    private bool justUnpaused;
    private int availableIteration;

    public float availableTimer;

    public GameObject spark;
    public GameObject explosion;

    private CameraController mainCamera;

	void Start () {
        timer = 0;
        explodeTimer = 4;
        exploded = false;
        gameOver = false;
        moveSpeed = 1f;
        moveSpeedX = 0.2f;
        moveSpeedY = 0.04f;
        rotateSpeed = 2.5f;
        rotateSparkUsed = 2;
        jumpSparkUsed = 4;
        iteration = 0;
        moving = false;
        paused = false;
        holding = false;
        setStarted(false);
        activeRings = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActiveRings>();
        active = activeRings.activeRingGlow;
        timed = activeRings.timedRingGlow;
        trigger = GameObject.FindGameObjectWithTag("Sphere").GetComponent<SphereTrigger>();
        sparkCharge = GameObject.FindGameObjectWithTag("Sphere").GetComponent<SparkCharge>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        holdMoved = false;
        rotateMoved = false;
        justUnpaused = false;
        touchTimer = 0;
        touchDelayTimer = 0;

        availableIteration = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (trigger.available)
        {
            Renderer[] availableRends = trigger.available.GetComponentsInChildren<Renderer>();
            if (availableRends.Length > 0)
            {
                GameObject availableChild = availableRends[0].gameObject;
                string availableType = trigger.available.GetComponentInChildren<RingType>().getType();
                float previousTime = availableTimer;
                availableTimer += Time.deltaTime;
                availableTimer = availableTimer % 0.5f;
                if (previousTime > availableTimer)
                {
                    availableIteration++;
                    availableIteration = availableIteration % 2;
                    for (int i = 0; i < availableRends.Length; i++)
                    {
                        if (availableIteration == 0)
                        {
                            availableRends[i].material = activeRings.activeRingSetGlow;
                        }
                        else
                        {
                            availableRends[i].material = activeRings.setRingColour(availableChild);
                        }
                    }
                }
            }
        }

        if (touchTimerOn && !gameOver)
        {
            touchTimer += Time.deltaTime;

            //for hold move
            if (touchTimer > 0.3f && direction.y < 25f && direction.y > -25f && !getPaused())
            {
                holdMoved = true;
                OptionsValues.optionValues.move.Play();
                setHorizontalMove(true);
                StopAllCoroutines();
                StartCoroutine(mainCamera.setCameraPositionZ());
                mainCamera.setCameraPositionXY();
                StartCoroutine(sparkMoveHorizontal(spark, spark.transform.localPosition.x));
                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));
                touchTimerOn = false;
                touchTimer = 0;
            }
        }

        /*
        //For double tap move

        if (touchDelayTimerOn)
        {
            touchDelayTimer += Time.deltaTime;
            if(touchDelayTimer > 0.3f)
            {
                OptionsValues.optionValues.move.Play();
                StopAllCoroutines();
                explodeTimer = 4;
                trigger.setTriggered(false);
                StartCoroutine(sparkJump(spark, spark.GetComponentInParent<Transform>().position.z));
                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));
                touchDelayTimerOn = false;
                touchDelayTimer = 0;
            }
        }
        */

        if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() == "rotating"  && !getPaused() && !touchDelayTimerOn)
        {
            if (!holding)
            {
                timer += Time.deltaTime;
            }
            if (timer >= 1 && !moving)
            {
                StartCoroutine(rotateRing(1));
                timer = 0;
            }
        }
        else if(activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() == "timed" && !getPaused())
        {
            timer += Time.deltaTime;
            explodeTimer -= Time.deltaTime;
            Renderer[] rends = activeRings.getActiveRing().GetComponentsInChildren<Renderer>();
            if (timer >= explodeTimer/5)
            {
                for(int i = 0; i<rends.Length; i++)
                {
                    if(rends[i].gameObject.tag != "Sphere")
                    {
                        if(iteration%2 == 0)
                        {
                            rends[i].material = active;
                        }
                        else
                        {
                            rends[i].material = timed;
                        }
                    }
                }
                iteration ++;
                timer = 0;
            }
            if(explodeTimer <= 0 && !exploded)
            {
                exploded = true;
                gameOver = true;
                explode();
                sparkCharge.endGame();
            }
        }
        else
        {
            timer = 0;
            explodeTimer = 4;
            iteration = 0;
        }
        
        
        if (Input.GetButtonDown("Cancel") && !getPaused())
        {
            GetComponent<Pause>().pause();
            setPaused(true);
        }
        else if (Input.GetButtonDown("Cancel") && getPaused())
        {
            GetComponent<Pause>().unpause();
            setPaused(false);
        }
        if (!sparkCharge.getOutOfChargeStatus() && getStarted() && !getPaused() && !gameOver)
        {
            if (Input.GetButtonDown("Jump") && !moving)
            {
                OptionsValues.optionValues.move.Play();
                StopAllCoroutines();
                StartCoroutine(mainCamera.setCameraPositionZ());
                mainCamera.setCameraPositionXY();
                explodeTimer = 4;
                trigger.setTriggered(false);
                StartCoroutine(sparkJump(spark, spark.GetComponentInParent<Transform>().position.z));
                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));
            }
            if (Input.GetButtonDown("moveToNextRing") && !moving)
            {
                OptionsValues.optionValues.move.Play();
                setHorizontalMove(true);
                StopAllCoroutines();
                StartCoroutine(mainCamera.setCameraPositionZ());
                mainCamera.setCameraPositionXY();
                StartCoroutine(sparkMoveHorizontal(spark, spark.transform.localPosition.x));
                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));
            }
            if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
            {
                if (Input.GetButtonDown("moveRight") && !moving)
                {
                    StopAllCoroutines();
                    StartCoroutine(mainCamera.setCameraPositionZ());
                    mainCamera.setCameraPositionXY();
                    StartCoroutine(rotateRing(1));
                    StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                }
                if (Input.GetButtonDown("moveLeft") && !moving)
                {
                    StopAllCoroutines();
                    StartCoroutine(mainCamera.setCameraPositionZ());
                    mainCamera.setCameraPositionXY();
                    StartCoroutine(rotateRing(-1));
                    StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                }
            }
            else
            {
                if (Input.GetButtonDown("moveRight") || Input.GetButtonDown("moveLeft"))
                {
                    OptionsValues.optionValues.fixedRing.Play();
                }
            }
            // // // // 
            if (Input.touchCount > 0 && !paused && !gameOver) {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        direction = Vector2.zero;
                        touchStart = touch.position;
                        touchTimerOn = true;
                        setHolding(true);

                        /*
                        // for double tap move

                        if(touchDelayTimer > 0 && touchDelayTimer < 0.3f)
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
                        if (touchStart.x > Screen.width / 2 && !rotateMoved && !getPaused())
                        {
                            if (direction.y > 25f)
                            {
                                if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(mainCamera.setCameraPositionZ());
                                        mainCamera.setCameraPositionXY();
                                        StartCoroutine(rotateRing(-1));
                                        StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        rotateMoved = true;
                                    }
                                }
                            }
                            else if (direction.y < -25f)
                            {
                                if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(mainCamera.setCameraPositionZ());
                                        mainCamera.setCameraPositionXY();
                                        StartCoroutine(rotateRing(1));
                                        StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        rotateMoved = true;
                                    }
                                }
                            }
                        }
                        else if (touchStart.x < Screen.width / 2 && !rotateMoved && !getPaused())
                        {
                            if (direction.y < -25f)
                            {
                                if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(mainCamera.setCameraPositionZ());
                                        mainCamera.setCameraPositionXY();
                                        StartCoroutine(rotateRing(-1));
                                        StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        rotateMoved = true;
                                    }
                                }
                            }
                            else if (direction.y > 25f)
                            {
                                if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                {
                                    if (!moving)
                                    {
                                        StopAllCoroutines();
                                        StartCoroutine(mainCamera.setCameraPositionZ());
                                        mainCamera.setCameraPositionXY();
                                        StartCoroutine(rotateRing(1));
                                        StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        rotateMoved = true;
                                    }
                                }
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        if (!getPaused() && !holdMoved && !justUnpaused)
                        {
                            direction = touch.position - touchStart;


                            /*
                            //for double tap move

                            if (doubleTouch && !moving)
                            {
                                OptionsValues.optionValues.move.Play();
                                setHorizontalMove(true);
                                StopAllCoroutines();
                                StartCoroutine(sparkMoveHorizontal(spark, spark.transform.localPosition.x));
                                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));
                                touchDelayTimerOn = false;
                                touchDelayTimer = 0;
                            }
                            */

                            if (touchTimer < 0.3f && !moving && direction.y < 25f && direction.y > -25f)
                            {
                                //for hold move
                                
                                OptionsValues.optionValues.move.Play();
                                StopAllCoroutines();
                                StartCoroutine(mainCamera.setCameraPositionZ());
                                mainCamera.setCameraPositionXY();
                                trigger.setTriggered(false);
                                StartCoroutine(sparkJump(spark, spark.GetComponentInParent<Transform>().position.z));
                                StartCoroutine(sparkCharge.useCharge(jumpSparkUsed));

                                /* 
                                //for double tap move

                                touchDelayTimerOn = true;
                                touchDelayTimer = 0;
                                */
                            }
                            else if (touchStart.x > Screen.width / 2 && !rotateMoved)
                            {
                                if (direction.y > 25f)
                                {
                                    if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(mainCamera.setCameraPositionZ());
                                            mainCamera.setCameraPositionXY();
                                            StartCoroutine(rotateRing(-1));
                                            StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        }
                                    }
                                }
                                else if (direction.y < -25f)
                                {
                                    if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(mainCamera.setCameraPositionZ());
                                            mainCamera.setCameraPositionXY();
                                            StartCoroutine(rotateRing(1));
                                            StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        }
                                    }
                                }
                            }
                            else if (touchStart.x < Screen.width / 2 && !rotateMoved)
                            {
                                if (direction.y < -25f)
                                {
                                    if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(mainCamera.setCameraPositionZ());
                                            mainCamera.setCameraPositionXY();
                                            StartCoroutine(rotateRing(-1));
                                            StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        }
                                    }
                                }
                                else if (direction.y > 25f)
                                {
                                    if (activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "fixed" && activeRings.getActiveRing().GetComponentInChildren<RingType>().getType() != "rotating")
                                    {
                                        if (!moving)
                                        {
                                            StopAllCoroutines();
                                            StartCoroutine(mainCamera.setCameraPositionZ());
                                            mainCamera.setCameraPositionXY();
                                            StartCoroutine(rotateRing(1));
                                            StartCoroutine(sparkCharge.useCharge(rotateSparkUsed));
                                        }
                                    }
                                }
                            }
                            setHolding(false);
                            touchTimerOn = false;
                            touchTimer = 0;
                            direction = Vector2.zero;
                        }
                        holdMoved = false;
                        rotateMoved = false;
                        justUnpaused = false;
                        break;
                }


            }
        }
        if (sparkCharge.getOutOfChargeStatus() || gameOver)
        {
            gameOver = true;
            sparkCharge.endGame();
            GetComponent<ShowFinalScore>().showFinalScore();
        }
    }

    private IEnumerator sparkJump(GameObject spark, float initialZPos)
    {
        setMovement(true);
        while (spark.transform.position.z < (initialZPos + 8))
        {
            spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, spark.transform.position.z + moveSpeed);
            yield return new WaitForEndOfFrame();
        }
        spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, initialZPos + 8);
        yield return new WaitForSeconds(0.05f);
        if (trigger.getTriggered())
        {
            timer = 0;
            resetExplodeTimer();
            yield break;
        }
        else
        {
            while (spark.transform.position.z > initialZPos)
            {
                spark.transform.position = new Vector3(spark.transform.position.x, spark.transform.position.y, spark.transform.position.z - moveSpeed);
                yield return new WaitForEndOfFrame();
            }
            setMovement(false);
        }
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
        if (trigger.getTriggered())
        {
            timer = 0;
            resetExplodeTimer();
            yield break;
        }
        else
        {
            while (spark.transform.localPosition.x > initialXPos)
            {
                spark.transform.localPosition = new Vector3(spark.transform.localPosition.x - moveSpeedX, spark.transform.localPosition.y - moveSpeedY, spark.transform.localPosition.z);
                yield return new WaitForEndOfFrame();
            }
            spark.transform.localPosition = new Vector3(initialXPos, initialYPos, spark.transform.localPosition.z);
            setMovement(false);
        }
        setHorizontalMove(false);
    }

    private IEnumerator rotateRing(int leftRight) //-ve for left +ve for right
    {
        Transform ringTransform = activeRings.getActiveRing().transform;
        setMovement(true);
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
        setMovement(false);
        yield break;
    }

    private void explode()
    {
        GameObject explosionInstance = Instantiate(explosion, activeRings.getActiveRing().transform.position, Quaternion.identity);
        explosionInstance.GetComponent<ParticleSystem>().Play();
        trigger.gameObject.SetActive(false);
        Renderer[] rends = activeRings.getActiveRing().GetComponentsInChildren<Renderer>();
        for(int i=0; i < rends.Length; i++)
        {
            rends[i].enabled = false;
        }
    }

    public void setMovement(bool isMoving)
    {
        moving = isMoving;
    }

    public bool getMovement()
    {
        return moving;
    }

    public void setHorizontalMove(bool movingH)
    {
        horizontalMove = movingH;
    }

    public bool getHorizontalMove()
    {
        return horizontalMove;
    }

    public void setStarted(bool hasStarted)
    {
        started = hasStarted;
    }

    public bool getStarted()
    {
        return started;
    }

    public bool getPaused()
    {
        return paused;
    }

    public void setPaused(bool pausedp)
    {
        paused = pausedp;
        if (pausedp)
        {
            touchTimerOn = false;
            touchTimer = 0;
        }
        else
        {
            justUnpaused = true;
        }
    }

    public void resetExplodeTimer()
    {
        explodeTimer = 4;
    }

    public void setHolding(bool hold)
    {
        holding = hold;
    }
}
