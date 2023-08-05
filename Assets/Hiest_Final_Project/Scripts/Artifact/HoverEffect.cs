using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float hoverHeight = 0.5f; // The height offset of the hovering effect
    public float hoverSpeed = 1f; // The speed of the hovering effect
    public float rotationSpeedX = 30f; // The speed of the rotation around local X-axis
    public float rotationSpeedY = 30f; // The speed of the rotation around local Y-axis

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the vertical offset using Mathf.Sin for hovering effect
        float yOffset = hoverHeight * Mathf.Sin(Time.time * hoverSpeed);

        // Apply the offset to the artifact's position
        Vector3 newPosition = initialPosition + new Vector3(0f, yOffset, 0f);
        transform.position = newPosition;

        // Rotate the artifact around its local X-axis and Y-axis for the gyroscopic effect
        float rotationX = rotationSpeedX * Time.deltaTime;
        float rotationY = rotationSpeedY * Time.deltaTime;
        transform.Rotate(Vector3.right, rotationX, Space.Self);
        transform.Rotate(Vector3.up, rotationY, Space.Self);
    }
}
