using UnityEngine;
using System.Collections;

public class HappinessIndex : Observer {

    public int nightsWithoutSleep = 0;
    public int daysWithoutFood = 0;
    public int numDeaths = 0;
    TimeCycle time;

	// Use this for initialization
	void Start () {
        time = GetComponent<TimeCycle>();
        Register();
	}

    public override void Register()
    {
        time.Subscribe(this);
    }

    public override void Publish()
    {
        nightsWithoutSleep += 1;
    }
}
