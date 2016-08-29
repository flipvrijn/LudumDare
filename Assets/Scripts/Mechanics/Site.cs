using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WorkSite { Pyramid, Farm, Settlement }

public class Site : MonoBehaviour {

    protected int numWorkers;
    public List<Worker> workers;

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
            if (!worker.rebelling)
                efficiency += (1f - (ToFloat(worker.hungry) * 0.2f) - (ToFloat(worker.sleepy) * 0.3f));
        }
        efficiency *= 0.01f;

        return efficiency;
    }

    public virtual Vector2 GetRandomPosition()
    {
        return new Vector2(0,0);
    }

    public List<Worker> GetWorkers()
    {
        return workers;
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
            React();
        }
    }

    protected virtual void React() { }

    public static Site Create(WorkSite worksite)
    {
        switch(worksite)
        {
            case WorkSite.Farm:
                return GameObject.Find("Farm").GetComponent<Farm>();

            case WorkSite.Pyramid:
                return GameObject.Find("Pyramid").GetComponent<Pyramid>();

            case WorkSite.Settlement:
                return GameObject.Find("Settlement").GetComponent<Settlement>();

            default:
                return null;
        }
    }

}
