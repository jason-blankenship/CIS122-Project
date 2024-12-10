//coded by Devon McKinney
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;

    [SerializeField] private WaveSpawner waveCount;
    public int waveNumber;

    [SerializeField] private WaveSpawner timeAmount;
    public float timePassed;

    SceneManager sceneManager;

    public void Start()
    {
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        mainMenu.SetActive(true);
        
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        waveCount = GetComponent<WaveSpawner>();
        timeAmount = GetComponent<WaveSpawner>();
    }

    void Update()
    {
        waveNumber = WaveSpawner.currentWave;
        timePassed = WaveSpawner.timePassed;

    }

    public void PlayGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayAgain() 
    {
        SceneManager.LoadSceneAsync("SampleScene");
        SceneManager.LoadSceneAsync("StartScreen", LoadSceneMode.Additive);
        
        mainMenu.SetActive(true);

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
       PlayAgain();
    }
    public void ActivateDeathScene() 
    {
        deathScreen.SetActive(true);
        
    }
    public void EndGame() 
    {
        ActivateDeathScene();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    

}
