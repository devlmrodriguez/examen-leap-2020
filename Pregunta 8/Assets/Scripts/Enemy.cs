using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void OnMeleeAttackConnected()
    {
        BattleManager.Instance.OnEnemyMeleeAttacked(this);
    }

    public void DoDamage(int amount)
    {

    }
}
