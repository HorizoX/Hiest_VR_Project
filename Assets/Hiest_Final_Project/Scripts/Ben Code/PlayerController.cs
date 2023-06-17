using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 3f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private InputActionReference moveActionRef;
    private InputAction moveAction;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float mouseX, mouseY;



    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        // Set up input actions
        moveAction = moveActionRef.action;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;
        moveAction.Enable();
        
    }

    private void Update()
    {
        // Get input for movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        // Move the player
        transform.position += moveDirection;

        // Get input for looking around
        mouseX += Mouse.current.delta.x.ReadValue() * mouseSensitivity;
        mouseY -= Mouse.current.delta.y.ReadValue() * mouseSensitivity;

        // Clamp vertical rotation between -90 and 90 degrees
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // Rotate the camera based on mouse input
        
        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
}
