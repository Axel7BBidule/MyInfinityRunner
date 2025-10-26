using Unity.VisualScripting;
using UnityEngine;

public class CollectableCube : MonoBehaviour
{
    [SerializeField] private IntEventChannelSO CollectableEvent;
    [SerializeField] private int score = 100;

    public void OnCollected()
    {
        CollectableEvent.RaiseEvent(score);
        Destroy(gameObject);
    }
}