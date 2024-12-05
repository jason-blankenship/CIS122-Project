using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwingController : MonoBehaviour
{

    private Animator animator;
    private bool swingDone = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enetered Swinging Range");
            animator.SetTrigger("Swing");
            Debug.Log("Swing");
        }

    }
}
