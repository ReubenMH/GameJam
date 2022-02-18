using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float bulletSpeed;
    [SerializeField] Tree.TreeConfig spawnedTree;
    [SerializeField] float treeSpawnChance;
    [SerializeField] int damage;

    float lifetime = 10f;
    float lifetimeCounter = 0f;

    public void Shoot(Vector3 direction)
    {
        rigid.velocity = direction * bulletSpeed;
    }

    private void Update()
    {
        lifetimeCounter += Time.deltaTime;
        if(lifetimeCounter >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        BulletHit(collision.gameObject.transform);
    }

    private void BulletHit(Transform hitObj)
    {
        Enemy enemy = hitObj.GetComponent<Enemy>();
        if (enemy)
        {
            Debug.Log("Hit enemy");
            enemy.Damage(damage);
            if (enemy.IsDead)
            {
                Tree.StartGrowing(hitObj, spawnedTree, transform.position);
            }
        }
        else if(Random.Range(0f, 1f) < treeSpawnChance)
        {
            Debug.Log("Hit other object");
            //Create a tree
            Tree.StartGrowing(null, spawnedTree, transform.position);
        }

        Destroy(gameObject);
    }
}
