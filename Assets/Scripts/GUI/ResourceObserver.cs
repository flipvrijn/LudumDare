using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceObserver : Observer {

    Text food;
    Text stone;
    Text totalFoodConsumption;
    Text workers;
    Text workersPyramid;
    Text workersField;
    Text time;

    int day;
    int hour;

	// Use this for initialization
	void Start () {
        food = GameObject.Find("Text_food").GetComponent<Text>();
        stone = GameObject.Find("Text_stones").GetComponent<Text>();

        workers = GameObject.Find("Text_workers").GetComponent<Text>();

        time = GameObject.Find("Text_time").GetComponent<Text>();
        time.text = "0d 0h";

        Register();
	}
	
    public override void Register()
    {
        GameObject.Find("Manager").GetComponent<Supplies>().Subscribe(this);
        GameObject.Find("Manager").GetComponent<TimeCycle>().Subscribe(this);
    }

    public override void Publish(Publisher publisher)
    {
        if (publisher.GetType() == typeof(Supplies))
        {
            Supplies supplies = (Supplies)publisher;

            this.food.text = "Food: " + (System.Math.Floor(supplies.food)).ToString();
            this.stone.text = "Stones: " + (System.Math.Floor(supplies.stones)).ToString();
        }
        else if (publisher.GetType() == typeof(TimeCycle))
        {
            TimeCycle timeCycle = (TimeCycle)publisher;

            hour = timeCycle.hour;
            day = timeCycle.daysPassed;
            time.text = day.ToString() + "d " + hour.ToString() + "h";
        }
        else if (publisher.GetType() == typeof(WorkerIndex))
        {
            WorkerIndex workerIndex = (WorkerIndex)publisher;

            this.workers.text = "Workers: " + workerIndex.GetAllWorkers().Count.ToString();
        }
    }
}
