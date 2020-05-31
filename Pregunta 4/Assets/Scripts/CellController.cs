using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public enum CellShape
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
    private CellShape shape;
    [SerializeField]
    private CellDirection direction;

    private Vector2Int gridSize;
    private Vector2Int gridPosition;

    private List<Vector2Int> edgeList;
    private List<Vector2Int> edgeGridPositionList;

    private bool isRotating;

    private void Initialize(Vector2Int gridSize, Vector2Int gridPosition)
    {
        this.gridSize = gridSize;
        this.gridPosition = gridPosition;
        GenerateEdges();
        isRotating = false;
    }

    private void GenerateEdges()
    {
        edgeList = new List<Vector2Int>();

        switch (shape)
        {
            case CellShape.START_END:
                edgeList.Add(new Vector2Int(1, 0));
                break;
            case CellShape.CURVE:
                edgeList.Add(new Vector2Int(-1, 0));
                edgeList.Add(new Vector2Int(0, -1));
                break;
            case CellShape.BAR:
                edgeList.Add(new Vector2Int(-1, 0));
                edgeList.Add(new Vector2Int(1, 0));
                break;
        }

        int times = 0;
        switch (direction)
        {
            case CellDirection.DOWN:
                times = 1;
                break;
            case CellDirection.LEFT:
                times = 2;
                break;
            case CellDirection.UP:
                times = 2;
                break;
        }

        for (int i = 0; i < times; i++)
            RotateEdgesClockwise();
    }

    private void RotateEdgesClockwise()
    {
        for (int i = 0; i < edgeList.Count; i++)
        {
            //Aplicar una rotación de -90° simple, T(x,y) = <y, -x>
            Vector2Int rotatedEdge = new Vector2Int(edgeList[i].y, -edgeList[i].x);
            edgeList[i] = rotatedEdge;
        }
    }

    private IEnumerator _RotateClockwise()
    {
        isRotating = true;
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, 0f, -90f);

        float timeStep = 0.1f;
        for (float t = 0; t <= 1f; t += timeStep)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        RotateEdgesClockwise();
        isRotating = false;
    }

    public List<Vector2Int> GetEdgeGridPositionList()
    {
        List<Vector2Int> edgeGridPositionList = new List<Vector2Int>();
        for (int i = 0; i < edgeList.Count; i++)
        {
            Vector2Int edgeGridPosition = gridPosition + edgeList[i];
            if(edgeGridPosition.x >= 0 && edgeGridPosition.x < gridSize.x && edgeGridPosition.y >= 0 && edgeGridPosition.y < gridSize.y)
                edgeGridPositionList.Add(edgeGridPosition);
        }

        return edgeGridPositionList;
    }

    public bool CheckConnection(CellController otherCell)
    {
        return GetEdgeGridPositionList().Intersect(otherCell.GetEdgeGridPositionList()).First() != null;
    }

    public void OnButtonClicked()
    {
        if (!isRotating)
            StartCoroutine(_RotateClockwise());
    }
}
