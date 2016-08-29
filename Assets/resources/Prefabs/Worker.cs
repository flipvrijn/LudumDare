using UnityEngine;
using System.Collections;

public class Worker : Observer {

    public float _hp;
    public float hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = (value < 0) ? 0 : value;
            _hp = (value > 100) ? 100 : value;
        }
    }
    public float speed;
    public float happyness = 100f;

    /* Movement related */
    public WorkSite site;
    public WorkSite lastSite;

    public bool moving = false;
    private Vector2? origin;
    private Vector2? targetPosition;

    /* Food stats */
    public float foodConsumption = 0f;
    public bool hungry = false;
    public int hoursWithoutFood = 0;

    /* Sleep stats */
    public bool sleeping = false;
    public bool sleepy = false;
    public float sleepyness = 0f;
    public int hoursWithoutSleep = 0;
    public float sleepyPenalty = 0f;

    /* Rebelling stats */
    public bool rebelling = false;

    TimeCycle timeCycle;

    // Use this for initialization
    void Start () {
        timeCycle = GameObject.Find("Manager").GetComponent<TimeCycle>();
        
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
        if (!sleeping)
        {
            sleepyness += 0.5f;
            if (sleepyness >= 10f)
            {
                sleepyness = 10f;
                sleepy = true;
            }
        }

        // Starvation ticking every hour
        if (hungry)
        {
            hp -= foodConsumption * 10;
            hoursWithoutFood++;
        }
        // Regen ticking every hour
        else
        {
            hp += foodConsumption * 20;
            hoursWithoutFood = 0;
        }

        // Sleepyness penalty
        if (sleepy && !sleeping)
        {
            sleepyPenalty += 0.1f;
            if (sleepyPenalty > 10)
                sleepyPenalty = 10;
            hp -= sleepyPenalty;
            hoursWithoutSleep++;
        }
    }

    public void Sleep()
    {
        sleeping = true;
        sleepyness -= 0.01f;
        if (sleepyness <= 0)
        {
            sleepyness = 0;
            sleeping = false;
            sleepy = false;
            MoveToSite(WorkSite.Pyramid);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        // Move to new target
        if (targetPosition.HasValue && moving)
        {
            MoveToTarget();
        }

        // Check if dead
        DeathCheck();

        // Update happyness
        HappynessCheck();

        // Go to bed
        if (site != WorkSite.Settlement && sleepy)
        {
            if (Random.Range(0f, 1000 - timeCycle.speed) <= 2)
                GoToBed();
        }

        // Decide to rebel or not
        if (happyness < 50f)
        {
            if (Random.Range(0f, 10000 - happyness - timeCycle.speed) <= 2)
            {
                rebelling = true;
            }
        }
    }

    void GoToBed()
    {
        MoveToSite(WorkSite.Settlement);
    }

    public void SetTargetPosition(Vector2 targetPos)
    {
        this.targetPosition = targetPos;
        moving = true;
    }

    void DeathCheck()
    {
        if (hp == 0)
        {
            Site.Create(this.site).Unregister(this);
            Destroy(this.gameObject);
        }
    }

    void HappynessCheck()
    {
        happyness = 100 - 1.6f * hoursWithoutFood - 1.4f * hoursWithoutSleep - 1 / hp;
    }

    public void MoveToSite(WorkSite site)
    {
        if (this.site != site)
        {
            lastSite = this.site;
            Site.Create(this.site).Unregister(this);
        }

        this.site = site;
        SetTargetPosition(Site.Create(site).GetRandomPosition());
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        if (moving && Vector2.Distance(currentPosition, targetPosition.Value) > 0.4f)
        {
            Vector2 directionOfTravel = targetPosition.Value - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(
                directionOfTravel.x * speed * timeCycle.speed * Time.fixedDeltaTime,
                directionOfTravel.y * speed * timeCycle.speed * Time.fixedDeltaTime,
                0f,
                Space.World
            );
        }
        else
        {
            targetPosition = null;
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
            MoveToSite(lastSite);
            Debug.Log("dsfjsi");
        }
    }
}
