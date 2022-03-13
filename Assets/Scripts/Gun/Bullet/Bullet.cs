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
	[SerializeField] GameObject hitPfxGround;
	[SerializeField] GameObject hitPfxEnemy;
	[SerializeField] GameObject hitPfxOther;

	[SerializeField] private GameObject blankGO;
	[SerializeField] GameObject bulletVisual;

	float lifetime = 10f;
    float lifetimeCounter = 0f;

	bool hit = false;
	

    public void Shoot(Vector3 direction)
    {
        rigid.velocity = direction * bulletSpeed;
    }

    private void Update()
    {
		if(hit)
			return;

        lifetimeCounter += Time.deltaTime;
        if(lifetimeCounter >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        BulletHit(collision.gameObject.transform, collision.contacts[0].normal);
    }

    private void BulletHit(Transform hitObj, Vector3 hitNorm)
    {

		if(hit) {
			return;
		}

		bool hitHandled = false;

		/*if(Vector3.Dot(hitNorm, rigid.velocity) > 0f){
			hitNorm = hitNorm * -1;
		}*/

        Enemy enemy = hitObj.GetComponent<Enemy>();
        if (enemy)
        {
            Debug.Log("Hit enemy");
            enemy.Damage(damage);
			if(enemy.IsDead) {
				Tree.StartGrowing(hitObj, spawnedTree, transform.position, Vector3.down);
				Instantiate(hitPfxGround, transform);
			} else {
				Instantiate(hitPfxEnemy, transform);
			}

			hitHandled = true;
		}
        else if(Random.Range(0f, 1f) < treeSpawnChance)
        {
            Debug.Log("Hit other object");

            if (hitObj.gameObject.layer == 9)
            {
                //Create a tree
                GameObject treeParent = Instantiate(blankGO);
                treeParent.transform.position = transform.position;
                treeParent.transform.LookAt(treeParent.transform.position + Vector3.up);
                Tree.StartGrowing(treeParent.transform, spawnedTree, transform.position, Vector3.down);
                treeParent.transform.LookAt(treeParent.transform.position + hitNorm);
				//treeParent.transform.rotation = Quaternion.Euler(treeParent.transform.rotation.x, 0, treeParent.transform.rotation.x);
				//treeParent.transform.rotation = Quaternion.Euler(0, 0, treeParent.transform.rotation.z);

				Instantiate(hitPfxGround, treeParent.transform);

				hitHandled = true;
			}
        }

		if(hitHandled == false) {
			Instantiate(hitPfxOther, transform);
		}

		StartCoroutine(HideAndDestroyAfterTime());
    }

	IEnumerator HideAndDestroyAfterTime() {

		hit = true;
		bulletVisual.SetActive(false);
		rigid.velocity = Vector3.zero;

		yield return new WaitForSeconds(.5f);

		Destroy(gameObject);
	}

}
