using UnityEngine;
using System.Collections;

public class Supplies : MonoBehaviour {

    public float food;
    public float stones;
    public int updateRate;
    public float avgFoodConsumption;

    private int currentUpdate;
    private WorkerIndex workerIndex;

	// Use this for initialization
	void Start () {
        workerIndex = GameObject.Find("Manager").GetComponent<WorkerIndex>();
        food   = 0f;
        stones = 0f;
        avgFoodConsumption = 0f;

        updateRate = 100;
        currentUpdate = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (currentUpdate % updateRate == 0)
        {
            food += workerIndex.NumWorkersFarm() * 0.5f;

            float foodConsumption = 0f;
            foreach (Worker worker in workerIndex.GetAllWorkers())
            {
                food -= worker.foodConsumption;
                foodConsumption += worker.foodConsumption;
            }
            avgFoodConsumption = foodConsumption / workerIndex.GetAllWorkers().Count;
        }
        currentUpdate += 1;        
	}
}
