using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX; // mouse sens
    public float sensY;

    public Transform orientation; //orientation for player

    float xRotation; // camera rotations
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //cursor locked in middle of screen
        Cursor.visible = false; //cursor invisible
    }

    private void Update()
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
}
