﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    [SerializeField] private MainMenu mainMenuScript;
    [SerializeField] private PlayerHealth hitPoints;
    
    public string sceneName;
    private KeyCode pauseKey = KeyCode.Escape;
    public GameObject myGameObj;
    public Transform mainMenuTransform;
    public Transform pauseMenuTransform;
    public bool isPaused;



    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    public void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        // Lock the mouse cursor to the game screen.
       
        Cursor.lockState = CursorLockMode.Locked;

        if (mainMenuScript == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                mainMenuScript = canvas.GetComponentInChildren<MainMenu>();
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
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        
    }
    private void HandlePause() 
    {
        if (Input.GetKeyDown(pauseKey) && mainMenuScript != null)
        {
            if (Time.timeScale == 1)
            {
                mainMenuScript.Pause();
                isPaused = true;

            }
            else
            {
                mainMenuScript.UnPause();
                isPaused = false;

            }
        }
    }
    private void HandleDeath()
    {
        if (hitPoints.currHealth <= 0)
        {
            mainMenuScript.EndGame();
        }
    }
    
}