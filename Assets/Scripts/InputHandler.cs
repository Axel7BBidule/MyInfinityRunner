using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction transformAction;

    void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController non assigné, il faut le glisser déposer.");
            return;
        }


        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        transformAction = InputSystem.actions.FindAction("Transform");


        jumpAction.performed += OnJump;
        crouchAction.performed += OnCrouch;
        transformAction.performed += OnTransform;
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        playerController.Move(moveInput);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        playerController.Jump();

    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        playerController.Crouch();
    }

    private void OnTransform(InputAction.CallbackContext context)
    {
        playerController.Transform();
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJump;
        crouchAction.performed -= OnCrouch;
        transformAction.performed -= OnTransform;
    }
}