using UnityEngine;
using UnityEngine.InputSystem;

// Bens Version, extended from Eriks code
public class RefinedGrab : MonoBehaviour
{
    public Transform grabOrigin;
    public float grabRadius = 0.1f;

    public bool triggerPressed;

    private GrabbableObject highlightedObject;
    private GrabbableObject heldObject;

    // InputActionReference for the trigger action
    [SerializeField] InputActionReference triggerInput;
    private InputAction triggerAction;
    // Debug to show input value
    public float triggerValue;

    // Velocites for throwing
    private Vector3 velocity;
    private Vector3 angularVelocity;
    // Positions for calucating velocites
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    [SerializeField] 
    [Header("Velocity Settings")] private float velocityScaleMultiplier = 1f;
    [SerializeField] 
    private float velocityThreshold = 6f;
    

    private void Awake()
    {
        triggerAction = triggerInput;
        triggerAction.Enable(); // Enable the InputAction
    }

    // This runs for EVERY physics step.
    private void FixedUpdate()
    {
        if (heldObject == null) return;

        // Calculate velocity
        Vector3 displacement = heldObject.transform.position - previousPosition;
        velocity = displacement / Time.fixedDeltaTime;
        velocity *= velocityScaleMultiplier;

        // If velocity is below threshold, set to minimum velocity
        if (velocity.magnitude < velocityThreshold)
        {
            velocity = velocity.normalized * velocityThreshold;
        }

        // Calculate angular velocity
        Quaternion delta = heldObject.transform.rotation * Quaternion.Inverse(previousRotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        angularVelocity = (Mathf.Deg2Rad * angle / Time.fixedDeltaTime) * axis.normalized;

        // Update previous position data
        previousPosition = heldObject.transform.position;
        previousRotation = heldObject.transform.rotation;
    }

    private void Update()
    {
        // Update the trigger action value, and the bool triggerPressed every frame
        triggerValue = triggerAction.ReadValue<float>();
        if(triggerValue > 0f)
        {
            triggerPressed = true;
        }
        else
        {
            triggerPressed = false;
        }
        

        // If this hand is holding an object this frame
        if (heldObject != null)
        {
            
            // If trigger was released this frame, release object, and apply velocities from hand (calculated from distance over last frame)
            if (!triggerPressed)
            {
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.GetComponent<Rigidbody>().velocity = velocity;
                heldObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
                Debug.Log("Releasing object, Velocity: " + velocity + " AngularVelocity: " + angularVelocity);

                // Pull the pin on the grenade - if it's a grenade
                GrenadeScript grenadeScript = heldObject.GetComponent<GrenadeScript>();
                if (grenadeScript != null)
                {
                    grenadeScript.pinPulled = true;
                }

                heldObject = null;
            }
        }
        // Else, hand is empty, so allow interactions
        else
        {
            // If there is already an object being highlighted, clear it
            if (highlightedObject != null)
            {
                highlightedObject.SetHighlight(false);
                highlightedObject = null;
            }

            // Array of all colliders within grab radius from grab origin
            Collider[] cols = Physics.OverlapSphere(grabOrigin.position, grabRadius);

            // Did we hit anything at all?
            foreach (Collider col in cols)
            {
                GrabbableObject grabbable = col.GetComponent<GrabbableObject>();

                if (grabbable != null)
                {
                    // Grab the object if the user wants to (i.e., presses the trigger).
                    if (triggerPressed)
                    {
                        heldObject = grabbable;
                        // Attach held object to this hand
                        heldObject.transform.parent = transform;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                        // Call PlayAudio() on the held object 
                        heldObject.PlayAudio();
                    }
                    else // Else, user is not pressing trigger
                    {
                        highlightedObject = grabbable;
                        highlightedObject.SetHighlight(true);
                    }

                    // Exit the loop, we've found something to grab!
                    break;
                }
            }
        }
    }

  


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(grabOrigin.position, grabRadius);
    }
}
