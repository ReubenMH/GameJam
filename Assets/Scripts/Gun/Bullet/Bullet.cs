using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float bulletSpeed;
    [SerializeField] Tree.TreeConfig spawnedTree;
    [SerializeField] float treeSpawnChance;

    public void Shoot(Vector3 direction)
    {
        rigid.velocity = direction * bulletSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        BulletHit();
    }

    private void BulletHit()
    {
        if(Random.Range(0f, 1f) < treeSpawnChance)
        {
            //Create a tree
            Tree.StartGrowing(spawnedTree, transform.position);
        }

        Destroy(gameObject);
    }
}
