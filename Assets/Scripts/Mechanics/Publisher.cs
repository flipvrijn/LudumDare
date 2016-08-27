using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Publisher : MonoBehaviour {

    protected List<Observer> observers = new List<Observer>();

    public virtual void Notify() {
        foreach (Observer observer in observers)
        {
            observer.Publish();
        }
    }


    public void Subscribe(Observer observer)
    {
        observers.Add(observer);
    }
}
