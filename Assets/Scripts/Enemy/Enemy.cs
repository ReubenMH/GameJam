using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerControl targetPlayer;

    [SerializeField] float enemySpeed;
    [SerializeField] Rigidbody rigid;
    [SerializeField] int maxHealth;
    [SerializeField] private float explodeForce;
    [SerializeField] private float explodeRadius;
    [SerializeField] private float explodeTriggerDist;
    int health;
    [SerializeField] List<MeshRenderer> skinRenderers;
    [SerializeField] Material deadMat;
    [SerializeField] private GameObject explosion;

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

        if ((targetPlayer.transform.position - transform.position).magnitude < explodeTriggerDist)
        {
            Debug.Log("Boom");
            targetPlayer.GetComponent<Rigidbody>().AddForce(explodeForce * targetVector);
            Die();
        }
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
        health = 0;
        GameObject exp = Instantiate(explosion);
        exp.transform.position = transform.position;
        StartCoroutine(DestroyExplosion(exp));
        foreach (MeshRenderer mr in skinRenderers)
        {
            mr.material = deadMat;
        }
        rigid.useGravity = true;
    }

    IEnumerator DestroyExplosion(GameObject expInst)
    {
        yield return new WaitForSeconds(1f);
        Destroy(expInst);
    }
}
