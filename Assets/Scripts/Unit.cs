using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public int attack;
    public int health;

    private void Awake()
    {
        attack = unitData.attack;
        health = unitData.health;
    }

    void Attack(Unit target)
    {
        target.Hit(attack);
    }

    void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
