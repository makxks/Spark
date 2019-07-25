using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour {

    private Slider volumeSlider;
	// Use this for initialization
	void Start () {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = OptionsValues.optionValues.getVolume();
	}

    public void setVolumeSlider(float sliderValue)
    {
        OptionsValues.optionValues.setVolume(sliderValue);
    }
}
