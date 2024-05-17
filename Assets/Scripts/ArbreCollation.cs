using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbreCollation : MonoBehaviour
{
    [SerializeField] public GameObject[] prefabsCollations;
    private GameObject collationActuelle;
    private float prochaineCollation = 0f;
    private EnergieJoueur _energieJoueur;
    [SerializeField] private Transform positionCollation;

    void Start()
    {
        _energieJoueur = FindObjectOfType<EnergieJoueur>();
        StartCoroutine(FaireTomber());
    }

    private IEnumerator FaireTomber()
    {
        while (true)
        {
            if (collationActuelle == null)
            {
                int index = Random.Range(0, prefabsCollations.Length);
                collationActuelle = Instantiate(prefabsCollations[index], positionCollation.position, Quaternion.identity);

                Rigidbody rigidbodyCollation = collationActuelle.AddComponent<Rigidbody>();
                rigidbodyCollation.useGravity = true;
                rigidbodyCollation.isKinematic = false; 
                prochaineCollation = Time.time + 30f;
            }

            yield return new WaitUntil(() => Time.time >= prochaineCollation);
        }
    }
}