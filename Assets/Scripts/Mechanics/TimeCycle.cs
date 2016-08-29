using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class TimeCycle : Publisher {

    public int daysPassed = 0;
    public bool nightTime = false;
    public int speed;
    public int seconds;
    public int hour;

    int lastHour;

    int maxSecs = 86400;

	// Use this for initialization
	void Start () {
        speed = 2;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        seconds += speed*2;
        lastHour = hour;
        hour = seconds / 3600;

        if (hour != lastHour)
        {
            Notify(this);
        }

        if (seconds >= maxSecs)
        {
            daysPassed += 1;
            seconds = 0;
            hour = 0;
        }

        if (seconds > 0.9*maxSecs)
        {
            nightTime = true;
        }

        else if (seconds < 0.25*maxSecs)
        {
            nightTime = true;
        }
        else
        {
            nightTime = false;
        }
	}

    public int GetHours()
    {
        return daysPassed * 24 + hour;
    }

}
