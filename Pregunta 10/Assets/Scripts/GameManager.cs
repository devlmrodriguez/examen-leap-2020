using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Enemy frogEnemy;
    [SerializeField]
    private Enemy trunkEnemy;
    [SerializeField]
    private Enemy monkeyEnemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            frogEnemy.AttackRandomly();
        if (Input.GetKeyDown(KeyCode.W))
            trunkEnemy.AttackRandomly();
        if (Input.GetKeyDown(KeyCode.T))
            monkeyEnemy.AttackRandomly();
    }
}
