using UnityEngine;
using System.Collections.Generic;

// Abstract base class for all balloon types.
public abstract class ObjectGameplay : MonoBehaviour
{
    // Indicates whether this balloon type is unlocked by the player.
    public bool IsUnlocked { get; private set; } = false;

    // The current level of this object. Affects its cost, effects, etc.
    public int Level { get; private set; } = 0;

    // Returns the cost to unlock this balloon type in the shop.
    public abstract int GetUnlockCost();

    // Returns the cost to upgrade this balloon to the next level.
    public abstract int GetUpgradeCost();

    // Increases the level of the balloon. Can affect its effects, stats, and cost.
    public virtual void Upgrade()
    {
        Level++;
    }

    // Unlock the object. This can be called when the player unlocks it in the shop.
    public void Unlock()
    {
        IsUnlocked = true;
    }
}