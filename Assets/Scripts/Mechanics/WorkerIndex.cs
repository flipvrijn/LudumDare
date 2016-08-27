using UnityEngine;
using System.Collections;

public class WorkerIndex : MonoBehaviour {

    public int workers;
    public int workersPyramid;
    public int workersFarm;

	// Use this for initialization
	void Start () {
        workers = 3;
        workersPyramid = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Kill(int num)
    {
        workers -= num;
        float distr = Random.Range(0f, 1f);
        workersPyramid -= (int)(distr * num);
        workersFarm -= (int)((1f-distr) * num);
    }
}
