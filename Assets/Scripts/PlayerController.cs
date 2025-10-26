using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float scrollerSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 20f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchDuration = 3f;
    [SerializeField] private float crouchCooldownDuration = 5f;


    [Header("Transform Settings")]
    [SerializeField] private float transformDuration = 4f;
    [SerializeField] private float cooldownTransform = 6f;

    [Header("Debug Colors")]
    [SerializeField] private Color crouchColor = Color.blue;
    [SerializeField] private Color cooldownColor = Color.red;
    [SerializeField] private Color transformColor = Color.green;

    private Renderer playerRenderer;
    private Color actualColor;

    private CharacterController characterController;
    private Vector3 originalScale;
    private bool isCrouching = false;
    private bool crouchOnCooldown = false;
    private bool isTransformActive = false;
    private Vector3 crouchScale = new Vector3(2f, 0.5f, 2f);
    private Vector3 velocity;
    private bool isGrounded;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    public void Move(Vector2 moveInput)
    {
        isGrounded = characterController.isGrounded;

        Vector3 directionMove = transform.right * moveInput.x;
        directionMove *= moveSpeed * Time.deltaTime;

        Vector3 forwardMove = transform.forward * scrollerSpeed * Time.deltaTime;


        velocity.y -= gravity * Time.deltaTime;

        Vector3 move = directionMove + forwardMove + velocity * Time.deltaTime;
        characterController.Move(move);
    }

    public void Jump()
    {

        if (!isCrouching && isGrounded)
        {
            Debug.Log("Jump demandée");
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
        }
    }

    public void Crouch()
    {
        if (!isCrouching && !crouchOnCooldown)
        {
            Debug.Log("Crouch demandée");
            StartCoroutine(CrouchCoroutine());
        }
    }

    public void Transform()
    {

        if (!isCrouching && !isTransformActive && !crouchOnCooldown)
        {
            Debug.Log("Transformation demandée");
            StartCoroutine(TransformCoroutine());
        }
    }

    //Couroutine 

    private IEnumerator TransformCoroutine()
    {
        isTransformActive = true;
        playerRenderer.material.color = transformColor;
        yield return new WaitForSeconds(transformDuration);

        playerRenderer.material.color = cooldownColor;
        crouchOnCooldown = true;
        yield return new WaitForSeconds(cooldownTransform);
        isTransformActive = false;
        playerRenderer.material.color = actualColor;
        crouchOnCooldown = false;
    }

    private IEnumerator CrouchCoroutine()
    {
        isCrouching = true;
        transform.localScale = crouchScale;

        yield return new WaitForSeconds(crouchDuration);


        transform.localScale = originalScale;
        isCrouching = false;
        crouchOnCooldown = true;
        playerRenderer.material.color = cooldownColor;

        yield return new WaitForSeconds(crouchCooldownDuration);
        playerRenderer.material.color = actualColor;
        crouchOnCooldown = false;
    }
}




