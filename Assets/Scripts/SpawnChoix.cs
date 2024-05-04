using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChoix : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject hommeCharacterPrefab;
    [SerializeField] private GameObject femmeCharacterPrefab;

    void Start()
    {
        if (ParametresParties.Instance.caraIndex == 0)
        {
            hommeCharacterPrefab.SetActive(true);
            femmeCharacterPrefab.SetActive(false);
        }
        else if (ParametresParties.Instance.caraIndex == 1)
        {
            hommeCharacterPrefab.SetActive(false);
            femmeCharacterPrefab.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
