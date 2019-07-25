using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRings : MonoBehaviour {

    private GameObject activeRing;
    private GameObject activeRingSet;
    private GameObject inactiveRing;
    private GameObject inactiveRingSet;
    public Material normalGlow;
    public Material activeRingSetGlow;
    public Material activeRingGlow;
    public Material fixedRingGlow;
    public Material rotatorRingGlow;
    public Material timedRingGlow;

    public GameObject getActiveRing()
    {
        return activeRing;
    }

    public void setActiveRing(GameObject ringToSetActive, GameObject ringToSetInactive, bool horizontalMove)
    {
        if (ringToSetInactive && ringToSetInactive.transform.parent!=getActiveRingSet())
        {
            inactiveRing = ringToSetInactive;
            Renderer[] rendsInactive = inactiveRing.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rendsInactive.Length; i++)
            {
                if (rendsInactive[i].gameObject.tag != "Sphere")
                {
                    if (!horizontalMove)
                    {
                        rendsInactive[i].material = setRingColour(rendsInactive[i].gameObject);
                    }
                    else
                    {
                        if (rendsInactive[i].gameObject.GetComponent<RingType>().getType() == "normal")
                        {
                            rendsInactive[i].material = activeRingSetGlow;
                        }
                        else
                        {
                            rendsInactive[i].material = setRingColour(rendsInactive[i].gameObject);
                        }
                    }
                }
            }
        }
        activeRing = ringToSetActive;
        Renderer[] rendsActive = activeRing.GetComponentsInChildren<Renderer>();
        for (int i=0; i<rendsActive.Length; i++)
        {
            if (rendsActive[i].gameObject.tag != "Sphere")
            {
                if (rendsActive[i].gameObject.GetComponent<RingType>().getType() == "normal")
                {
                    rendsActive[i].material = activeRingGlow;
                }
                else
                {
                    rendsActive[i].material = setRingColour(rendsActive[i].gameObject);
                }
            }
        }
    }

    public GameObject getActiveRingSet()
    {
        return activeRingSet;
    }

    public void setActiveRingSet(GameObject ringSetToSetActive, GameObject ringSetToSetInactive)
    {
        if (ringSetToSetInactive)
        {
            if(ringSetToSetInactive == ringSetToSetActive)
            {
                return;
            }
            inactiveRingSet = ringSetToSetInactive;
            Renderer[] rendsInactive = inactiveRingSet.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rendsInactive.Length; i++)
            {
                if (rendsInactive[i].gameObject.tag != "Sphere")
                {
                    rendsInactive[i].material = setRingColour(rendsInactive[i].gameObject);
                }
            }
        }
        activeRingSet = ringSetToSetActive;
        Renderer[] rendsActive = activeRingSet.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rendsActive.Length; i++)
        {
            if (rendsActive[i].gameObject.tag != "Sphere")
            {
                if (rendsActive[i].gameObject.GetComponent<RingType>().getType() == "normal")
                {
                    rendsActive[i].material = activeRingSetGlow;
                }
                else
                {
                    rendsActive[i].material = setRingColour(rendsActive[i].gameObject);
                }
            }
        }
    }

    public Material setRingColour(GameObject ring)
    {
        string type = ring.GetComponent<RingType>().getType();
        Material returnedMat = normalGlow;

        switch (type)
        {
            case "fixed":
                returnedMat = fixedRingGlow;
                break;
            case "rotating":
                returnedMat = rotatorRingGlow;
                break;
            case "timed":
                returnedMat = timedRingGlow;
                break;
            case "normal":
                returnedMat = normalGlow;
                break;
        }

        return returnedMat;
    }
}
