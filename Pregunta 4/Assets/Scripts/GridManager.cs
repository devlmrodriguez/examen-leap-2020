using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private List<CellController> cellList;

    private List<CellGroup> cellGroupList;

    private void Awake()
    {
        
    }

    private void GenerateCellGroups()
    {
        for(int i = 0; i < cellList.Count; i++)
        {

        }
    }

    private void CheckCellConnection(Vector2Int cellPosition, CellController cell)
    {
        //List<Vector2Int> cellGridPositionEdge = 
    }

    private void GetGridSize()
    {

    }

    public CellController GetCell(Vector2Int gridPosition)
    {
        return cellList[gridPosition.y * gridSize.x + gridPosition.x];
    }

    public void OnCellRotated(CellController cell)
    {

    }


}
