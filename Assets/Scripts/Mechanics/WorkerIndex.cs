using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorkerIndex : Publisher {

    public int numWorkers;

    private Supplies supplies;

    private int currentTick;
    private int tickRate;

    private bool noFood;

    // Use this for initialization
    void Start() {
        tickRate = 100;
        noFood = false;

        supplies = GameObject.Find("Manager").GetComponent<Supplies>();
        
        CreateWorkers(3, WorkSite.Farm);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (currentTick % tickRate == 0)
        {
            DoHungerCheck();

            currentTick = 0;
        }
        currentTick++;
    }
    
    private void DoHungerCheck()
    {
        int currentFood = (int)System.Math.Floor(supplies.food);

        // Get workers from all work sites
        List<Worker> allWorkers = GetAllWorkers();

        // Calculate how many will starve with current food
        int starvingWorkers = allWorkers.Count - currentFood;
        if (starvingWorkers > 0 && !noFood)
        {
            noFood = true;

            // Randomly select workers from all workers
            System.Random rand = new System.Random();
            int[] starvers = System.Linq.Enumerable.Range(0, allWorkers.Count)
                .OrderBy(x => rand.Next())
                .Take(starvingWorkers)
                .ToArray();

            // Starve them
            for (int i = 0; i < starvers.Length; i++)
            {
                Worker worker = allWorkers[starvers[i]];
                worker.hungry = true;
            }
        }
        else if (starvingWorkers <= 0)
        {
            noFood = false;
            // No longer starve them
            for (int i = 0; i < allWorkers.Count; i++)
            {
                Worker worker = allWorkers[i];
                worker.hungry = false;
            }
        }

        Notify(this);
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
        numWorkers += num;
        
        Site worksite = Site.Create(site);
        for (int i = 0; i < num; i++)
        {
            GameObject instance = null;
            switch (site)
            {
                case WorkSite.Farm:
                    instance = CreateInstance(Site.Create(WorkSite.Farm).GetRandomPosition(), transform.rotation);
                    break;
                case WorkSite.Pyramid:
                    instance = CreateInstance(Site.Create(WorkSite.Pyramid).GetRandomPosition(), transform.rotation);
                    break;
                case WorkSite.Settlement:
                    instance = CreateInstance(Site.Create(WorkSite.Settlement).GetRandomPosition(), transform.rotation);
                    break;
            }

            Worker worker = instance.GetComponent<Worker>();
            worker.hp = 100;
            worker.speed = Random.Range(0.2f, 0.5f);
            worker.foodConsumption = Random.Range(0.01f, 0.5f);
            worker.site = site;
            worksite.Register(worker);
        }
        
        Notify(this);
    }    

    public List<Worker> GetAllWorkers()
    {
        List<Worker> allWorkers = new List<Worker>();
        WorkSite[] worksites = (WorkSite[])System.Enum.GetValues(typeof(WorkSite));
        foreach (WorkSite ws in worksites)
        {
            allWorkers.AddRange(Site.Create(ws).GetWorkers());
        }

        return allWorkers;
    }

}
