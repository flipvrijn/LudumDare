using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SiteSlider : Slider {
    
    Site worksite;
    WorkerIndex workers;
    Text text;
    int number;
    bool selected;

	// Use this for initialization
	new void Start () {
        base.Start();
        workers = GameObject.Find("Manager").GetComponent<WorkerIndex>();

        text = GetComponentInChildren<Text>();
        if(name == "Slider_Pyramid")
        {
            worksite = GameObject.Find("Pyramid").GetComponent<Site>();
        }
        else if (name == "Slider_Farm")
        {
            worksite = GameObject.Find("Farm").GetComponent<Site>();
        }

        maxValue = workers.numWorkers;

        minValue = 0f;

        value = worksite.workers.Count;
        text.text = number.ToString();

    }

    void Update()
    {
        if(!selected)
        {
            maxValue = workers.numWorkers;
            value = worksite.workers.Count;
            text.text = value.ToString();
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        selected = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        selected = false;
    }


    public void MoveWorkers()
    {
        WorkSite other = worksite.type == WorkSite.Farm ? WorkSite.Pyramid : WorkSite.Farm;

        if(value < worksite.workers.Count)
        {
            for (int i = 0; i < (worksite.workers.Count - value); i++)
            {
                worksite.workers[i].MoveToSite(other);
            }
        }
        else if(value > worksite.workers.Count)
        {
            Site otherSite = Site.Create(other);
            for (int i = 0; i < value; i++)
            {
                try
                {
                    otherSite.workers[i].MoveToSite(worksite.type);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    ;
                }
             
            }
        }


    }

}
