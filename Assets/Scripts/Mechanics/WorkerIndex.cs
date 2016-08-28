using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WorkSite { Pyramid, Farm };

public class WorkerIndex : Publisher {

    private List<Worker> workersPyramid = new List<Worker>();
    private List<Worker> workersFarm = new List<Worker>();

    private Supplies supplies;
    private TimeCycle timeCycle;

    private int currentTick;
    private int tickRate;

    // Use this for initialization
    void Start() {
        tickRate = 100;

        supplies = GameObject.Find("Manager").GetComponent<Supplies>();
        timeCycle = GameObject.Find("Manager").GetComponent<TimeCycle>();

        CreateWorkers(1, WorkSite.Pyramid);
        CreateWorkers(2, WorkSite.Farm);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (currentTick % tickRate == 0)
        {
            workersPyramid = DoHungerCheck(workersPyramid);
            workersFarm = DoHungerCheck(workersFarm);

            workersPyramid = DoDeathCheck(workersPyramid);
            workersFarm = DoDeathCheck(workersFarm);

            currentTick = 0;
        }
        currentTick++;
    }

    private List<Worker> DoDeathCheck(List<Worker> workers)
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

    private List<Worker> DoHungerCheck(List<Worker> workers)
    {
        int currentFood = (int)System.Math.Floor(supplies.food);

        for (int i = 0; i < workers.Count; i++)
        {
            Worker worker = workers[i];
            if (currentFood == 0)
            {
                // Not hungry yet
                if (!worker.Hungry)
                {

                    if (!worker.WithoutFood)
                    {
                        worker.WithoutFood = true;
                        worker.WithoutFoodSince = timeCycle.GetHours();
                    }

                    // Check if without food for a day
                    if (timeCycle.GetHours() - worker.WithoutFoodSince > 24)
                    {
                        worker.Hungry = true;
                        worker.LastHungry = timeCycle.hour;
                    }
                }
                else
                {
                    // Reduce HP every so often
                    int currentHour = timeCycle.hour;
                    if (currentHour != worker.LastHungry)
                    {
                        worker.HP -= worker.FoodConsumption * 10;
                        worker.LastHungry = currentHour;
                    }
                }
            }
            workers[i] = worker;
        }

        return workers;
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
            worker.Speed = Random.Range(0.3f, 1f);
            worker.FoodConsumption = Random.Range(0.01f, 0.5f);
            worker.SleepRate = Random.Range(0.2f, 0.5f);
            switch (site)
            {
                case WorkSite.Farm:
                    worker.site = WorkSite.Farm;
                    workersFarm.Add(worker);
                    break;
                case WorkSite.Pyramid:
                    worker.site = WorkSite.Pyramid;
                    workersPyramid.Add(worker);
                    break;
            }
        }

        Notify(this);
    }

    public void SendToPyramid(int num)
    {
        int n = (workersFarm.Count - num >= 0) ? num : workersFarm.Count;

        List<Worker> farmWorkers = workersFarm.GetRange(0, n);
        farmWorkers.ForEach(worker => {
            worker.SetTargetPosition(GetPyramidPosition());
            workersFarm.Remove(worker);
        });
        workersPyramid.AddRange(farmWorkers);
        Notify(this);
    }

    public void SendToFarm(int num)
    {
        int n = (workersPyramid.Count - num >= 0) ? num : workersPyramid.Count;

        List<Worker> pyramidWorkers = workersPyramid.GetRange(0, n);
        pyramidWorkers.ForEach(worker => {
            worker.SetTargetPosition(GetFarmPosition());
            workersPyramid.Remove(worker);
        });
        workersFarm.AddRange(pyramidWorkers);
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
        return workersPyramid.Count;
    }

    public int NumWorkersFarm()
    {
        return workersFarm.Count;
    }

    internal List<Worker> GetAllWorkers()
    {
        List<Worker> allWorkers = new List<Worker>();
        allWorkers.AddRange(workersFarm);
        allWorkers.AddRange(workersPyramid);

        return allWorkers;
    }

}
