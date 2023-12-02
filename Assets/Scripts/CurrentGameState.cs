using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

public class CurrentGameState
{

    public event Action ScoreChanged = delegate { };
    public event Action TimeChanged = delegate { };

    private int currentTime;
    private int currentScore;
    private float lastHitTime;

    public int CurrentTime
    {
        get => currentTime;
        set
        {
            currentTime = value;
            TimeChanged();
        }
    }
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            ScoreChanged();
        }
    }
    public float LastHitTime
    {
        get => lastHitTime;
        set
        {
            // Add "new hit time" - "last hit time" to the sum
            SumTimeToHit += value - LastHitTime;

            lastHitTime = value;
        }
    }
    public float SumTimeToHit { get; set; }
    public int ShotsCount { get; set; }

    public void Init(int time)
    {
        CurrentTime = time;
        CurrentScore = 0;
        LastHitTime = Time.time;
        SumTimeToHit = 0; // important: SumTimeToHit is set after LastHitTime has been set
        ShotsCount = 0;
    }
}