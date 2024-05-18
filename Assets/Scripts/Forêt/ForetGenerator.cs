using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForetGenerator : MonoBehaviour
{
    private StrategieForet strategie;
    public void SetStrategy(StrategieForet newStrategie)
    {
        strategie = newStrategie;
    }

    public void GenerateForet(GameObject arbrePrefab)
    {
        strategie.GenerateForet(arbrePrefab);
    }
}

