using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class TeleportationScript : MonoBehaviour
{

    public Transform head;
    public Transform hand;

   
    public Vector3 TeleportTarget;

    public float raycastDistance = 10f;
    public LayerMask raycastLayer;
    public Color lineColor = Color.white;
    public float lineThickness = 0.02f;
    public Vector3 forwardVector = Vector3.forward;
    public InputActionReference TeleportButtonAction;
    public bool isTeleportButtonPressed = false;
    public float TeleportButtonActionValue;
    public bool teleportAimActive = false;

    public GameObject teleportationIndicatorPrefab;
    public float spinSpeed = 100.0f;
    private GameObject spawnedPrefab;
    private Coroutine spinCoroutine; // to keep track of the spin coroutine
    public float teleportationIndicatorOffset = 0.1f;


    private LineRenderer lineRenderer;
    private bool hitSomething = false;
    public GameObject VRRig;

    private AudioSource audioSource;
    public AudioClip teleportationClip;

    private void Awake()
    {
        TeleportButtonAction.action.Enable();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material.color = lineColor;
        // if audioClip is not null, assign it to the audiosource
        if (teleportationClip != null)
        {
            // audioSource is new audiosource
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = teleportationClip;
        }
    }

    private void Update()
    {
        // Update the TeleportButtonActionValue from TeleportButtonAction
        TeleportButtonActionValue = TeleportButtonAction.action.ReadValue<float>();
        // I[date isTeleportButtonPressed from TeleportButtonActionValue
        isTeleportButtonPressed = TeleportButtonActionValue > 0.5f;

        if (isTeleportButtonPressed)
        {
            teleportAimActive = true;

            // Perform raycast
            Vector3 localForwardVector = transform.TransformDirection(forwardVector);
            Ray ray = new Ray(transform.position, localForwardVector);
            RaycastHit hit;
            hitSomething = Physics.Raycast(ray, out hit, raycastDistance, raycastLayer);

            // If the hit object has a collider
            if (hit.collider != null)
            {
                // Check if the hit object has the tag "Floor"
                if (hit.collider.gameObject.tag == "Floor")
                {
                    if (spawnedPrefab != null)
                    {
                        // Update the position of the spawned prefab Indicator
                        spawnedPrefab.transform.position = hit.point + Vector3.up * teleportationIndicatorOffset; // spawn the prefab slightly above the floor
                    }
                    else
                    {
                        // Initialize the Indicator
                        Vector3 spawnPosition = hit.point + Vector3.up * teleportationIndicatorOffset; // spawn the prefab slightly above the floor
                        spawnedPrefab = Instantiate(teleportationIndicatorPrefab, spawnPosition, Quaternion.identity); // instantiate the prefab
                        spinCoroutine = StartCoroutine(SpinPrefab(spawnedPrefab)); // start spinning the prefab
                    }
                }
                else // Object is not Floor
                {
                    if (spawnedPrefab != null)
                    {
                        StopCoroutine(spinCoroutine); // stop the spin coroutine
                        Destroy(spawnedPrefab); // destroy the spawned prefab
                    }
                }
            }

            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            if (hitSomething)
            {
                lineRenderer.SetPosition(1, hit.point);
                TeleportTarget = hit.point;
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + localForwardVector * raycastDistance);
            }
        }
        else if (!isTeleportButtonPressed)
        {
            
            if (spawnedPrefab != null)
            {
                StopCoroutine(spinCoroutine); // stop the spin coroutine
                Destroy(spawnedPrefab); // destroy the spawned prefab
            }

            // If aim is active and the button is released, check if something is hit
            if (teleportAimActive)
            {
                // Clear the last aim line drawn Once
                lineRenderer.enabled = false;
                lineRenderer.positionCount = 0;
                // Did the aim have something hit when it was released
                if (hitSomething)
                {
                    Vector3 directionToHead = VRRig.transform.position - head.position;
                    directionToHead.y = 0;

                    // Teleport the rig
                    VRRig.transform.position = TeleportTarget + directionToHead;
                    // if audioSource.clip is not null, then play the audio clip
                    if (audioSource != null)
                    {
                        if (audioSource.clip != null)
                        {
                            // play the clip
                            audioSource.Play();
                        }
                    }

                }
            }
            teleportAimActive= false;
        }
    }

    IEnumerator SpinPrefab(GameObject prefab)
    {
        while (true)
        {
            prefab.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime); // spin the prefab around the Y-axis
            yield return null;
        }
    }
}
