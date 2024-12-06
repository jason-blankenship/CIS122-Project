using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Written by Cole Gibeau

public class NewBehaviourScript : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent nav;
    public float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.destination = playerTransform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy entered trigger with player!");



            // Stop the enemy from moving
            nav.isStopped = true; // Stop the NavMeshAgent from moving



            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player Damaged");
            }

        }


       

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy exited trigger with player!");

            // Resume movement towards the player if desired
            nav.isStopped = false; // Resume movement


        }
    }


  
}
