using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTrigger : MonoBehaviour {

    private ActiveRings activeRings;
    private bool triggered;
    private Controls control;
    private CameraController mainCamera;
    private RingGenerator generator;
    private int currentRow;
    private GameObject ringToSet;
    private GameObject ringSetToSet;

    public GameObject available;

    private GameObject currentRing;

    void Start()
    {
        activeRings = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActiveRings>();
        control = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<RingGenerator>();
        currentRing = null;
    }

	private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ring")
        {
            if(other.gameObject != currentRing)
            {
                currentRing = other.gameObject;
                control.resetExplodeTimer();
            }
            setTriggered(true);
            control.setMovement(false);
            control.StopAllCoroutines();
            ringToSet = other.transform.parent.gameObject;
            ringSetToSet = ringToSet.transform.parent.gameObject;
            transform.parent = other.GetComponentInParent<Transform>();
            if(activeRings.getActiveRingSet() != ringSetToSet)
            {
                generator.generateRingSet(generator.getNumberOfRings());
            }
            activeRings.setActiveRingSet(ringSetToSet, activeRings.getActiveRingSet());
            activeRings.setActiveRing(ringToSet, activeRings.getActiveRing(), control.getHorizontalMove());
            StartCoroutine(mainCamera.setCameraPositionZ());
            mainCamera.setCameraPositionXY();
            transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
            currentRow = ringToSet.GetComponent<RingPosition>().getRow();

            //set next ring "blinking" to show
            available = generator.rings.Find(availableRing);
            control.availableTimer = 0;
        }

        if(other.tag == "ChargeGate" && other.GetComponent<Charged>().getChargable())
        {
            StartCoroutine(GetComponent<SparkCharge>().replenishCharge(75)); //replenish 75/100 units of charge
            other.GetComponent<Charged>().setChargable(false);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ring")
        {
            setTriggered(false);
            control.setMovement(true);
        }
    }

    public void setTriggered(bool isTriggered)
    {
        triggered = isTriggered;
    }

    public bool getTriggered()
    {
        return triggered;
    }

    private bool availableRing(GameObject o)
    {
        if (o)
        {
            if (o.GetComponent<RingPosition>().getRow() == currentRow + 1)
            {
                if (o.GetComponent<RingPosition>().getNumber() == ringToSet.GetComponent<RingPosition>().getNumber())
                {
                    return true;
                }
            }
        }
        return false;
    }
}
