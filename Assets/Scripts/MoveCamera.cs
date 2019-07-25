using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    private bool moving = false;

    public IEnumerator move(Vector3 to)
    {
        setMoving(true);
        float moves = 50;
        float xMove = (to.x - transform.position.x) / moves;
        float yMove = (to.y - transform.position.y) / moves;
        float zMove = (to.z - transform.position.z) / moves;
        for (int i = 0; i < moves; i++)
        {
            transform.position = new Vector3(transform.position.x + xMove, transform.position.y + yMove, transform.position.z + zMove);
            yield return new WaitForEndOfFrame();
        }
        setMoving(false);
    }

    private void setMoving (bool movingp)
    {
        moving = movingp;
    }

    public bool getMoving()
    {
        return moving;
    }
}
