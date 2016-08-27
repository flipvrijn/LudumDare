using UnityEngine;
using System.Collections;

public class PyramidBuild : MonoBehaviour {

    public int numLayers = 0;
    public int maxLayers = 3;
    public float progressLayer = 0f;
    WorkerIndex workerindex;

    SpriteRenderer[] layers;

	// Use this for initialization
	void Start () {

        workerindex = GameObject.Find("Manager").GetComponent<WorkerIndex>();

        layers = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer layer in layers)
        {
            layer.color = new Color(1f, 1f, 1f, 0);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        double speed = workerindex.workersPyramid * 0.01;

        if (numLayers < maxLayers)
        {
            progressLayer += (float)speed;
            if (progressLayer >= 1f)
            {
                layers[numLayers].color = new Color(1f, 1f, 1f, 1f);
                numLayers += 1;
                progressLayer = 0f;
            }
            else
            {
                layers[numLayers].color = new Color(1f, 1f, 1f, progressLayer);
            }
        }
	}
}
