using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct Worker
{
    public int HP { get; set; }
    public float Speed { get; set; }
    
    /* Food stats */
    public float FoodConsumption { get; set; }
    public bool Hungry { get; set; }
    public bool WithoutFood { get; set; }
    public int WithoutFoodSince { get; set; }
    public int LastHungry { get; set; }
    
    /* Game instance of worker */
    public GameObject Instance { get; set; }
}

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
            int currentFood = (int)System.Math.Floor(supplies.food);

            List<Worker> allWorkers = GetAllWorkers();
            for (int i = 0; i < allWorkers.Count; i++)
            {
                Worker worker = allWorkers[i];
                if (currentFood == 0)
                {
                    Debug.Log("No food!");
                    // Not hungry yet
                    if (!worker.Hungry)
                    {
                        Debug.Log("Not hungry yet...");

                        if (!worker.WithoutFood)
                        {
                            worker.WithoutFood = true;
                            worker.WithoutFoodSince = timeCycle.GetHours();
                            Debug.Log("Without food :(");
                            Debug.Log(worker.WithoutFoodSince);
                        }

                        // Check if without food for a day
                        Debug.Log(timeCycle.GetHours());
                        if (timeCycle.GetHours() - worker.WithoutFoodSince > 24)
                        {
                            Debug.Log("Getting hungry!");
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
                            Debug.Log("OUch!");
                            worker.HP -= 5;
                            worker.LastHungry = currentHour;
                        }
                    }
                }
                allWorkers[i] = worker;
            }
            workersFarm = allWorkers.GetRange(0, workersFarm.Count);
            workersPyramid = allWorkers.GetRange(workersFarm.Count - 1, workersPyramid.Count);
            currentTick = 0;
        }
        currentTick++;
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
            Worker worker = new Worker();
            worker.HP = 100;
            worker.Speed  = Random.Range(0.3f, 1f);
            worker.FoodConsumption = Random.Range(0.01f, 0.5f);
            worker.WithoutFood = false;
            worker.Hungry = false;
            switch (site)
            {
                case WorkSite.Farm:
                    worker.Instance = CreateInstance(GetFarmPosition(), transform.rotation);
                    workersFarm.Add(worker);
                    break;
                case WorkSite.Pyramid:
                    worker.Instance = CreateInstance(GetPyramidPosition(), transform.rotation);
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
            worker.Instance.transform.position = GetPyramidPosition();
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
            worker.Instance.transform.position = GetFarmPosition();
            workersPyramid.Remove(worker);
        });
        workersFarm.AddRange(pyramidWorkers);
        Notify(this);
    }

    public Vector3 GetFarmPosition()
    {
        return new Vector3(Random.Range(-3f, -4f), Random.Range(2.6f, 3f), 2f);
    }

    public Vector3 GetPyramidPosition()
    {
        return new Vector3(Random.Range(-2f, 2f), -2f, 2f);
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

    public void Kill(int num)
    {
        /*numWorkers -= num;
        float distr = Random.Range(0f, 1f);
        workersPyramid -= (int)(distr * num);
        workersFarm -= (int)((1f-distr) * num);
        */
    }
}
