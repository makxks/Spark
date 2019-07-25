using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour {

    private GameObject mainCamera;
    private RingGenerator generator;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<RingGenerator>();
    }

	void Update()
    {
        if(transform.position.z < mainCamera.transform.position.z - 8)
        {
            generator.rings.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
