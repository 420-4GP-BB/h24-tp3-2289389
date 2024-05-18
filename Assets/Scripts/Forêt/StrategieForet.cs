using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StrategieForet 
{
    public abstract void GenerateForet(GameObject arbrePrefab);
}

public class ForetGrille : StrategieForet
{
    private int tailleterrain = 130;
    private float espace = 5f;
    private GameObject[] zonesExclues;

    public override void GenerateForet(GameObject arbrePrefab)
    {
        Debug.Log("Forêt en grille");
        zonesExclues = GameObject.FindGameObjectsWithTag("ZoneExclue");
        int rangees = Mathf.FloorToInt(tailleterrain / espace);
        int arbresParRangee = Mathf.FloorToInt(tailleterrain / espace);

        Vector3 origine = new Vector3(-63f, 0f, -63f);
        Object.Instantiate(arbrePrefab, origine, Quaternion.identity);

        for (int i = 0; i < rangees; i++)
        {
            for (int j = 0; j < arbresParRangee; j++)
            {
                float x = j * espace + origine.x;
                float z = i * espace + origine.z;
                Vector3 position = new Vector3(x, 0, z);

                bool positionValide = true;
                foreach (GameObject zone in zonesExclues)
                {
                    if (zone.GetComponent<Collider>().bounds.Contains(position))
                    {
                        positionValide = false;
                        break;
                    }
                }

                if (positionValide)
                {
                    Object.Instantiate(arbrePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}

public class ForetRandom : StrategieForet
{
    private float espace = 4f;
    private GameObject[] zonesExclues;
    private Vector3[] positionArbres;

    public override void GenerateForet(GameObject arbrePrefab)
    {
        Debug.Log("Forêt aléatoire");
        zonesExclues = GameObject.FindGameObjectsWithTag("ZoneExclue");
        int nombreArbres = Random.Range(300, 500);
        positionArbres = new Vector3[nombreArbres];
        int positionValide = 0;

        while (positionValide < nombreArbres)
        {
            Vector3 position = GenererPosition();

            if (isValide(position))
            {
                positionArbres[positionValide] = position;
                positionValide++;
            }
        }
        for (int i = 0; i < positionValide; i++)
        {
            Object.Instantiate(arbrePrefab, positionArbres[i], Quaternion.identity);
        }
    }

    private Vector3 GenererPosition()
    {
        float x = Random.Range(-63.5f, 63.5f);
        float z = Random.Range(-63.5f, 63.5f);
        return new Vector3(x, 0f, z);
    }

    private bool isValide(Vector3 position)
    {
        foreach (GameObject zone in zonesExclues)
        {
            if (zone.GetComponent<Collider>().bounds.Contains(position))
            {
                return false;
            }
        }

        for (int i = 0; i < positionArbres.Length; i++)
        {
            if (positionArbres[i] != Vector3.zero && Vector3.Distance(position, positionArbres[i]) < espace)
            {
                return false;
            }
        }
        return true;
    }
}

public class ForetSimulation : StrategieForet
{
    public override void GenerateForet(GameObject arbrePrefab)
    {
        Debug.Log("Foret simulée");
    }
}
