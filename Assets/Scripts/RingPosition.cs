using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPosition : MonoBehaviour {

    public int row;
    public int number;

    public void setNumber(int rowIn, int numberIn)
    {
        row = rowIn;
        number = numberIn;
    }

    public int getRow()
    {
        return row;
    }

    public int getNumber()
    {
        return number;
    }
}
