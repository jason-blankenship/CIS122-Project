using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public SceneManager Scene;
    public GameObject myGameObjCanvas;
    public KeyCode pauseKey = KeyCode.Escape;
    // Start is called before the first frame update
    public void Start()
    {
        myGameObjCanvas = GameObject.Find("Pause");
        myGameObjCanvas.SetActive(false);
    
        myGameObjCanvas = GameObject.Find("MainMenu");
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        myGameObjCanvas.SetActive(true);


    }
    public void Update()
    {
        if (Input.GetKey(pauseKey))
        {
            Pause();
        }
        else if(Input.GetKey(pauseKey) && myGameObjCanvas)
        {
            UnPause();
        }
    }
    public void PlayGame()
    {
        
        myGameObjCanvas = GameObject.Find("MainMenu");
        myGameObjCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.SetActiveScene(); //taking active scene getting the build index adding one then loading that
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause() 
    {
        SceneManager.UnloadSceneAsync("SampleScene");
        SceneManager.LoadScene("StartScreen");
        myGameObjCanvas = GameObject.Find("Pause");
        myGameObjCanvas.SetActive(true);

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause()
    { 
        myGameObjCanvas = GameObject.Find("Pause");
        myGameObjCanvas.SetActive(false);
        SceneManager.UnloadSceneAsync("StartScreen");

        SceneManager.LoadScene("SampleScene");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
