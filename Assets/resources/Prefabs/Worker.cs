using UnityEngine;
using System.Collections;

public class Worker : Observer {

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
    public int HoursWithoutFood;

    /* Sleep stats */
    public bool slept;
    public bool sleepy;
    public float sleepyness;
    public float SleepRate;

    TimeCycle timeCycle;

    // Use this for initialization
    void Start () {
        timeCycle = GameObject.Find("Manager").GetComponent<TimeCycle>();

        slept = false;
        sleepy = false;
        Hungry = false;
        
        Register();

        MoveToSite(WorkSite.Pyramid);
    }

    public override void Register()
    {
        timeCycle.Subscribe(this);
    }

    public override void Publish(Publisher publisher)
    {
        // Sleepyness ticking every hour
        sleepyness += 0.5f;
        if (sleepyness > 10f)
        {
            sleepyness = 10f;
        }

        // Starvation ticking every hour
        if (Hungry)
        {
            HP -= FoodConsumption * 10;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (targetPosition.HasValue && moving)
        {
            if (!origin.HasValue)
                origin = new Vector2(this.transform.position.x, this.transform.position.y);

            MoveToTarget();
        }

        // Sense the sleepyness
        SenseTheSleep();

        // Check if dead
        DeathCheck();
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

    void DeathCheck()
    {
        if (HP == 0)
        {
            Site.Create(this.site).Unregister(this);
            Destroy(this.gameObject);
        }
    }

    void SenseTheSleep()
    {
        if (sleepyness >= 10)
        {
            sleepy = true;
        }
    }

    public void MoveToSite(WorkSite site)
    {
        if (this.site != site)
        {
            Site.Create(this.site).Unregister(this);
        }

        this.site = site;
        SetTargetPosition(Site.Create(site).GetRandomPosition());
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
            Site.Create(site).Register(this);
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
