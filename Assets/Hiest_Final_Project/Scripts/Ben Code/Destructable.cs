using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion; // Reference to the shattered version of the object

    public void Destroy()
    {
        // Spawn a shattered object
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        // Remove the current object
        Destroy(gameObject);
    }


}
