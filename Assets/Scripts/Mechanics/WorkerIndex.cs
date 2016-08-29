﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum WorkSite { Pyramid, Farm };

public class WorkerIndex : Publisher {
    
    private Dictionary<WorkSite, List<Worker>> workers;
    private List<Worker> hungryWorkers;

    private Supplies supplies;
    private TimeCycle timeCycle;

    private int currentTick;
    private int tickRate;

    private bool noFood;

    // Use this for initialization
    void Start() {
        tickRate = 100;
        noFood = false;

        supplies = GameObject.Find("Manager").GetComponent<Supplies>();
        timeCycle = GameObject.Find("Manager").GetComponent<TimeCycle>();

        workers = new Dictionary<WorkSite, List<Worker>>();
        workers.Add(WorkSite.Farm, new List<Worker>());
        workers.Add(WorkSite.Pyramid, new List<Worker>());
        hungryWorkers = new List<Worker>();

        CreateWorkers(1, WorkSite.Pyramid);
        CreateWorkers(2, WorkSite.Farm);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (currentTick % tickRate == 0)
        {
            DoHungerCheck();

            DoDeathCheck();

            currentTick = 0;
        }
        currentTick++;
    }

    private List<Worker> DoDeathCheck()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            Worker worker = workers[i];

            if (worker.HP == 0)
            {
                Destroy(worker.gameObject);
                workers.RemoveAt(i);
            }
        }

        return workers;
    }

    private void DoHungerCheck()
    {
        int currentFood = (int)System.Math.Floor(supplies.food);

        int starvingWorkers = workers.Count - currentFood;
        if (starvingWorkers > 0 && !noFood)
        {
            noFood = true;

            System.Random rand = new System.Random();
            int[] starvers = System.Linq.Enumerable.Range(0, starvingWorkers).OrderBy(x => rand.Next()).Take(20).OrderByDescending(x => x).ToArray();
            int workersOnFarm = workers[WorkSite.Farm].Count;
            int workersOnPyramid = workers[WorkSite.Pyramid].Count;
            for (int i = 0; i < starvers.Length; i++)
            {
                WorkSite site = starvers[i] < workersOnFarm ? WorkSite.Farm : WorkSite.Pyramid;

                Worker worker = workers[site][starvers[i]];
                worker.Hungry = true;
                hungryWorkers.Add(worker);
            }
        }
        else if (starvingWorkers <= 0)
        {
            noFood = false;
            for (int i = 0; i < hungryWorkers.Count; i++)
            {
                Worker worker = hungryWorkers[i];
                worker.Hungry = false;
            }

            hungryWorkers.Clear();
        }

    }

    private GameObject CreateInstance(Vector3 position, Quaternion rotation)
    {
        GameObject lemmingObject = (GameObject)Resources.Load("Prefabs/Lemming", typeof(GameObject));

        GameObject instance = (GameObject)Instantiate(lemmingObject, position, rotation);
        instance.transform.parent = GameObject.Find("Lemmings").transform;
        return instance;
    }

    public void CreateWorkers(int num, WorkSite site)
    {
        List<Worker> workersOnSite = new List<Worker>();
        for (int i = 0; i < num; i++)
        {
            GameObject instance = null;
            switch (site)
            {
                case WorkSite.Farm:
                    instance = CreateInstance(GetFarmPosition(), transform.rotation);
                    break;
                case WorkSite.Pyramid:
                    instance = CreateInstance(GetPyramidPosition(), transform.rotation);
                    break;
            }

            Worker worker = instance.GetComponent<Worker>();
            worker.HP = 100;
            worker.Speed = Random.Range(0.2f, 0.5f);
            worker.FoodConsumption = Random.Range(0.01f, 0.5f);
            worker.site = site;
            workersOnSite.Add(worker);
        }
        workers[site].AddRange(workersOnSite);

        Notify(this);
    }

    public void SendToPyramid(int num)
    {
        int n = (workers[WorkSite.Farm].Count - num >= 0) ? num : workers[WorkSite.Farm].Count;

        List<Worker> farmWorkers = workers[WorkSite.Farm].GetRange(0, n);
        farmWorkers.ForEach(worker => {
            worker.SetTargetPosition(GetPyramidPosition());
            workers[WorkSite.Farm].Remove(worker);
        });
        workers[WorkSite.Pyramid].AddRange(farmWorkers);
        Notify(this);
    }

    public void SendToFarm(int num)
    {
        int n = (workers[WorkSite.Pyramid].Count - num >= 0) ? num : workers[WorkSite.Pyramid].Count;

        List<Worker> pyramidWorkers = workers[WorkSite.Pyramid].GetRange(0, n);
        pyramidWorkers.ForEach(worker => {
            worker.SetTargetPosition(GetFarmPosition());
            workers[WorkSite.Pyramid].Remove(worker);
        });
        workers[WorkSite.Farm].AddRange(pyramidWorkers);
        Notify(this);
    }

    public Vector3 GetFarmPosition()
    {
        Vector3 ret = new Vector3(Random.Range(-1, -5f), Random.Range(2f, 3f), 0f);
        Debug.Log(ret);
        return ret;
    }

    public Vector3 GetPyramidPosition()
    {
        Vector3 ret = new Vector3(Random.Range(3f, 6f), Random.Range(-1.5f,-3.5f), 0f);
        Debug.Log(ret);
        return ret;
    }

    public int NumWorkersPyramid()
    {
        return workers[WorkSite.Pyramid].Count;
    }

    public int NumWorkersFarm()
    {
        return workers[WorkSite.Farm].Count;
    }

    internal List<Worker> GetAllWorkers()
    {
        List<Worker> allWorkers = new List<Worker>();
        allWorkers.AddRange(workers[WorkSite.Farm]);
        allWorkers.AddRange(workers[WorkSite.Pyramid]);

        return allWorkers;
    }

}
