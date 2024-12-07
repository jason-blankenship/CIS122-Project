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

    public void SwingAxe()
    {
        animator.SetTrigger("Swing");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwingAxe();
            Debug.Log("Axe Swung");
        }

    }
}
