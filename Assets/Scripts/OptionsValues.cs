using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsValues : MonoBehaviour {

    public static OptionsValues optionValues;
    public AudioSource confirm;
    public AudioSource cancel;
    public AudioSource move;
    public AudioSource rotate;
    public AudioSource gate;
    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public AudioSource gameOver;
    public AudioSource fixedRing;
    public AudioSource explodeSound;

    private float volumeValue;
	// Use this for initialization
	void Awake () {
        if (optionValues == null)
        {
            optionValues = this;
            setVolume(1); ;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (optionValues != this)
        {
            Destroy(gameObject);
        }
	}

    public float getVolume()
    {
        return volumeValue;
    }

    public void setVolume(float volume)
    {
        volumeValue = volume;
        move.volume = volumeValue * 0.8f;
        gate.volume = volumeValue * 0.8f;
        rotate.volume = volumeValue * 0.8f;
        confirm.volume = volumeValue * 0.8f;
        cancel.volume = volumeValue * 0.8f;
        gameOver.volume = volumeValue * 0.8f;
        fixedRing.volume = volumeValue * 0.8f;
        explodeSound.volume = volumeValue * 0.8f;
        menuMusic.volume = volumeValue * 0.9f;
        gameMusic.volume = volumeValue * 0.9f;
    }
}
