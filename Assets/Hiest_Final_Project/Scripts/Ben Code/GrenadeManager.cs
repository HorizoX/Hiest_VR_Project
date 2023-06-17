using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public Transform grenadeSpawnPosition; // Reference to the transform position to spawn the grenade
    public GameObject grenadePrefab; // Reference to the grenade prefab

    private GameObject currentGrenade; // Reference to the currently active grenade

    void Start()
    {
        // Spawn the initial grenade at the designated position
        SpawnGrenade();
    }

    void Update()
    {
        if (currentGrenade != null) 
        {
            if (currentGrenade.GetComponent<GrenadeScript>() != null)
            {
                // Check if the pin has been pulled
                if (currentGrenade.GetComponent<GrenadeScript>().pinPulled)
                {
                    // Spawn a new grenade at the designated position
                    SpawnGrenade();

                }
            }
        }
        
    }

    void SpawnGrenade()
    {
        // Instantiate a new grenade prefab at the designated position
        currentGrenade = Instantiate(grenadePrefab, grenadeSpawnPosition.position, grenadeSpawnPosition.rotation);

        // Make the newly spawned grenade kinematic
        currentGrenade.GetComponent<Rigidbody>().isKinematic = true;

        // Assign the transform parent to the grenade spawn position
        currentGrenade.transform.parent = grenadeSpawnPosition;
    }
}
