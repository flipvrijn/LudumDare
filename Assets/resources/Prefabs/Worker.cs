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
    private Vector3 targetPosition;


    /* Food stats */
    public float FoodConsumption;
    public bool Hungry;
    public bool WithoutFood;
    public int WithoutFoodSince;
    public int LastHungry;
    

    // Use this for initialization
    void Start () {
        targetPosition = new Vector3(-6f, -1f, 0);
        moving = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (targetPosition != null && moving)
        {
            MoveToTarget();
        }
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
        Vector3 currentPosition = this.transform.position;

        if (Vector3.Distance(currentPosition, targetPosition) > 0.1f)
        {
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(
                directionOfTravel.x * Speed * Time.fixedDeltaTime,
                directionOfTravel.y * Speed * Time.fixedDeltaTime,
                directionOfTravel.z * Speed * Time.fixedDeltaTime,
                Space.World
            );
        }
        else
        {
            moving = false;
        }
    }
}
