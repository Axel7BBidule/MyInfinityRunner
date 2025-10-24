using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollected;
    [SerializeField] private string collectableTag = "Collectable";

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collectableTag))
        {
            onCollected?.Invoke();
        }

    }
}
