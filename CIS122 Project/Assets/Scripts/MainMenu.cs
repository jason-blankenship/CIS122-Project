using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public List<MonoBehaviour> playerControlScripts;
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0;
 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


    }
    public void PlayGame()
    {
        SceneManager.UnloadSceneAsync("StartScreen");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //taking active scene getting the build index adding one then loading that
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
