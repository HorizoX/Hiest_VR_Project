using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float rotationRange = 30f;
    public string stopTag = "Slime";

    private bool shouldRotate = true;
    private float initialRotationY;
    private float targetRotationY;
    private bool rotateClockwise = true;

    private void Start()
    {
        initialRotationY = transform.eulerAngles.y;
        targetRotationY = initialRotationY;
    }

    void Update()
    {
        if (shouldRotate)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Determine the rotation direction
            if (rotateClockwise)
            {
                targetRotationY = initialRotationY + rotationRange;
            }
            else
            {
                targetRotationY = initialRotationY - rotationRange;
            }

            // Interpolate towards the target rotation
            float currentRotationY = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationY, rotationAmount);
            transform.eulerAngles = new Vector3(0f, currentRotationY, 0f);

            // Change rotation direction if the target rotation is reached
            if (Mathf.Abs(targetRotationY - currentRotationY) <= 0.1f)
            {
                rotateClockwise = !rotateClockwise;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(stopTag))
        {
            shouldRotate = false;
        }
    }
}
