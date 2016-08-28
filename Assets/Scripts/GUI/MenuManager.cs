using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    Transform pauseScreen;
    bool paused = false;

	// Use this for initialization
	void Start () {
        pauseScreen = transform.GetChild(0);
        pauseScreen.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            paused = !paused;
            pauseScreen.gameObject.SetActive(paused);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToGame()
    {
        paused = !paused;
        pauseScreen.gameObject.SetActive(paused);
    }
}
