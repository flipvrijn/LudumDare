using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private float _foodConsumption;
    public float foodConsumption
    {
        get
        {
            return _foodConsumption;
        }
        set
        {
            _foodConsumption = value;
        }
    }


    private GameObject _instance;
    public GameObject instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
}

public enum WorkSite { Pyramid, Farm };

public class WorkerIndex : Publisher {

    private List<Worker> workersPyramid = new List<Worker>();
    private List<Worker> workersFarm = new List<Worker>();
    
    // Use this for initialization
    void Start() {
        CreateWorkers(1, WorkSite.Pyramid);
        CreateWorkers(2, WorkSite.Farm);
    }

    // Update is called once per frame
    void Update() {
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
            worker.speed = Random.Range(0.3f, 1f);
            worker.foodConsumption = Random.Range(0.01f, 0.5f);
            switch (site)
            {
                case WorkSite.Farm:
                    worker.instance = CreateInstance(GetFarmPosition(), transform.rotation);
                    workersFarm.Add(worker);
                    break;
                case WorkSite.Pyramid:
                    worker.instance = CreateInstance(GetPyramidPosition(), transform.rotation);
                    workersPyramid.Add(worker);
                    break;
            }
        }
        Notify();
    }

    public void SendToPyramid(int num)
    {
        int n = (workersFarm.Count - num >= 0) ? num : workersFarm.Count;

        List<Worker> farmWorkers = workersFarm.GetRange(0, n);
        farmWorkers.ForEach(worker => {
            worker.instance.transform.position = GetPyramidPosition();
            workersFarm.Remove(worker);
        });
        workersPyramid.AddRange(farmWorkers);
        Notify();
    }

    public void SendToFarm(int num)
    {
        int n = (workersPyramid.Count - num >= 0) ? num : workersPyramid.Count;

        List<Worker> pyramidWorkers = workersPyramid.GetRange(0, n);
        pyramidWorkers.ForEach(worker => {
            worker.instance.transform.position = GetFarmPosition();
            workersPyramid.Remove(worker);
        });
        workersFarm.AddRange(pyramidWorkers);
        Notify();
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

    public override void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.Publish(workersPyramid.Count + workersFarm.Count, workersPyramid.Count, workersFarm.Count);
        }
    }
}
