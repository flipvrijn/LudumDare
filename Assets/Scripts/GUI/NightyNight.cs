using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class NightyNight : Observer {

    ScreenOverlay nightOverlay;
    bool night = true;

    // Use this for initialization
    void Start () {
        nightOverlay = GetComponent<ScreenOverlay>();

        nightOverlay.intensity = 0.5f;

        Register();
    }

    public override void Register()
    {
        GameObject.Find("Manager").GetComponent<TimeCycle>().Subscribe(this);
    }

    public override void Publish(Publisher publisher)
    {
        int hour = 0;
        bool lastHour = night;

        if (publisher.GetType() == typeof(TimeCycle))
        {
            TimeCycle timeCycle = (TimeCycle)publisher;

            hour = timeCycle.hour;
            night = timeCycle.nightTime;
        }

        if (night != lastHour)
        {
            StartCoroutine("ChangeOverlay");
        }

        //nightOverlay.enabled = night;
    }

    IEnumerator ChangeOverlay()
    {
        if (night)
        {
            for (float f = 0f; f < 0.5f; f += 0.01f)
            {
                nightOverlay.intensity = f;
                yield return null;
            }
        } 
        else
        {
            for (float f = 0.5f; f > 0f; f -= 0.01f)
            {
                nightOverlay.intensity = f;
                yield return null;
            }
        }

    }
}
