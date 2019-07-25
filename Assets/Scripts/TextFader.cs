using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour {

    private Controls control;
    private bool faded;
    private Text text;
	// Use this for initialization
	void Start () {
        control = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
        faded = false;
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (control.getStarted() && !faded)
        {
            StartCoroutine(fade());        
        }
	}

    private IEnumerator fade()
    {
        faded = true;
        float change = 0.01f;
        while (text.color.a > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - change);
            yield return new WaitForEndOfFrame();
        }
    }
}
