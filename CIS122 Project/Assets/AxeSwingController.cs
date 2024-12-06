using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Written by Cole Gibeau 


public class AxeSwingController : MonoBehaviour
{

    private Animator animator;
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
