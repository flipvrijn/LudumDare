using UnityEngine;
using System.Collections;

public class PyramidBuild : MonoBehaviour {

    public int currentLayer = 0;
    public float progressLayer = 0f;
    WorkerIndex workerindex;

    SpriteRenderer[] layers;

	// Use this for initialization
	void Start () {

        workerindex = GameObject.Find("Manager").GetComponent<WorkerIndex>();

        layers = GameObject.Find("Pyramid").GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer layer in layers)
        {
            layer.color = new Color(1f, 1f, 1f, 0);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        double speed = workerindex.NumWorkersPyramid() * 0.01f;

        if (currentLayer < layers.Length)
        {
            progressLayer += (float)speed;
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

    void OnMouseDown()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(wp.x, wp.y);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == Physics2D.OverlapPoint(mousePos))
        {
            Debug.Log("clicked pyramid!");
        }
    }
}
