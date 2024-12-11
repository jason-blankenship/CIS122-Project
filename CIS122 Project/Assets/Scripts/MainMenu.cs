//coded by Devon McKinney
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    //setting types defining vars
    [SerializeField] private GameObject mainMenu; //game objects for scenes
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;

    public int waveNumber; //to get wavenumber field from SampleScene > WaveSpaner script 
    public float timePassed; //to get timepassed from SampleScene > WaveSpaner

    public TextMeshProUGUI timeStatText; //for text box editing
    public TextMeshProUGUI roundStatText;
    SceneManager sceneManager;

    public void Start()
    {
        pauseMenu.SetActive(false); //deactivates the pause scene
        deathScreen.SetActive(false); //deactivates the death scene
        mainMenu.SetActive(true); //activate the main menu scene
        
        Time.timeScale = 0; //freezes time while in main menu
        Cursor.lockState = CursorLockMode.None; //unlocks cursor to click button
        Cursor.visible = true; //makes it visible

        timeStatText = timeStatText.GetComponent<TextMeshProUGUI>(); //setting to whatever textbox i put in the inspector
        roundStatText = roundStatText.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        waveNumber = WaveSpawner.currentWave; //setting round count value
        timePassed = WaveSpawner.timePassed; //setting timepassed value
        

    }

    public void PlayGame()
    {
        mainMenu.SetActive(false); //deactivates the main menu scene
        pauseMenu.SetActive(false); //deactivates the pause scene

        Time.timeScale = 1; //unfreezes time
        Cursor.lockState = CursorLockMode.Locked; //locks cursor to screen
        Cursor.visible = false; //makes cursor invisible
    }

    public void PlayAgain() 
    {
        SceneManager.LoadSceneAsync("SampleScene"); //loading sample scene
        SceneManager.LoadSceneAsync("StartScreen", LoadSceneMode.Additive); //loadin start screen without unloading sample scene
        
        mainMenu.SetActive(true); //activate the main menu scene


    }

    public void Pause()
    {
        pauseMenu.SetActive(true); //activates the pause scene

        Time.timeScale = 0;//freezes time
        Cursor.lockState = CursorLockMode.None;//unlocks cursor from screen
        Cursor.visible = true; //makes cursor visible
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false); //deactivates the pause scene

        Time.timeScale = 1;//unfreezes time
        Cursor.lockState = CursorLockMode.Locked; //locks cursor to screen
        Cursor.visible = false; //makes cursor invisible
    }

    public void ReturnToMainMenu()
    {
       PlayAgain(); //calls play again
    }
    public void ActivateDeathScene() 
    {
        deathScreen.SetActive(true); //activates the deathscreen scene

    }
    public void SetText() 
    {
        timeStatText.text = timePassed + " Seconds"; //setting the seconds in the text box

        if (waveNumber == 1) //singular vs plurar checking if i need to add an s
        {
            roundStatText.text = waveNumber + " Wave"; //adds wave number to a string to be sent to a text box
        }
        else
        {
            roundStatText.text = waveNumber + " Waves";
        }
    }
    public void EndGame() 
    {
        ActivateDeathScene(); //calls activate death scene func

        SetText(); //calls set text func

        Time.timeScale = 0; //freezes time
        Cursor.lockState = CursorLockMode.None; //unlocks cursor from screen
        Cursor.visible = true; //makes cursor visible
    }
    public void QuitGame()
    {
        Application.Quit(); //quits the application
    }

    

}
