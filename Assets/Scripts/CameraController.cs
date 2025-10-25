using UnityEngine;

public enum CameraMode
{
    Normal,
    Ceiling,
    Plane
}

public class CameraController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private BoolEventChannelSO isCeilingEvent;
    [SerializeField] private BoolEventChannelSO isPlaneEvent;

    [Header("Camera Modes")]
    [SerializeField] private CameraMode currentMode = CameraMode.Normal;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followSpeed = 5f;

    [Header("Ceiling Mode Settings")]
    [SerializeField] private Transform CameraCeilingFollow;
    [SerializeField] private Transform CameraCeilingLookAt;

    [Header("Plane Mode Settings")]
    [SerializeField] private Transform CameraPlaneFollow;
    [SerializeField] private Transform CameraPlaneLookAt;
    private bool playerIsOnCeiling = false;
    private bool playerIsOnPlane = false;
    private Vector3 targetOffset;
    private Vector3 targetRotation;


    private void Start()
    {
        SetCameraMode(CameraMode.Plane);
    }

    private void OnEnable()
    {
        isCeilingEvent.OnEventRaised += OnCeilingEvent;
        isPlaneEvent.OnEventRaised += OnPlaneEvent;
    }

    private void OnDisable()
    {
        isCeilingEvent.OnEventRaised -= OnCeilingEvent;
        isPlaneEvent.OnEventRaised -= OnPlaneEvent;
    }

    private void OnCeilingEvent(bool isOnCeiling)
    {
        playerIsOnCeiling = isOnCeiling;
        UpdateCameraMode();
    }

    private void OnPlaneEvent(bool isOnPlane)
    {
        playerIsOnPlane = isOnPlane;
        UpdateCameraMode();
    }




    private void UpdateCameraMode()
    {

        if (playerIsOnCeiling)
        {
            SetCameraMode(CameraMode.Ceiling);
        }
        else if (playerIsOnPlane)
        {
            SetCameraMode(CameraMode.Plane);
        }
    }

    private void SetCameraMode(CameraMode newMode)
    {
        if (currentMode == newMode) return;
        currentMode = newMode;

        switch (currentMode)
        {
            case CameraMode.Ceiling:
                targetOffset = CameraCeilingFollow.position - transform.position;
                targetRotation = CameraCeilingLookAt.eulerAngles;
                Debug.Log("[Camera] Mode Ceiling activé");
                break;

            case CameraMode.Plane:
                targetOffset = CameraPlaneFollow.position - transform.position;
                targetRotation = CameraPlaneLookAt.eulerAngles;
                Debug.Log("[Camera] Mode Plane activé");
                break;
        }
    }
    private void LateUpdate()
    {

        Vector3 targetPosition = followTarget.position + targetOffset;


        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);


        Quaternion targetQuat = Quaternion.Euler(targetRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuat, transitionSpeed * Time.deltaTime);

    }
}


