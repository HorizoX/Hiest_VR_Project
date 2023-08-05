using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass_Destroy : MonoBehaviour
{
    public AudioClip destructionSound; // Assign the destruction sound clip in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If there's no AudioSource component attached, add one dynamically
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Slime" tag
        if (other.CompareTag("Slime"))
        {
            Debug.Log("Glass Container destroyed by Slime!");

            // Play the destruction sound
            if (destructionSound != null)
            {
                audioSource.PlayOneShot(destructionSound);
                Debug.Log("Destruction sound played!");
            }

            // Destroy the glass container
            Destroy(gameObject);
        }
    }
}
