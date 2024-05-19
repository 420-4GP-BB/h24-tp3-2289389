using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonneesJoueur : MonoBehaviour
{
    private string _nom;

    public string Nom
    {
        get { return _nom; }
        set { _nom = value; }
    }

    void Awake()
    {
        Nom = ParametresParties.Instance.NomJoueur;
    }
}
