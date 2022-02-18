using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerControl targetPlayer;

    [SerializeField] float enemySpeed;
    [SerializeField] Rigidbody rigid;
    [SerializeField] int maxHealth;
    int health;

    public bool IsDead => health <= 0;

    public void SetTarget(PlayerControl target)
    {
        targetPlayer = target;

        health = maxHealth;
    }

    private void Update()
    {
        if (IsDead)
            return;

        Vector3 targetVector = targetPlayer.transform.position - transform.position;
        targetVector.Normalize();

        rigid.velocity = targetVector * enemySpeed;

        transform.LookAt(targetPlayer.transform);
    }

    public void Damage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        rigid.useGravity = true;
    }
}
