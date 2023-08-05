using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float angle1 = 150f; // The first target angle you pick
    public float angle2 = 210f; // The second target angle you pick
    public float pauseDuration = 3f; // The duration of the pause in seconds
    public string stopTag = "Slime";
    public Light spotlight; // Reference to the spotlight attached to the camera
    private bool shouldRotate = true;
    private bool isRotating = false;
    private float targetRotationY;
    private int direction = 1; // 1 for clockwise, -1 for counterclockwise

    private void Update()
    {
        if (shouldRotate && !isRotating)
        {
            StartCoroutine(RotateCamera());
        }
    }

    private System.Collections.IEnumerator RotateCamera()
    {
        isRotating = true;

        float currentRotationY = transform.eulerAngles.y;
        float startAngle = (direction == 1) ? angle1 : angle2;
        float endAngle = (direction == 1) ? angle2 : angle1;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            targetRotationY = Mathf.Lerp(startAngle, endAngle, t);
            transform.rotation = Quaternion.Euler(0f, targetRotationY, 0f);
            yield return null;
        }

        // Pause for the specified duration before changing rotation direction
        yield return new WaitForSeconds(pauseDuration);

        // Change rotation direction after the pause
        direction *= -1;

        isRotating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(stopTag))
        {
            shouldRotate = false;
            if (spotlight != null)
            {
                spotlight.enabled = false;
            }
        }
    }
}
