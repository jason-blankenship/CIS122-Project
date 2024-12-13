﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonLook : MonoBehaviour
{
    public float sensX; // mouse sens
    public float sensY;

    public Transform orientation; //orientation for player

    float xRotation; // camera rotations
    float yRotation;

    [SerializeField] private MainMenu mainMenuScript; //type from mainmenu class to pause
    [SerializeField] private PlayerHealth hitPoints; //type from playerhealth class to get hitpoints
    
    public string sceneName; //for setting the scene

    private KeyCode pauseKey = KeyCode.Escape; //pause key declaration
    public bool isPaused; //for chgecking if paused

    public void Start()
    {
        sceneName = SceneManager.GetActiveScene().name; //setting scene name to "SampleScene"

        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;

        if (mainMenuScript == null) //checking if mainMenuScript exist
        {
            GameObject canvas = GameObject.Find("Canvas"); //satting a gameobject to canvan in a different scene
            if (canvas != null)
            {
                mainMenuScript = canvas.GetComponentInChildren<MainMenu>(); //making getting children of main meu scene
            }
        }
    }
    public void Update() 
    {   
        HandleMouseLook();
        HandlePause();
        HandleDeath();
        
    }
    public void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX; //gets xInput for mouse
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY; //gets yInput for mouse

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //clapms the rotation

        //rotate cam and orientation
        //Quaternion 
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // to apply the rotations this one rotates the camera among both axis
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // this rotates the player among the y axis
    }
    private void HandlePause() 
    {
        if (Input.GetKeyDown(pauseKey) && mainMenuScript != null) //if esc is pressed
        {
            if (Time.timeScale == 1) //if already unpaused
            {
                mainMenuScript.Pause(); //call pause from other script
                isPaused = true; //set ispaused to true

            }
            else
            {
                mainMenuScript.UnPause(); //call unpause from MainMenu Scrpit
                isPaused = false; //set ispaused to false

            }
        }
    }
    private void HandleDeath()
    {
        //checks if currHealth is above 0 if not calls endgame from mainmenu script
        if (hitPoints.currHealth <= 0)
        {
            mainMenuScript.EndGame();
        }
    }
    
}