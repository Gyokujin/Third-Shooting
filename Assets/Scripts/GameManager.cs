using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void Shooting(Vector3 targetPosition)
    {
        Instantiate(weaponFlashFX, bulletPoint.position, Quaternion.identity); ;
        Vector3 aim = (targetPosition - bulletPoint.position).normalized;
        Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));
    }
}