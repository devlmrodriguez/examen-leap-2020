using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroup : MonoBehaviour
{
    private HashSet<CellController> cellSet;

    private void Awake()
    {
        cellSet = new HashSet<CellController>();
    }

    public void AddCell(CellController cell)
    {
        cellSet.Add(cell);
    }

    public void RemoveCell(CellController cell)
    {
        cellSet.Remove(cell);
    }

    public bool ContainsCell(CellController cell)
    {
        return cellSet.Contains(cell);
    }
}
