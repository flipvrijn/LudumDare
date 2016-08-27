using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum WorkSites { Pyramid, Farm, Fishing, Stones };

struct Worker
{
    private float _speed;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    private bool _dead;
    public bool dead
    {
        get
        {
            return _dead;
        }
        set
        {
            _dead = value;
        }
    }
    private WorkSites _worksite;
    public WorkSites worksite
    {
        get
        {
            return _worksite;
        }
        set
        {
            _worksite = value;
        }
    }
}

public class WorkerIndex : MonoBehaviour {

    private List<Worker> workers = new List<Worker>();
    public int numWorkers;
    public int workersPyramid;
    public int workersFarm;

    // Use this for initialization
    void Start() {
        numWorkers = 3;
        workersPyramid = 3;

        for (int i = 0; i < numWorkers; i++)
        {
            Worker worker = new Worker();
            worker.dead = false;
            worker.worksite = WorkSites.Pyramid;
            worker.speed = Random.Range(0.3f, 1f);
            workers.Add(worker);
        }
    }

    // Update is called once per frame
    void Update() {
    }

    public void CreateWorkers(int num)
    {
        numWorkers += num;
        workersPyramid += num;
    }

    public void SendToPyramid(int num)
    {
        if (workersFarm - num >= 0)
        { 
            workersPyramid += num;
            workersFarm -= num;
        }
        else
        {
            workersPyramid += workersFarm;
            workersFarm = 0;
        }
    }

    public void SendToFarm(int num)
    {
        if (workersPyramid - num >= 0)
        {
            workersFarm += num;
            workersPyramid -= num;
        }
        else
        {
            workersFarm += workersPyramid;
            workersPyramid = 0;
        }
        
    }

    public void Kill(int num)
    {
        numWorkers -= num;
        float distr = Random.Range(0f, 1f);
        workersPyramid -= (int)(distr * num);
        workersFarm -= (int)((1f-distr) * num);
    }
}
