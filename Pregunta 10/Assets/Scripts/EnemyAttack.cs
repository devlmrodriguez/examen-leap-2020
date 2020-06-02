using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttack
{
    [SerializeField]
    private float attackDuration;
    [SerializeField]
    private string[] attackTags;
    [SerializeField]
    private string[] reactionTags;

    public float GetAttackDuration()
    {
        return attackDuration;
    }

    public string[] GetAttackTags()
    {
        return attackTags;
    }

    public string[] GetReactionTags()
    {
        return reactionTags;
    }
}
