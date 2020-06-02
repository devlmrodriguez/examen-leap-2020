using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeGroup
{
    private HashSet<TubeController> tubeSet;

    public TubeGroup()
    {
        tubeSet = new HashSet<TubeController>();
    }

    public HashSet<TubeController> GetTubeSet()
    {
        return tubeSet;
    }

    public bool GetHasStartAndEnd()
    {
        //Solo tendrá un inicio y fin si los tubos contenido en el set son más de 2 start-end
        int count = 0;
        foreach (TubeController tube in tubeSet)
            if (tube.GetShape() == TubeShape.START_END)
                count++;

        return count >= 2;
    }
}
