using LearningRace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrchestratorSystem : Singleton<OrchestratorSystem>
{
    [SerializeField]
    private List<Enemy> activeEnemies;
    [SerializeField]
    private int minAttackNumber;
    [SerializeField]
    private float attackProbability;
    [SerializeField]
    private float minTimeCooldown;

    private int currentAttackNumber;
    private float currentTimeCooldown;

    private bool inTimeCooldown;
    private bool isAttackOrchestrated;

    private void Update()
    {
        if (inTimeCooldown)
        {
            currentTimeCooldown -= Time.deltaTime;
            if (currentTimeCooldown < 0f)
            {
                Debug.Log("La ventana de tiempo de cooldown ha terminado");
                currentTimeCooldown = 0f;
                inTimeCooldown = false;
            }
        }
    }

    public void OnEnemyAttack(Enemy enemy, EnemyAttack enemyAttack)
    {
        StartCoroutine(_OnEnemyAttack(enemy, enemyAttack));
    }

    private IEnumerator _OnEnemyAttack(Enemy enemy, EnemyAttack enemyAttack)
    {
        Debug.Log("El enemigo " + enemy.name + " ha intentado atacar");

        if (currentAttackNumber > 0)
            currentAttackNumber--;
        else
        {
            currentAttackNumber = 0;
            Debug.Log("El mínimo de ataques ha sido cumplido");
        }

        //Solo ejecutar el ataque correctamente si:
        //Primera condición: El ataque no es orquestado y se puede realizar un ataque O
        //Segunda condición: El ataque es orquestado, se puede realizar un ataque, y el mínimo de ataques y tiempo se han cumplido
        //Estas dos condiciones estan en un XOR, solo una de ellas puede ser cumplida, pero no ambas ni ninguna
        if ((!isAttackOrchestrated && enemy.CanAttack()) || (isAttackOrchestrated && enemy.CanAttack() && currentAttackNumber <= 0 && !inTimeCooldown))
        {
            Debug.Log("<color=green>El sistema ha permitido el ataque</color>");

            //Doble verificación en caso se entre a la primera condición de ataque (cuando no es orquestado)
            if(isAttackOrchestrated && enemy.CanAttack() && currentAttackNumber <= 0 && !inTimeCooldown)
            {
                //Reiniciar los ataques y tiempo mínimos en caso se haya realizado satisfactoriamente al ataque orquestrado
                currentAttackNumber = minAttackNumber;
                currentTimeCooldown = minTimeCooldown;
                inTimeCooldown = true;
            }

            //Bloquear el ataque de todos los enemigos del mismo tipo
            for (int i = 0; i < activeEnemies.Count; i++)
                if (activeEnemies[i].GetEnemyType().Equals(enemy.GetEnemyType()))
                    activeEnemies[i].OnOtherEnemyAttack(enemy);

            yield return new WaitForSeconds(enemyAttack.GetAttackDuration());

            //Devolver el ataque de todos los enemigos del mismo tipo
            for (int i = 0; i < activeEnemies.Count; i++)
                if (activeEnemies[i].GetEnemyType().Equals(enemy.GetEnemyType()))
                    activeEnemies[i].OnOtherEnemyAttacked(enemy);

            float randomProbability = Random.Range(0f, 1f);
            if (randomProbability <= attackProbability)
            {
                Enemy reactionEnemy = null;
                EnemyAttack reactionEnemyAttack = null;

                //Obtener un enemigo y ataque de reacción en caso exista uno
                GetReactionEnemyAndAttack(enemyAttack, ref reactionEnemy, ref reactionEnemyAttack);

                if (reactionEnemy != null)
                    ExecuteOrchestratedAttack(enemy, reactionEnemy, reactionEnemyAttack);
            }
        }
        else
        {
            Debug.Log("<color=red>El sistema no ha permitido el ataque porque:</color>");
            if (!enemy.CanAttack())
                Debug.Log("\tUn enemigo del mismo tipo está atacando");
            if (currentAttackNumber > 0)
                Debug.Log("\tEl mínimo de ataques aún no ha sido cumplido");
            if (currentTimeCooldown > 0)
                Debug.Log("\tEl mínimo de tiempo de cooldown aún no ha sido cumplido");
        }

        yield return null;
    }

    private void ExecuteOrchestratedAttack(Enemy enemy, Enemy reactionEnemy, EnemyAttack reactionEnemyAttack)
    {
        Debug.Log("<color=yellow>El sistema ha originado un ataque de orquesta entre enemigo " + enemy.name + " y " + reactionEnemy.name + "</color>");

        //Tratar de realizar un ataque con el flag de orquestado
        isAttackOrchestrated = true;
        enemy.Attack(reactionEnemyAttack);
        isAttackOrchestrated = false;
    }

    public void OnEnemyAttacked(Enemy enemy)
    {
        if (activeEnemies != null)
        {
            //Transmitir el mensaje de ataque a todos los demás enemigos
            for (int i = 0; i < activeEnemies.Count; i++)
                activeEnemies[i].OnOtherEnemyAttack(enemy);
        }
    }

    private void GetReactionEnemyAndAttack(EnemyAttack enemyAttack, ref Enemy reactionEnemy, ref EnemyAttack reactionEnemyAttack)
    {
        //Verificar en todos los enemigos activos los ataques que compartan tags de ataque y reacción, y devolver el primero que se encuentre junto con el ataque elegido
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy activeEnemy = activeEnemies[i];
            EnemyAttack[] activeEnemyAttacks = activeEnemy.GetEnemyAttacks();

            List<EnemyAttack> activeEnemyReactionAttacks = new List<EnemyAttack>();
            for (int j = 0; j < activeEnemyAttacks.Length; j++)
            {
                EnemyAttack activeEnemyAttack = activeEnemyAttacks[j];
                //Detectar si comparten algun reaction tag y attack tag por medio de una intersección de listas y comparar si no es nula
                if (activeEnemyAttack.GetReactionTags().Intersect(enemyAttack.GetAttackTags()) != null)
                    activeEnemyReactionAttacks.Add(activeEnemyAttack);
            }

            if (activeEnemyReactionAttacks.Count > 0)
            {
                reactionEnemy = activeEnemy;

                //Elegir un ataque de reacción aleatorio en caso sea más de uno
                int randomIndex = Random.Range(0, activeEnemyReactionAttacks.Count);
                reactionEnemyAttack = activeEnemyReactionAttacks[randomIndex];

                return;
            }
        }
    }
}
