using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuRaycaster : MonoBehaviour
{
    public float raycastDistance = 10f;
    public LayerMask raycastLayer;
    public Color lineColor = Color.white;
    public float lineThickness = 0.02f;
    public Vector3 forwardVector = Vector3.forward;
    // Reference to an Input action for the trigger to be used for selecting UI elements
    public InputActionReference triggerAction;
    public bool isTriggerPressed = false;
    public float triggerActionValue;

    private LineRenderer lineRenderer;

    void Awake()
    {
        // Enable the input for triggerAction
        triggerAction.action.Enable();
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material.color = lineColor;
    }

    void Update()
    {
        //Update the trigger action value
        triggerActionValue = triggerAction.action.ReadValue<float>();
        // Update the trigger state bool isTriggerPressed
        isTriggerPressed = triggerActionValue > 0.5f;
        
        // Perform raycast
        Vector3 localForwardVector = transform.TransformDirection(forwardVector);
        Ray ray = new Ray(transform.position, localForwardVector);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, raycastDistance, raycastLayer);

        // Update line renderer
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);

        if (hitSomething)
        {
            lineRenderer.SetPosition(1, hit.point);

            //check if the raycast hit a button
            if (hit.collider.gameObject.GetComponent<Button>())
            {
                // Set Button as highlighted
                EventSystem.current.SetSelectedGameObject(hit.collider.gameObject);

                // If trigger is pressed, select the button
                if (isTriggerPressed)
                {
                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }
            }
           
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + localForwardVector * raycastDistance);
            // Clear selected game object
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
        
    }

    
}
