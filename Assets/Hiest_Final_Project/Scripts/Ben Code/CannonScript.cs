using UnityEngine;
using UnityEngine.InputSystem;

public class CannonScript : MonoBehaviour
{
    [SerializeField] private float forceAmount = 1000f;
    [SerializeField] private bool listenForInput = true;
    [SerializeField] private GameObject grenadePrefab;

    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (listenForInput && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (mainCamera != null)
        {
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
            GameObject grenade = Instantiate(grenadePrefab, spawnPosition, Quaternion.identity);

            GrenadeScript grenadeScript = grenade.GetComponent<GrenadeScript>();
            if (grenadeScript != null)
            {
                grenadeScript.pinPulled = true;
            }

            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(mainCamera.transform.forward * forceAmount);
            }

        }
    }
}
