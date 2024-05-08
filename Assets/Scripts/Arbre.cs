using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbre : MonoBehaviour, IPoussable
{
    [SerializeField] private GameObject buche;
    [SerializeField] private Transform positionBuche;

    public void Pousser(Inventaire inventaireJoueur)
    {
        Destroy(gameObject);
    }

    public EtatJoueur EtatAUtiliser(ComportementJoueur Sujet)
    {
        return new EtatArbre(Sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
    public void AfficherBuche()
    {
        Instantiate(buche, positionBuche.position, Quaternion.identity);
    }
}
