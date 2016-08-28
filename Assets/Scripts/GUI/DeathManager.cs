using UnityEngine;
using System.Collections;

public class DeathManager : MonoBehaviour {

    Transform deathScreen;

	// Use this for initialization
	void Start () {
        deathScreen = transform.GetChild(0);
        deathScreen.gameObject.SetActive(false);
    }
}
