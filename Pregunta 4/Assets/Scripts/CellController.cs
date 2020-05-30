using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public enum CellType
    {
        START_END,
        BAR,
        CURVE
    }

    public enum CellDirection
    {
        RIGHT,
        DOWN,
        LEFT,
        UP
    }

    [SerializeField]
    private CellType type;
    [SerializeField]
    private CellDirection direction;

    private bool isRotating;

    private void Awake()
    {
        transform.rotation = GetQuaternionFromDirection(direction);
        isRotating = false;
    }


    public void OnButtonClicked()
    {
        if(!isRotating)
            StartCoroutine(_RotateClockwise());
    }

    private IEnumerator _RotateClockwise()
    {
        isRotating = true;
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, 0f, -90f);

        float timeStep = 0.1f;
        for(float t = 0; t <= 1f; t += timeStep)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }

    private Quaternion GetQuaternionFromDirection(CellDirection direction)
    {
        switch(direction)
        {
            case CellDirection.RIGHT: return Quaternion.Euler(0f, 0f, 0f);
            case CellDirection.DOWN: return Quaternion.Euler(0f, 0f, -90f);
            case CellDirection.LEFT: return Quaternion.Euler(0f, 0f, -180f);
            case CellDirection.UP: return Quaternion.Euler(0f, 0f, -270f);
        }

        return Quaternion.identity;
    }
}
