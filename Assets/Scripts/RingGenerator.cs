using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGenerator : MonoBehaviour {

    public GameObject ringPart;
    public GameObject emptyRing;
    public GameObject ringSet;
    public GameObject sphere;
    public GameObject chargeGate;

    public List<GameObject> rings;

    private ActiveRings activeRings; 
    private int numberOfRings;
    private int maxSegments;
    private int noOfSegments;
    private int row;
    private int rowSeparation;
    private int ringSetRadius;
    private float segmentRotation;

	// Use this for initialization
	void Start () {
        activeRings = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActiveRings>();
        setNumberOfRings(1);
        maxSegments = 16;
        setRow(0);
        segmentRotation = 22.5f;
        ringSetRadius = 8;
        rowSeparation = 8;
        initialise();
	}

    //initialise (create first few sets of rings)
    private void initialise()
    {
        rings.Clear();
        int initialRows = 12;

       for (int i = 1; i <= initialRows; i++)
        { 
            generateRingSet(numberOfRings);
        }
    }

    public void generateRingSet(int noOfRings)
    {
        GameObject ringSetInstance = Instantiate(ringSet);
        ringSetInstance.transform.position = new Vector3(0, 0, getRow() * rowSeparation);
        for (int i = 0; i < noOfRings; i++)
        {
            
            float xPos = ringSetRadius * Mathf.Sin(i * (Mathf.PI/4));
            float yPos = ringSetRadius * Mathf.Cos(i * (Mathf.PI/4));

            Vector3 ringPos = new Vector3(xPos, yPos, 0);
            generateRing(ringPos, ringSetInstance, i);
        }
        setRow(getRow()+1);
        if(row%10 == 0 && getNumberOfRings() != 8 && getNumberOfRings() != 0)
        {
            setNumberOfRings(getNumberOfRings() + 1);
        }
        if(row%5 == 0)
        {
            generateChargeGate();
        }
    }

    private void generateRing(Vector3 position, GameObject ringSet, int ringNumber)
    {
        GameObject emptyRingInstance = Instantiate(emptyRing);
        emptyRingInstance.GetComponent<RingPosition>().setNumber(getRow(), ringNumber);
        rings.Add(emptyRingInstance);
        string type = "";
        if (getRow() > 10)
        {
            type = setRingType();
        }
        else
        {
            type = "normal";
        }
        emptyRingInstance.transform.parent = ringSet.transform;
        emptyRingInstance.transform.localPosition = position;
        bool[] hasSegment = new bool[maxSegments];
        for (int i = 0; i<hasSegment.Length; i++)
        {
            hasSegment[i] = false;
        }
        int segments = setNumberOfSegments();
        int currentSegments = 0;
        int generatingSegment = 0;
        while (currentSegments < segments)
        {
            if (!hasSegment[generatingSegment % 16])
            {
                float generationChance;
                if (segments == 16)
                {
                    generationChance = 1;
                }
                else
                {
                    generationChance = Random.Range(0f, 1f);
                }
                if (generationChance > 0.4f)
                {
                    float rotation = (generatingSegment * segmentRotation) - 11.25f;
                    GameObject ringPartInstance = Instantiate(ringPart);
                    if(getRow() == 0)
                    {
                        ringPartInstance.GetComponent<RingType>().setType("normal");
                    }
                    else
                    {
                        ringPartInstance.GetComponent<RingType>().setType(type);
                    }
                    ringPartInstance.transform.parent = emptyRingInstance.transform;
                    ringPartInstance.transform.localPosition = Vector3.zero;
                    ringPartInstance.transform.eulerAngles = new Vector3(0, 0, rotation);
                    ringPartInstance.GetComponent<Renderer>().material = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActiveRings>().setRingColour(ringPartInstance);
                    hasSegment[generatingSegment % 16] = true;
                    currentSegments++;
                }

                if (getRow() == 0)
                {
                    initialiseFirstActiveRing(emptyRingInstance, ringSet);
                }
            }
            generatingSegment = (generatingSegment++) % 16;
            generatingSegment++;
        }
    }

    private int setNumberOfSegments()
    {
        int minimum;
        if (getRow() == 0 || getRow()==10)
        {
            return 16;
        }
        else if (numberOfRings == 1)
        {
            minimum = 1;
        }
        else
        {
            minimum = 0;
        }
        int segments = Random.Range(minimum, 16);
        return segments;
    }

    private void setRow(int i)
    {
        row = i;
    }

    public int getRow()
    {
        return row;
    }

    public int getNumberOfRings()
    {
        return numberOfRings;
    }

    public void setNumberOfRings(int rings)
    {
        numberOfRings = rings;
    }

    private void initialiseFirstActiveRing(GameObject ring, GameObject ringSet)
    {
        sphere.transform.parent = ring.transform;
        activeRings.setActiveRingSet(ringSet, null);
        activeRings.setActiveRing(ring, null, false);
    }

    private void generateChargeGate()
    {
        int rings = getNumberOfRings();
        int randomRing = Random.Range(0, rings);

        float xPos = ringSetRadius * Mathf.Sin(randomRing * (Mathf.PI / 4));
        float yPos = ringSetRadius * Mathf.Cos(randomRing * (Mathf.PI / 4));

        Vector3 gatePos = new Vector3(xPos, yPos, (getRow()*8) + 2.5f);
        GameObject gate = Instantiate(chargeGate, gatePos, Quaternion.identity);
        gate.transform.Rotate(Vector3.forward, 45f);
    }

    private string setRingType()
    {
        float ringTypeValue = Random.Range(0.0f, 10.0f);
        string type = "";

        switch ((int)ringTypeValue)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                type = "normal";
                break;
            case 7:
                type = "timed";
                break;
            case 8:
                type = "fixed";
                break;
            case 9:
            case 10:
                type = "rotating";
                break;
        }
        return type;
    }
}
