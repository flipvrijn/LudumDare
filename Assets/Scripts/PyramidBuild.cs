using UnityEngine;
using System.Collections;

public class PyramidBuild : MonoBehaviour {

    public int numLayers = 0;
    public int maxLayers = 3;
    public int numWorkers = 0;
    public float progressLayer = 0f;

    SpriteRenderer[] layers;

	// Use this for initialization
	void Start () {

        layers = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer layer in layers)
        {
            layer.color = new Color(1f, 1f, 1f, 0);
        }

        numWorkers = 3;
	}

	// Update is called once per frame
	void FixedUpdate () {
        double speed = numWorkers * 0.01;

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
