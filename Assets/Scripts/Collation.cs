using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Collation : MonoBehaviour, IRamassable
{
    private EnergieJoueur _energieJoueur;

    private void Start()
    {
        if (ParametresParties.Instance.caraIndex == 1)
            _energieJoueur = GameObject.Find("Joueuse").GetComponent<EnergieJoueur>();
        else
            _energieJoueur = GameObject.Find("Joueur").GetComponent<EnergieJoueur>();
    }

    public async void Ramasser(Inventaire inventaireJoueur)
    {
        Destroy(gameObject);
        await Task.Delay(1200); 
        _energieJoueur.Energie += 0.15f;
    }

    public EtatJoueur EtatAUtiliser(ComportementJoueur sujet)
    {
        return new EtatRamasserObjet(sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }
}	