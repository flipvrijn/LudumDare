using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

    Material material;
    public GameObject slider;

	// Use this for initialization
	void Start () {
        material = GetComponent<Renderer>().material;
        slider.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        StartCoroutine("ClickColour");
    }

    IEnumerator ClickColour()
    {
        material.color = new Color(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(1f, 1f, 1f);
        slider.SetActive(true);
    }
}
