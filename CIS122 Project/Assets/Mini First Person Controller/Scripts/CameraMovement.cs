using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosision;
    // Update is called once per frame
    public void Update()
    {
        transform.position = cameraPosision.position; //to move the camera
    }
}
