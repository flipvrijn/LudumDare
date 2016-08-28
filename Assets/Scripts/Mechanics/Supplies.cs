using UnityEngine;
using System.Collections;

public class Supplies : Publisher {

    public float food;
    public float stones;
    public int updateRate;
    public float totalFoodConsumption;

    private int currentUpdate;
    private WorkerIndex workerIndex;
   // private GameTick gameTick;

	// Use this for initialization
	void Start () {
        workerIndex = GameObject.Find("Manager").GetComponent<WorkerIndex>();
   //     gameTick = GameObject.Find("Manager").GetComponent<GameTick>();
        food   = 0f;
        stones = 0f;

        updateRate = 100;
        currentUpdate = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        food += workerIndex.NumWorkersFarm() * 0.5f;

        float foodConsumption = 0f;
        foreach (Worker worker in workerIndex.GetAllWorkers())
        {
            food -= worker.FoodConsumption;
            foodConsumption += worker.FoodConsumption;
        }
        totalFoodConsumption = foodConsumption;
        Notify();
	}

    public override void Notify()
    {
        foreach (Observer observer in observers)
        {
            observer.Publish(food, stones, totalFoodConsumption);
        }
    }
}
