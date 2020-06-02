using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private string enemyType;
    [SerializeField]
    private EnemyAttack[] enemyAttacks;

    private bool canAttack;

    private void Awake()
    {
        canAttack = true;
    }

    public void Attack(EnemyAttack enemyAttack)
    {
        OrchestratorSystem.Instance.OnEnemyAttack(this, enemyAttack);
    }

    public void AttackRandomly()
    {
        int randomIndex = Random.Range(0, enemyAttacks.Length);
        Attack(enemyAttacks[randomIndex]);
    }

    public void OnOtherEnemyAttack(Enemy enemy)
    {
        canAttack = false;
    }

    public void OnOtherEnemyAttacked(Enemy enemy)
    {
        canAttack = true;
    }

    public string GetEnemyType()
    {
        return enemyType;
    }

    public EnemyAttack[] GetEnemyAttacks()
    {
        return enemyAttacks;
    }

    public bool CanAttack()
    {
        return canAttack;
    }
}
