using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenardSpawner : MonoBehaviour
{
    private Soleil _soleil;
    private GameObject renard;
    [SerializeField] private GameObject renardPrefab; 

    void Start()
    {
        _soleil = GameObject.FindObjectOfType<Soleil>();
    }

    void Update()
    {
        if ((_soleil.CurrentTimeOfDay >= 21.0f || _soleil.CurrentTimeOfDay < 8.0f) && renard == null)
        {
            InstantiateRenard();
        }
        else if (_soleil.CurrentTimeOfDay >= 8.0f && _soleil.CurrentTimeOfDay < 21.0f && renard != null)
        {
            DestroyRenard();
        }
    }

    private void InstantiateRenard()
    {
        renard = Instantiate(renardPrefab, transform.position, Quaternion.identity);
    }

    private void DestroyRenard()
    {
        Destroy(renard);
        renard = null;
    }
}