using UnityEngine;
using System.Collections.Generic;

public static class RewardManager
{
    public static void ApplyAll(List<RewardData> rewards)
    {
        foreach (var reward in rewards)
            ApplyReward(reward);
    }

    public static void ApplyReward(RewardData reward)
    {
        switch (reward.category)
        {
            case RewardCategory.Points:
                ApplyPointsReward(reward);
                break;
            case RewardCategory.Money:
                ApplyMoneyReward(reward);
                break;
            case RewardCategory.InvestmentBoost:
                // implementa aquí
                break;
        }
    }

    private static void ApplyPointsReward(RewardData reward)
    {
        switch (reward.modifier)
        {
            case RewardModifier.Flat:
				ScoreManager.Instance.AddPoints(Mathf.RoundToInt(reward.value));
                break;
            case RewardModifier.Multiplier:
                ScoreManager.Instance.SetMultiplier(
                    ScoreManager.Instance.Multiplier * reward.value);
                break;
            case RewardModifier.Power:
                ScoreManager.Instance.SetMultiplier(
                    Mathf.Pow(ScoreManager.Instance.Multiplier, reward.value));
                break;
        }
    }

    private static void ApplyMoneyReward(RewardData reward)
    {
        int current = GameManager.Instance.Money;

        switch (reward.modifier)
        {
            case RewardModifier.Flat:
                GameManager.Instance.AddMoney(Mathf.RoundToInt(reward.value));
                break;
            case RewardModifier.Multiplier:
                GameManager.Instance.AddMoney(Mathf.RoundToInt(current * (reward.value - 1f)));
                break;
            case RewardModifier.Power:
                GameManager.Instance.AddMoney(Mathf.RoundToInt(Mathf.Pow(current, reward.value)));
                break;
        }
    }
}
