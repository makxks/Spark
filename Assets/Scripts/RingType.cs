using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingType : MonoBehaviour {

    [SerializeField] private string type;

    public string getType()
    {
        return type;
    }

    public void setType(string toSetType)
    {
        type = toSetType;
    }
}
