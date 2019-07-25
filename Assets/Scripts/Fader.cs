using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    private float faderSetting;
    private Image fader;
    private bool fadedOut;

	// Use this for initialization
	void Start () {
        fader = GetComponent<Image>();
        faderSetting = 1;
        fader.color = new Color(0, 0, 0, faderSetting);
        setFaderOpacity(0);
        fadedOut = false;
	}

    public void setFaderOpacity(float valueToSet)
    {
        StartCoroutine(fade(valueToSet));
    }

    private IEnumerator fade(float fadeTo)
    {
        float changeFrom = fader.color.a;
        if (changeFrom > fadeTo)
        {
            while (changeFrom > fadeTo)
            {
                changeFrom -= 0.02f;
                fader.color = new Color(0, 0, 0, changeFrom);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (changeFrom < fadeTo)
            {
                changeFrom += 0.02f;
                fader.color = new Color(0, 0, 0, changeFrom);
                yield return new WaitForEndOfFrame();
            }
        }
        if(changeFrom >= 1)
        {
            setFadedOut(true);
        }
    }

    public bool getFadedOut()
    {
        return fadedOut;
    }

    public void setFadedOut(bool isFaded)
    {
        fadedOut = isFaded;
    }
}
