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

    public WorkSite site;

    /* Food stats */
    public float FoodConsumption;
    public bool Hungry;
    public bool WithoutFood;
    public int WithoutFoodSince;
    public int LastHungry;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
