using UnityEngine;
using System.Collections.Generic;

// Abstract base class for all balloon types.
public abstract class Balloon : MonoBehaviour
{
    // Indicates whether this balloon type is unlocked by the player.
    public bool IsUnlocked { get; private set; } = false;

    // The current level of this balloon. Affects its cost, effects, etc.
    public int Level { get; private set; } = 0;

    // Initializes the balloon (each subclass can initialize its own specific properties).
    public abstract void Init(); // cada tipo de globo puede hacer algo distinto

    // Defines how this balloon moves. Different types may float, dash, zigzag, etc.
    public abstract void Move();

    // Reduces the balloon's health when damaged. Used for types like armored balloons.
    public abstract void TakeDamage(int amount);

    // Checks whether the balloon has been destroyed.
    public abstract bool IsDestroyed();

    // Called when the balloon is destroyed: trigger effects, rewards, or removal.
    public abstract void OnDestroyed();

    // Returns the specific balloon type (enum), useful for logic, filtering, and stats.
    public abstract BalloonTypeName GetBalloonType();

    // Returns the cost to unlock this balloon type in the shop.
    public abstract int GetUnlockCost();

    // Returns the cost to upgrade this balloon to the next level.
    public abstract int GetUpgradeCost();

    // Increases the level of the balloon. Can affect its effects, stats, and cost.
    public virtual void Upgrade()
    {
        Level++;
    }

    // This would define what happens when the balloon is collected or hit:
    // giving points, applying score multipliers, gold, or penalties.
    // Uncomment and implement in child classes if needed.
    // public abstract void ApplyEffect(GameManager gameManager);
}
