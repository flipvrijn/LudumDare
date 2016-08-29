using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PyramidBuild : Site {

    public int currentLayer = 0;
    public float progressLayer = 0f;
    public float efficiency;

    SpriteRenderer[] layers;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        worksite = WorkSite.Pyramid;
        
        layers = GameObject.Find("Pyramid").GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer layer in layers)
        {
            layer.color = new Color(1f, 1f, 1f, 0);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        efficiency = CalculateEfficiency();

        if (currentLayer < layers.Length)
        {
            progressLayer += (float)efficiency;
            if (progressLayer >= 1f)
            {
                layers[currentLayer].color = new Color(1f, 1f, 1f, 1f);
                currentLayer += 1;
                progressLayer = 0f;
            }
            else
            {
                layers[currentLayer].color = new Color(1f, 1f, 1f, progressLayer);
            }
        }
	}

    public static Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(3f, 6f), Random.Range(-1.5f, -3.5f));
    }

}
