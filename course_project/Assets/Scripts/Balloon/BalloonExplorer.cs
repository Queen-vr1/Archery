using System.Collections;
using UnityEngine;

public class BalloonExplorer : Balloon
{
    private int health = 10;
    private Vector3 moveDirection;
    private float speed = 10f;
    private float verticalSpeed = 5f;
    public override void Init()
    {
        Debug.Log("Globo explorador listo!");
    }

    void Update()
    {
        Debug.Log("Globo explorador moviendose");
        Move();
    }


    public override void Move() {
        Vector3 horizontalMove = moveDirection * speed * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * verticalSpeed * Time.deltaTime;
        transform.position += horizontalMove + verticalMove;
    }

    public override void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            OnDestroyed();
        }
    }
    public override bool IsDestroyed()
    {
        return health <= 0;
    }
    public override void OnDestroyed()
    {
        Debug.Log("Globo explorador destruido!");
    }

    public override BalloonTypeName GetBalloonType()
    {
        return BalloonTypeName.Explorer;
    }

    public override int GetUnlockCost()
    {
        return 10;
    }

    public override int GetUpgradeCost()
    {
        return 5;
    }
}
