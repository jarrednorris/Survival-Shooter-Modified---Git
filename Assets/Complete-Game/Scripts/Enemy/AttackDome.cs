using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDome : MonoBehaviour
{

    public float timeBetweenAttacks = 5f;     // The time in seconds between each attack.
    public int attackDamage = 99;               // The amount of health taken away per attack.

    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    CompleteProject.PlayerHealth playerHealth;                  // Reference to the player's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<CompleteProject.PlayerHealth>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks)
        {
            if (playerInRange == true && playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("true again");
                
                
            }
            playerHealth.domeable = true;
            Destroy(gameObject);
        }
    }
}
