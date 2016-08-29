using UnityEngine;
using System.Collections;

public class Supplies : Publisher {

    public float food;
    public float stones;
    public float totalFoodConsumption;

    public int currentTick;
    private int tickRate;


    private WorkerIndex workerIndex;

    private Farm farm;

	// Use this for initialization
	void Start () {

        tickRate = 100;
        currentTick = 0;

        workerIndex = GameObject.Find("Manager").GetComponent<WorkerIndex>();
        food   = 2f;
        stones = 0f;

        farm = GameObject.Find("Farm").GetComponent<Farm>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (currentTick % tickRate == 0)
        {
            food += farm.CalculateEfficiency() * 0.5f;

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
