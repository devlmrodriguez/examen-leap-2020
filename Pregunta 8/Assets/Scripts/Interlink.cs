using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interlink : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float triggerProbability;
    [SerializeField]
    private float bounceTriggerProbability;
    [SerializeField]
    private int maxBounces;
    [SerializeField]
    private LayerMask enemyMask;

    private int currentBounces;
    private HashSet<Enemy> enemySet;

    private void Awake()
    {
        enemySet = new HashSet<Enemy>();
    }

    public void Execute(Enemy enemy)
    {
        currentBounces = 0;
        enemySet.Clear();
        Bounce(enemy, triggerProbability);
    }
    
    private void Bounce(Enemy enemy, float assignedProbability)
    {
        float probability = Random.Range(0f, 1f);
        if (probability <= assignedProbability)
        {
            currentBounces++;
            List<Enemy> activeEnemies = GetActiveEnemies(enemy);

            if (activeEnemies.Count > 0)
            {
                int randomIndex = Random.Range(0, activeEnemies.Count);
                Enemy selectedEnemy = activeEnemies[randomIndex];

                //Añadimos el enemigo al set para que no vuelva a ser dañado por el interlink
                enemySet.Add(selectedEnemy);
                selectedEnemy.DoDamage(damage);

                Debug.Log("Dañado enemigo con Interlink: " + enemy.name);

                //Rebotamos el interlink solo si no hemos pasado la cantidad máxima de bounces
                if(currentBounces + 1 <= maxBounces)
                    Bounce(selectedEnemy, bounceTriggerProbability);
            }
        }
    }

    private List<Enemy> GetActiveEnemies(Enemy enemy)
    {
        //Obtiene la lista de enemigos haciendo un raycast a la derecha e izquierda del enemigo asignado
        RaycastHit2D[] rightHits = Physics2D.RaycastAll(enemy.transform.position, Vector2.right, distance);
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(enemy.transform.position, Vector2.left, distance);

        List<Enemy> activeEnemies = new List<Enemy>();

        //Solo obtendremos enemigos que aún no hemos dañado
        foreach (RaycastHit2D hit in rightHits)
        {
            if (hit.collider != null)
            {
                Enemy activeEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (activeEnemy != null && !enemySet.Contains(activeEnemy))
                    activeEnemies.Add(activeEnemy);
            }
        }

        foreach (RaycastHit2D hit in leftHits)
        {
            if (hit.collider != null)
            {
                Enemy activeEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (activeEnemy != null && !enemySet.Contains(activeEnemy))
                    activeEnemies.Add(activeEnemy);
            }
        }

        return activeEnemies;
    }
}
