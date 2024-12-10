using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosision;
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosision.position;
    }
}
