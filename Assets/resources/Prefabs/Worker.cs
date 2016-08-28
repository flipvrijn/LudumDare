using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour {

    public float hp;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = (value < 0) ? 0 : value;
        }
    }
    public float Speed;

    /* Movement related */
    public WorkSite site;
    public bool moving = false;
    private Vector2? origin;
    private Vector2? targetPosition;

    /* Food stats */
    public float FoodConsumption;
    public bool Hungry;
    public bool WithoutFood;
    public int WithoutFoodSince;
    public int LastHungry;

    /* Sleep stats */
    public bool slept;
    public bool sleepy;
    public float sleepyness;
    public float SleepRate;

    TimeCycle timeCycle;
    private int currentTick;
    private int tickRate;

    // Use this for initialization
    void Start () {
        timeCycle = GameObject.Find("Manager").GetComponent<TimeCycle>();
        tickRate = 100;

        WithoutFood = false;
        Hungry = false;
        slept = false;
        sleepy = false;

        SetTargetPosition(new Vector2());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (targetPosition.HasValue && moving)
        {
            if (!origin.HasValue)
                origin = new Vector2(this.transform.position.x, this.transform.position.y);

            MoveToTarget();
        }
        SenseTheSleep();
    }

    void Pause()
    {
        moving = !moving;
    }

    public void SetTargetPosition(Vector2 targetPos)
    {
        this.targetPosition = targetPos;
        moving = true;
    }

    void SenseTheSleep()
    {
        /** THIS FUCKIN' SUCKS **/
        if (currentTick % tickRate == 0)
        {
            sleepyness += SleepRate*timeCycle.speed + 0.01f * timeCycle.hour;
            if (sleepyness > 20)
                sleepy = true;
            currentTick = 0;
        }
        currentTick++;
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        if (Vector2.Distance(currentPosition, targetPosition.Value) > 0.1f)
        {
            Vector2 directionOfTravel = targetPosition.Value - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(
                directionOfTravel.x * Speed * timeCycle.speed * Time.fixedDeltaTime,
                directionOfTravel.y * Speed * timeCycle.speed * Time.fixedDeltaTime,
                0f,
                Space.World
            );
        }
        else
        {
            moving = false;
        }
    }

    float CalculateSleep(int hourOfDay)
    {
        float hourNormalized = (2f / 24f) - 1;
        return 10 / (1 + 6 * Mathf.Exp(-7 * hourNormalized));
    }

    void OnMouseDown()
    {
        if (moving)
        {
            Debug.Log("MEH!");
            targetPosition = origin;
        }
    }
}
