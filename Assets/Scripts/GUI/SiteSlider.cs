using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SiteSlider : Observer {

    Slider slider;
    public GameObject site;
    Site worksite;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        slider.maxValue = GameObject.Find("Manager").GetComponent<WorkerIndex>().numWorkers;
        Debug.Log(GameObject.Find("Manager").GetComponent<WorkerIndex>().numWorkers);
        slider.minValue = 0f;

        worksite = site.GetComponent<Site>();
        slider.value = worksite.workers.Count;
        Register();
    }

    public override void Register()
    {
        GameObject.Find("Manager").GetComponent<WorkerIndex>().Subscribe(this);
    }

    public override void Publish(Publisher publisher)
    {
        Debug.Log(worksite.workers.Count);
        slider.maxValue = ((WorkerIndex)publisher).numWorkers;
        slider.value = worksite.workers.Count;
    }

}
