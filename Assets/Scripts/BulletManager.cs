using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigid;

    [SerializeField]
    private float moveSpeed = 10f;

    void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        bulletRigid.velocity = transform.forward * moveSpeed;
    }
}