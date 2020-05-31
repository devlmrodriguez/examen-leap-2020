using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer enemyRenderer;

    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsRendering()
    {
        return enemyRenderer.isVisible;
    }
}
