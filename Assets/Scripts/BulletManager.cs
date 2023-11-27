using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigid;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float destroyTime = 3f;
    private float currentDestroyTime;

    void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
        currentDestroyTime = destroyTime;
    }

    void Update()
    {
        currentDestroyTime -= Time.deltaTime;

        if (currentDestroyTime <= 0)
        {
            DestroyBullet();
        }

        BulletMove();
    }

    void BulletMove()
    {
        bulletRigid.velocity = transform.forward * moveSpeed;
    }

    void DestroyBullet()
    {
        gameObject.SetActive(false);
        currentDestroyTime = destroyTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().enemyCurrentHP--;
        }

        DestroyBullet();
    }
}