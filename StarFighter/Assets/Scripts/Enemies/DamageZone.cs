using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class DamageZone : MonoBehaviour, IHitable
{
    [SerializeField] private EnemyBehavior enemyParent;


    public void Hit(int amount)
    {
        enemyParent.Hit(amount);
    }
}
