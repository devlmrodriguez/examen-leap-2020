using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private List<Enemy> enemies;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float angle;

    private bool hasCameraRenderedOnce;

    private void Awake()
    {
        hasCameraRenderedOnce = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasCameraRenderedOnce)
        {
            Enemy lockedOnEnemy = LockOn();
            if (lockedOnEnemy != null)
                Debug.Log("Enemigo encontrado: " + lockedOnEnemy.name + " con distancia de " + distance.ToString() + " y ángulo de " + angle.ToString() + ".");
            else
                Debug.Log("Enemigo NO encontrado con distancia de " + distance.ToString() + " y ángulo de " + angle.ToString() + ".");
        }
    }

    private void LateUpdate()
    {
        hasCameraRenderedOnce = true;
    }

    public Enemy LockOn()
    {
        List<Enemy> activeEnemies = new List<Enemy>();

        //Castear una esfera de radio igual a la distancia asignada
        Collider[] colliders = Physics.OverlapSphere(player.GetPosition(), distance, enemyMask);
        foreach(Collider collider in colliders)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if(enemy != null & enemy.IsRendering())
            {
                //Obtener el producto punto entre la dirección hacia el enemigo y la dirección del jugador
                //Si el producto punto es 1, el enemigo esta exactamente enfrente de nosotros
                //Si el producto punto es 0, el enemigo esta a la derecha o izquierda de nosotros
                //Para detectar si el enemigo esta dentro de un ángulo de visión, comparar el producto punto contra el coseno del ángulo designado
                Vector3 playerToCollider = (enemy.GetPosition() - player.GetPosition()).normalized;
                float dotProduct = Vector3.Dot(playerToCollider, player.GetDirection());

                if(dotProduct >= Mathf.Cos(Mathf.Deg2Rad * angle))
                    activeEnemies.Add(enemy);
            }
        }

        if (activeEnemies.Count > 0)
            return activeEnemies[0];
        else
            return null;
    }
}
