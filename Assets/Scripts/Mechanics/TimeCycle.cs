﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeCycle : MonoBehaviour {

    public int daysPassed = 0;
    public bool nightTime = false;
    public int speed;
    public int seconds;
    public int hour;
    List<Observer> observers = new List<Observer>();

    int maxSecs = 86400;

	// Use this for initialization
	void Start () {
        speed = 2;   
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        seconds += speed*2;
        hour = seconds / 3600;
        if (seconds >= maxSecs)
        {
            NextDay();
        }

        if (seconds > 0.9*maxSecs)
        {
            nightTime = true;
        }

        if (seconds < 0.25*maxSecs)
        {
            nightTime = false;
        }
	}

    void NextDay()
    {
        daysPassed += 1;
        seconds = 0;
        hour = 0;
        Notify();
    }

    public void Subscribe(Observer observer) {
        observers.Add(observer);
    }

    void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.Publish();
        }
    }
}
