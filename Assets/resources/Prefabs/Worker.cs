﻿using UnityEngine;
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
    TimeCycle time;
    public bool slept;
    public float sleepyness;
    
    // Use this for initialization
    void Start () {
        time = GameObject.Find("Manager").GetComponent<TimeCycle>();
        HP = 100;
        WithoutFood = false;
        Hungry = false;
        slept = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (targetPosition.HasValue && moving)
        {
            if (!origin.HasValue)
                origin = new Vector2(this.transform.position.x, this.transform.position.y);

            MoveToTarget();
        }

        sleepyness = CalculateSleep(time.hour);
	}

    void Pause()
    {
        moving = !moving;
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        this.targetPosition = targetPos;
        moving = true;
    }

    void MoveToTarget()
    {
        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        if (Vector2.Distance(currentPosition, targetPosition.Value) > 0.1f)
        {
            Vector2 directionOfTravel = targetPosition.Value - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(
                directionOfTravel.x * Speed * Time.fixedDeltaTime,
                directionOfTravel.y * Speed * Time.fixedDeltaTime,
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
