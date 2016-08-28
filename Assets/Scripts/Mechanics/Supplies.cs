using UnityEngine;
using System.Collections;

public class Supplies : Publisher {

    public float food;
    public float stones;
    public float totalFoodConsumption;

    public int currentTick;
    private int tickRate;
    private WorkerIndex workerIndex;

	// Use this for initialization
	void Start () {
        workerIndex = GameObject.Find("Manager").GetComponent<WorkerIndex>();
        food   = 0f;
        stones = 0f;

        tickRate = 100;
        currentTick = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (currentTick % tickRate == 0)
        { 
            food += workerIndex.NumWorkersFarm() * 0.5f;

            float foodConsumption = 0f;
            foreach (Worker worker in workerIndex.GetAllWorkers())
            {
                food -= worker.FoodConsumption;
                if (food < 0)
                {
                    food = 0;
                }
                foodConsumption += worker.FoodConsumption;
            }
            totalFoodConsumption = foodConsumption;
            Notify(this);
            currentTick = 0;
        }

        currentTick++;
	}
}
