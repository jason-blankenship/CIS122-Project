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
    public float damageInterval = 1.5f;
    private bool isPlayerInTrigger = false;
    private Coroutine damageCoroutine;

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


            isPlayerInTrigger = true;
           
            // Initializes a Coroutine 
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(other));
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

            isPlayerInTrigger = false;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }


    private IEnumerator ApplyDamageOverTime(Collider player)
    {
        while (isPlayerInTrigger) // Keep applying damage while the player is in the trigger
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();


            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
                Debug.Log("Player Damaged");
            }

            yield return new WaitForSeconds(damageInterval); // Wait for the next interval before applying damage again
        }
    }


}
