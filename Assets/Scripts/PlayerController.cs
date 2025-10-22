using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using NUnit.Framework;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float scrollerSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 3f;

    [Header("Ceiling Detection")]
    [SerializeField] private LayerMask ceilingLayer = 1 << 6;
    [SerializeField] private float ceilingCheckDistance = 0.2f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchDuration = 3f;
    [SerializeField] private float crouchCooldownDuration = 5f;
    [SerializeField] private Vector3 crouchScale = new Vector3(2f, 0.5f, 2f);

    [Header("Transform Settings")]
    [SerializeField] private float transformDuration = 4f;

    [Header("Debug Colors")]
    [SerializeField] private Color crouchColor = Color.blue;
    [SerializeField] private Color cooldownColor = Color.red;
    [SerializeField] private Color transformColor = Color.green;

    private Renderer playerRenderer;
    private Color actualColor;

    private CharacterController characterController;
    private float verticalVelocity;
    private Vector3 originalScale;
    private bool isStickingToCeiling = false;
    private bool jumpOnCooldown = false;
    private bool isCrouchingActive = false;
    private bool crouchOnCooldown = false;
    private bool isTransformActive = false;
    private float gravity = 30f;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    public void Move(Vector2 moveInput)
    {

        Vector3 directionMove = transform.right * moveInput.x;
        directionMove *= moveSpeed * Time.deltaTime;


        Vector3 forwardMove = transform.forward * scrollerSpeed * Time.deltaTime;


        Vector3 move = directionMove + forwardMove;
        characterController.Move(move);


        JumpMovement();

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void JumpMovement()
    {
        if (isStickingToCeiling)
        {
            if (IsCeilingAbove())
            {
                Vector3 rayOrigin = transform.position + Vector3.up * (characterController.height * 0.5f);
                if (Physics.Raycast(rayOrigin, Vector3.up, out RaycastHit hit, ceilingCheckDistance, ceilingLayer))
                {
                    float distanceToCeiling = hit.distance;
                    verticalVelocity = Mathf.Min(0.5f, distanceToCeiling * 10f);
                }
            }
            else
            {
                verticalVelocity = 0;
            }
        }
        else
        {
            if (verticalVelocity > 0 && IsCeilingAbove())
            {
                isStickingToCeiling = true;
                verticalVelocity = 0;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }
    }

    public void Jump()
    {
        if (jumpOnCooldown) return;

        if (characterController.isGrounded && !isCrouchingActive)
        {
            verticalVelocity = jumpForce;
            isStickingToCeiling = false;
            StartCoroutine(JumpCooldownCoroutine());
        }
        else if (isStickingToCeiling && !isCrouchingActive)
        {
            verticalVelocity = -jumpForce;
            isStickingToCeiling = false;
            StartCoroutine(JumpCooldownCoroutine());
        }
    }

    private bool IsCeilingAbove()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (characterController.height * 0.5f);
        return Physics.Raycast(rayOrigin, Vector3.up, ceilingCheckDistance, ceilingLayer);
    }

    public void Crouch()
    {
        if (!isCrouchingActive && !crouchOnCooldown)
        {
            StartCoroutine(CrouchCoroutine());
        }
    }

    public void Transform()
    {
        Debug.Log("Transformation demandée");
        if (!isCrouchingActive && !isTransformActive)
        {
            StartCoroutine(TransformCoroutine());
        }
    }

    //Couroutine 

    private IEnumerator TransformCoroutine()
    {
        isTransformActive = true;
        playerRenderer.material.color = transformColor;
        yield return new WaitForSeconds(transformDuration);
        playerRenderer.material.color = actualColor;
        isTransformActive = false;
    }

    private IEnumerator CrouchCoroutine()
    {
        isCrouchingActive = true;
        transform.localScale = crouchScale;

        yield return new WaitForSeconds(crouchDuration);


        transform.localScale = originalScale;
        isCrouchingActive = false;
        crouchOnCooldown = true;
        playerRenderer.material.color = cooldownColor;

        yield return new WaitForSeconds(crouchCooldownDuration);
        playerRenderer.material.color = actualColor;
        crouchOnCooldown = false;
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        jumpOnCooldown = true;
        playerRenderer.material.color = cooldownColor;
        yield return new WaitForSeconds(jumpCooldown);
        playerRenderer.material.color = actualColor;
        jumpOnCooldown = false;
    }

}

