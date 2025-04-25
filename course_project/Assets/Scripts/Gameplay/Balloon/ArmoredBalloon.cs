using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class ArmoredBalloon : Balloon
{
    private int health = 3;
    private Vector3 moveDirection;
    
    [SerializeField] private int _verticalSpeed = 5;
    [SerializeField] private int _horizontalSpeed = 10;

    [SerializeField] private int rewardAmount = 500;

    public override int verticalSpeed
    {
        get => _verticalSpeed;
        set => _verticalSpeed = value;
    }

    public override int horizontalSpeed
    {
        get => _horizontalSpeed;
        set => _horizontalSpeed = value;
    }

    public override int maxHeight => 30; // Altura máxima del globo explorador
    public override int minHeight => 4; // Altura mínima del globo explorador
    
    public override void Init()
    {
        Debug.Log("Armored Balloon ready!");
        rewards = new List<RewardData>();

        rewards.Add(new RewardData
        {
            category = RewardCategory.Points,
            modifier = RewardModifier.Multiplier,
            value = 1.2f
        });
    }

    void Update()
    {
        //Move();
    }


    public override void Move() {
        Vector3 position = transform.position;

        if (position.y >= maxHeight)
        {
            verticalSpeed = -Mathf.Abs(verticalSpeed); // bajar
        }
        else if (position.y <= minHeight)
        {
            verticalSpeed = Mathf.Abs(verticalSpeed); // subir
        }

        // Movimiento horizontal y vertical
        Vector3 horizontalMove = moveDirection * horizontalSpeed * Time.deltaTime;
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

        Animator anim;
        anim = GetComponent<Animator>(); 
        anim.SetBool("isHit", true); 
        StartCoroutine(WaitForAnimationEnd(anim));
       // anim.SetBool("isHit", false); 
    }
    public override bool IsDestroyed()
    {
        return health <= 0;
    }
    public override void OnDestroyed()
    {
        Animator anim;
        anim = GetComponent<Animator>(); 
        anim.SetBool("isPop", true); 
    }

    public override BalloonTypeName GetBalloonType()
    {
        return BalloonTypeName.Armored;
    }

    public override int GetUnlockCost()
    {
        return 10;
    }

    public override int GetUpgradeCost()
    {
        return 5;
    }

    IEnumerator WaitForAnimationEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.001f); 
        anim.SetBool("isHit", false); 
    }
}
