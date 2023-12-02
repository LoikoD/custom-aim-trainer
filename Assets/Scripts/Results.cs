using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results
{
    // Score
    public int Score { get; private set; }

    // Accuracy in %
    public float Accuracy { get; private set; }

    // Average time to hit in ms 
    public float AvgTimeToHit { get; private set; }

    // Whether shots were fired or not
    public bool ShotsFired { get; private set; }

    public void Calculate(CurrentGameState gameState)
    {
        if (gameState.ShotsCount > 0)
        {
            Accuracy = ((float)gameState.CurrentScore / gameState.ShotsCount) * 100f;
            AvgTimeToHit = (gameState.SumTimeToHit / gameState.CurrentScore) * 1000f;
            ShotsFired = true;
        }
        else
        {
            Accuracy = 0f;
            AvgTimeToHit = 0f;
            ShotsFired = false;
        }
        Score = gameState.CurrentScore;
    }
}