using UnityEngine;
using System.Collections;

public class Settlement : Site {


    public int currentTick;
    private int tickRate;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        type = WorkSite.Settlement;

        tickRate = 100;
        currentTick = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (currentTick % tickRate == 0)
        {
            try
            {
                foreach (Worker worker in workers)
                {
                    worker.Sleep();
                }
                    
            }
            catch (System.InvalidOperationException e)
            {
                ;
            }

            currentTick = 0;
        }

        currentTick++;
	}

    public override Vector2 GetRandomPosition()
    {
        return new Vector2(6.4f, 4f);
    }
}
