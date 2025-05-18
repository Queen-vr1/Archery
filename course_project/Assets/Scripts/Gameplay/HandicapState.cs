using UnityEngine;

public class HandicapState
{
    public float TargetPoints { get; private set; } = 0;
    public int FewerBalloons { get; private set; } = 0;
    public float SizePenalty  { get; private set; } = 0;
    public int LessTime { get; private set; } = 0;

    public void Apply(HandicapType type)
    {
        switch (type)
        {
            case HandicapType.HigherTargetScore:
                TargetPoints *= 1.2f;
                Debug.Log("Handicap TargetPoints: " + TargetPoints);
                break;

            case HandicapType.FewerBalloons:
                FewerBalloons += 3;
                Debug.Log("Handicap FewerBalloons: " + FewerBalloons);
                break;

            case HandicapType.SmallerBalloons:
                SizePenalty += 0.05f;
                Debug.Log("Handicap SizePenalty : " + SizePenalty );
                break;

            case HandicapType.LessTime:
                LessTime += 5;
                Debug.Log("Handicap LessTime: " + LessTime);
                break;
        }
    }

    public void Reset() 
    {
        TargetPoints = 0;
        FewerBalloons = 0;
        SizePenalty = 0;
        LessTime = 0;
    }
}
