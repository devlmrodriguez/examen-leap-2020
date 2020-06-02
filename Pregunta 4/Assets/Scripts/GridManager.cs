using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private List<TubeController> tubeList;
    [SerializeField]
    private Button completedButton;

    private Dictionary<TubeController, TubeGroup> tubeGroupMemory;  //Guarda los tubos que estan dentro de grupos específicos
    private List<TubeGroup> tubeGroupList;  //Guardo los grupos de tubos

    private void Awake()
    {
        tubeGroupMemory = new Dictionary<TubeController, TubeGroup>();
        tubeGroupList = new List<TubeGroup>();
    }

    private void Start()
    {
        GenerateTubeGroups();
        CheckSolution();
    }

    private void GenerateTubeGroups()
    {
        //Limpiar los grupos ya existentes
        tubeGroupMemory.Clear();
        tubeGroupList.Clear();

        for (int i = 0; i < tubeList.Count; i++)
        {
            Vector2Int gridPosition = GetGridPositionFromIndex(i);
            TubeController tube = tubeList[i];

            //Obtener los tubos conectados a este
            List<TubeController> connectedTubes = GetConnectedTubes(gridPosition, tube);
            connectedTubes.Add(tube);

            //Obtener el grupo al que pertenece si es que existe
            TubeGroup tubeGroup = null;
            if (tubeGroupMemory.ContainsKey(tube))
                tubeGroup = tubeGroupMemory[tube];

            //Si el tubo no tiene un grupo asignado
            if(tubeGroup == null)
            {
                //Chequear sus tubos conexos, si estos ya están en un grupo, asignarle este
                for (int j = 0; j < connectedTubes.Count; j++)
                {
                    TubeController connectedTube = connectedTubes[j];
                    if (tubeGroupMemory.ContainsKey(connectedTube))
                    {
                        tubeGroup = tubeGroupMemory[connectedTube];
                        break;
                    }
                }

                //Si ningún tubo conexo está en algún grupo, crear uno nuevo
                if (tubeGroup == null)
                {
                    tubeGroup = new TubeGroup();
                    tubeGroupList.Add(tubeGroup);
                }
            }

            //Verificar todos los tubos conexos y agregarlos a las memorias
            for (int j = 0; j < connectedTubes.Count; j++)
            {
                TubeController connectedTube = connectedTubes[j];

                if (!tubeGroupMemory.ContainsKey(connectedTube))
                {
                    tubeGroupMemory.Add(connectedTube, tubeGroup);
                    tubeGroup.GetTubeSet().Add(connectedTube);
                }
            }
        }
    }

    private List<TubeController> GetConnectedTubes(Vector2Int gridPosition, TubeController tube)
    {
        List<TubeController> connectedTubes = new List<TubeController>();

        //Obtener los bordes en coordenadas del grid (tubos posiblemente conexos)
        List<Vector2Int> gridEdges = GetTubeGridEdges(gridPosition, tube);
        for (int i = 0; i < gridEdges.Count; i++)
        {
            Vector2Int potentialPosition = new Vector2Int(gridEdges[i].x, gridEdges[i].y);
            TubeController potentialTube = GetTubeInGrid(potentialPosition.x, potentialPosition.y);
            List<Vector2Int> potentialTubeGridEdges = GetTubeGridEdges(potentialPosition, potentialTube);

            //Este tubo potencial solo será un tubo conexo si el tubo con el que estamos comparándolo y él comparten coordenadas
            //de grid y están dentro de este, es decir, si ambos apuntan al otro
            if (potentialTubeGridEdges.Contains(gridPosition))
                connectedTubes.Add(potentialTube);
        }

        return connectedTubes;
    }

    private List<Vector2Int> GetTubeGridEdges(Vector2Int gridPosition, TubeController tube)
    {
        List<Vector2Int> gridEdges = new List<Vector2Int>();

        //Obtener los bordes locales del tubo
        List<Vector2Int> localEdges = tube.GetLocalEdges();
        for (int i = 0; i < localEdges.Count; i++)
        {
            //Convertir los bordes locales a bordes de grid (coordenadas del arreglo)
            //Y solo agregarlos si estas coordenadas estan dentro del grid
            Vector2Int gridEdge = new Vector2Int(gridPosition.x + localEdges[i].x, gridPosition.y + localEdges[i].y);
            if (gridEdge.x >= 0 && gridEdge.x < gridSize.x && gridEdge.y >= 0 && gridEdge.y < gridSize.y)
                gridEdges.Add(gridEdge);
        }

        return gridEdges;
    }

    private void CheckSolution()
    {
        GenerateTubeGroups();

        //Verificar que todos los grupos tengan tubos de inicio y fin
        bool isSolved = true;
        for (int i = 0; i < tubeGroupList.Count; i++)
            if (!tubeGroupList[i].GetHasStartAndEnd())
            {
                isSolved = false;
                break;
            }

        if (isSolved)
        {
            completedButton.gameObject.SetActive(true);
            Debug.Log("Puzzle completado");
        }
    }

    private Vector2Int GetGridPositionFromIndex(int index)
    {
        return new Vector2Int(index % gridSize.x, index / gridSize.x);
    }

    private TubeController GetTubeInGrid(int i, int j)
    {
        return tubeList[j * gridSize.x + i];
    }

    public void OnTubeRotated()
    {
        GenerateTubeGroups();
        CheckSolution();
    }

    public void OnCompletedButtonClicked()
    {
        completedButton.gameObject.SetActive(false);
    }
}
