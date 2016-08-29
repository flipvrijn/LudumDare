using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Farm : Site {

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(-1, -5f), Random.Range(2f, 3f));
    }

}
