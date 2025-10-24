using UnityEngine;

public class cubeRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private Vector3 rotationDirection = Vector3.zero;

    void Update()
    {
        transform.Rotate(rotationDirection, rotationSpeed * Time.deltaTime);
    }
}
