﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Site : MonoBehaviour {

    protected int numWorkers;
    protected List<Worker> workers;

    protected virtual void Start()
    {
        workers = new List<Worker>();

    }

    public void Register(Worker worker)
    {
        workers.Add(worker);
    }

    public void Unregister(Worker worker)
    {
        try
        {
            workers.Remove(worker);
        }
        catch (KeyNotFoundException e)
        {
            throw e;
        }
    }

    public float CalculateEfficiency()
    {
        float efficiency = 0;
        foreach (Worker worker in workers)
        {
            efficiency += (1f - (ToFloat(worker.Hungry) * 0.2f) - (ToFloat(worker.sleepy) * 0.3f));
        }
        efficiency *= 0.01f;

        return 0f;
    }

    float ToFloat(bool value)
    {
        if (value)
        {
            return 1f;
        }
        else
        {
            return 0f;
        }
    }

    protected void OnMouseDown()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(wp.x, wp.y);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == Physics2D.OverlapPoint(mousePos))
        {
            Debug.Log("clicked pyramid!");
        }
    }

    protected virtual void React() { }
}
