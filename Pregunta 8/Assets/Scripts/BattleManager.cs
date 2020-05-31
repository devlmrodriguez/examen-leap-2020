using LearningRace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField]
    private Interlink interlink;
    [SerializeField]
    private List<Enemy> enemyList;

    private void Start()
    {
        enemyList[0].OnMeleeAttackConnected();
    }

    public void OnEnemyMeleeAttacked(Enemy enemy)
    {
        interlink.Execute(enemy);
    }
}
