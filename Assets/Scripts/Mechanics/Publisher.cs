using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Publisher : MonoBehaviour {

    protected List<Observer> observers = new List<Observer>();

    public virtual void Notify(Publisher publisher) {
        foreach (Observer observer in observers)
        {
            observer.Publish(publisher);
        }
    }


    public void Subscribe(Observer observer)
    {
        observers.Add(observer);
    }
}
