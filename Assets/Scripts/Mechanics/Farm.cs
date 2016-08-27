using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(wp.x, wp.y);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == Physics2D.OverlapPoint(mousePos))
        {
            Debug.Log("clicked farm!");
        }
    }
}
