using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 direction;

    private void Awake()
    {
        direction = Vector3.forward;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetDirection()
    {
        return transform.rotation * direction;
    }
}
