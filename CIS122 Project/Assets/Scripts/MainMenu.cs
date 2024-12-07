using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //taking active scene getting the build index adding one then loading that
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //doing it again to load the map
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
