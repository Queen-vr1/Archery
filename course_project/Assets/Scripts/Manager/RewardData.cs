using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RewardData
{
    public RewardCategory category;
    public RewardModifier modifier;
    public float value;

    public string GetDescription()
    {
        string categoryStr = "";
        string modifierStr = "";
        string valueStr = value.ToString();

        switch (category)
        {
            case RewardCategory.Points:
                categoryStr = "Points";
                break;
            case RewardCategory.Money:
                categoryStr = "Money";
                break;
            case RewardCategory.InvestmentBoost:
                categoryStr = "Investment Boost";
                break;
        }

        switch (modifier)
        {
            case RewardModifier.Flat:
                modifierStr = "Flat";
                break;
            case RewardModifier.Multiplier:
                modifierStr = "Multiplier";
                break;
            case RewardModifier.Power:
                modifierStr = "Power";
                break;
        }

        return $"Reward type: {categoryStr} modifier {modifierStr} y valor {valueStr}.";
    }
}
