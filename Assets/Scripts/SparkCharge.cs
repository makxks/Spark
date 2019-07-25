using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkCharge : MonoBehaviour {

    private float charge;
    private ParticleSystem particles;
    private float changeSpeed;
    private ParticleSystem.MainModule main;
    private bool outOfCharge;
    private bool playedGOSound;
	// Use this for initialization
	void Start () {
        playedGOSound = false;
        charge = 100;
        particles = GetComponent<ParticleSystem>();
        main = particles.main;
    }

    void Update()
    {
        main.startSize = charge * 0.03f;
        if(charge <= 0)
        {
            outOfCharge = true;
            if(playedGOSound == false)
            {
                OptionsValues.optionValues.gameOver.Play();
                playedGOSound = true;
            }
        }
    }
	
	public IEnumerator useCharge(float chargeUsed)
    {
        changeSpeed = chargeUsed / 10;
        float changeTo = charge - chargeUsed;
        while (charge > changeTo){
            charge -= changeSpeed;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine("useCharge");
    }

    public IEnumerator replenishCharge(float chargeGained)
    {
        OptionsValues.optionValues.gate.Play();
        float finalTotal;
        if (charge + chargeGained > 100)
        {
            changeSpeed = (100 - charge) / 10;
            finalTotal = 100;
        }
        else
        {
            changeSpeed = chargeGained / 10;
            finalTotal = charge + chargeGained;
        }
        while (charge < finalTotal)
        {
            charge += changeSpeed;
            yield return new WaitForEndOfFrame();
        }
    }

    public bool getOutOfChargeStatus()
    {
        return outOfCharge;
    }

    public void endGame()
    {
        charge = 0;
    }
}
