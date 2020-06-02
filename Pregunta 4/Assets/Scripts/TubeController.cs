using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : MonoBehaviour
{
    [SerializeField]
    private TubeShape shape;
    [SerializeField]
    private TubeDirection direction;
    [SerializeField]
    private GridManager gridManager;

    private List<Vector2Int> localEdges;
    private bool isRotating;

    private void Awake()
    {
        GenerateLocalEdges();
    }

    private void GenerateLocalEdges()
    {
        localEdges = new List<Vector2Int>();

        //Los bordes son básicamente vectores enteros locales que apuntan a donde conectará
        //el tubo, pero la coordenada "y" debe ser inversa ya que el origen del grid esta al revés
        switch (shape)
        {
            case TubeShape.START_END:
                localEdges.Add(new Vector2Int(1, 0));
                break;
            case TubeShape.CURVE:
                localEdges.Add(new Vector2Int(-1, 0));
                localEdges.Add(new Vector2Int(0, 1));
                break;
            case TubeShape.BAR:
                localEdges.Add(new Vector2Int(-1, 0));
                localEdges.Add(new Vector2Int(1, 0));
                break;
        }

        //Rotar los bordes locales cuantas veces hayamos rotado desde la posición original
        int times = 0;
        switch (direction)
        {
            case TubeDirection.DOWN:
                times = 1;
                break;
            case TubeDirection.LEFT:
                times = 2;
                break;
            case TubeDirection.UP:
                times = 2;
                break;
        }

        for (int i = 0; i < times; i++)
            RotateLocalEdgesClockwise();
    }

    private void RotateLocalEdgesClockwise()
    {
        for (int i = 0; i < localEdges.Count; i++)
        {
            //Aplicar una rotación de -90° simple, T(x,y) = <y, -x>
            //Pero como estamos usando índices de arreglo, y el origen esta arriba izquierda en vez de abajo izquierda
            //La transformación queda como T(x,y) = <-y, x>
            Vector2Int rotatedEdge = new Vector2Int(-localEdges[i].y, localEdges[i].x);
            localEdges[i] = rotatedEdge;
        }
    }

    private IEnumerator _RotateClockwise()
    {
        isRotating = true;
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, 0f, -90f);

        //Rotar suavemente para dar una animación continua
        float timeStep = 0.1f;
        for (float t = 0; t <= 1f; t += timeStep)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            yield return null;
        }

        //Actualizar a la nueva dirección
        switch (direction)
        {
            case TubeDirection.RIGHT:
                direction = TubeDirection.DOWN;
                break;
            case TubeDirection.DOWN:
                direction = TubeDirection.LEFT;
                break;
            case TubeDirection.LEFT:
                direction = TubeDirection.UP;
                break;
            case TubeDirection.UP:
                direction = TubeDirection.RIGHT;
                break;
        }

        //Rotar completamente todos los demás atributos
        transform.rotation = targetRotation;
        RotateLocalEdgesClockwise();
        gridManager.OnTubeRotated();
        isRotating = false;
    }

    public TubeShape GetShape()
    {
        return shape;
    }

    public List<Vector2Int> GetLocalEdges()
    {
        return localEdges;
    }

    public void OnButtonClicked()
    {
        if (!isRotating)
            StartCoroutine(_RotateClockwise());
    }
}
