using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntEventChannelSO collectableEventChannel;
    [SerializeField] private IntEventChannelSO updateScoreEventChannel;
    private int score = 0;

    public void Start()
    {
        updateScoreEventChannel.RaiseEvent(score);
    }

    private void OnEnable()
    {
        collectableEventChannel.OnEventRaised += AddScore;
    }
    private void OnDisable()
    {
        collectableEventChannel.OnEventRaised -= AddScore;
    }
    public void AddScore(int score)
    {
        this.score += score;
        updateScoreEventChannel.RaiseEvent(this.score);
    }
}
