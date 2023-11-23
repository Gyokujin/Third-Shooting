using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyMaxHP = 100;
    public int enemyCurrentHP = 0;

    void Start()
    {
        InitEnemyHP();
    }

    void InitEnemyHP()
    {
        enemyCurrentHP = enemyMaxHP;
    }
}