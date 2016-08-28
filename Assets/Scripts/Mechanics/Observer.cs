using UnityEngine;
using System.Collections;

public class Observer : MonoBehaviour {

    public virtual void Publish(Publisher publisher) { }

    public virtual void Register() { }
}
