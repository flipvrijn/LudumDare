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

    public override void Publish(float food, float stones, float totalFoodConsumption)
    {
        this.food.text = "Food: " + (System.Math.Floor(food)).ToString();
        this.stone.text = "Stones: " + (System.Math.Floor(stones)).ToString();
       // this.totalFoodConsumption.text = totalFoodConsumption.ToString();

    }

    public override void Publish(int workers, int workersPyramid, int workersField)
    {
        this.workers.text = "Workers: " + workers.ToString();
       // this.workersPyramid.text = workersPyramid.ToString();
       // this.workersField.text = workersField.ToString();
    }

    public override void Publish(int hour)
    {
        this.hour = hour;
        time.text = day.ToString() + "d " + hour.ToString() + "h";
    }

    public override void Publish()
    {
        this.day += 1;
        time.text = day.ToString() + "d " + hour.ToString() + "h";
    }

}
