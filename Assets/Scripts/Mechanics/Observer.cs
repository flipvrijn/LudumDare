using UnityEngine;
using System.Collections;

public class Observer : MonoBehaviour {

    public virtual void Publish() { }

    public virtual void Publish(Publisher publisher) { }

    public virtual void Publish(float food, float stones, float totalFoodConsumption) { }

    public virtual void Publish(int workers, int workersPyramid, int workersField) { }

    public virtual void Publish(int hours) { }

    public virtual void Register() { }
}
