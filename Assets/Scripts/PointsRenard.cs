using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRenard : MonoBehaviour
{
    [SerializeField] private GameObject[] zones;
    [SerializeField] private GameObject pointPrefab;
    private int pointsZone = 2;
    private float rayon = 0.1f; 
    private bool isTermine = false;
    private Vector3[] pointPosition; 
    private int indexPoint = 0; 

    void Start()
    {
        pointPosition = new Vector3[pointsZone * zones.Length];
        StartCoroutine(GenererPoint());
    }

    void Update()
    {
        if (isTermine)
        {
            StopCoroutine(GenererPoint());
        }
    }

    IEnumerator GenererPoint()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject zone in zones)
        {
            GenererPoints(zone, pointsZone);
        }
        isTermine = true;
    }

    void GenererPoints(GameObject zone, int pointsAGenerer)
    {
        BoxCollider boxCollider = zone.GetComponent<BoxCollider>();
        for (int i = 0; i < pointsAGenerer; i++)
        {
            int tentatives = 0;
            int maxTentatives = 10; 

            while (tentatives < maxTentatives)
            {
                tentatives++;
                Vector3 point = new Vector3(
                    Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                    0,
                    Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z)
                );

                if (!isOccupe(point, rayon) && !IsProche(point))
                {
                    GameObject pointObject = Instantiate(pointPrefab, point, Quaternion.identity);
                    pointObject.tag = "PointRenard";
                    pointPosition[indexPoint] = point;
                    indexPoint++;
                    break; 
                }
            }
        }
    }

    bool isOccupe(Vector3 point, float rayon)
    {
        Collider[] colliders = Physics.OverlapSphere(point, rayon);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Arbre"))
            {
                return true;
            }
        }
        return false;
    }

    bool IsProche(Vector3 point)
    {
        for (int i = 0; i < indexPoint; i++)
        {
            if (Vector3.Distance(point, pointPosition[i]) < rayon)
            {
                return true;
            }
        }
        return false;
    }
}
