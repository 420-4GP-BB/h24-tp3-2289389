using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AffichageRessource : MonoBehaviour
{
    [SerializeField] private TMP_Text _nomJoueur;
    [SerializeField] private TMP_Text _energieJoueurTexte;
    private Inventaire ressourcesJoueurs;
    private EnergieJoueur energieJoueur;

    [SerializeField] private TMP_Text textOr;
    [SerializeField] private TMP_Text textViande;
    [SerializeField] private TMP_Text textChoux;
    [SerializeField] private TMP_Text textGraines;
    [SerializeField] private TMP_Text textBois;

    private Color couleurParDefautEnergie;
    
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject joueur = null; 
        if(ParametresParties.Instance.caraIndex == 0){
            joueur = UnityEngine.GameObject.Find("Joueur");
        }
        else if(ParametresParties.Instance.caraIndex == 1){
            joueur = UnityEngine.GameObject.Find("Joueuse");
        }
        ressourcesJoueurs = joueur.GetComponent<Inventaire>();
        energieJoueur = joueur.GetComponent<EnergieJoueur>();
        _nomJoueur.text = joueur.GetComponent<DonneesJoueur>().Nom;

        couleurParDefautEnergie = _energieJoueurTexte.color;
    }

    void OnGUI()
    {
        textOr.text = ressourcesJoueurs.Or.ToString();
        textViande.text = ressourcesJoueurs.Oeuf.ToString();
        textChoux.text = ressourcesJoueurs.Choux.ToString();
        textGraines.text = ressourcesJoueurs.Graines.ToString();
        textBois.text = ressourcesJoueurs.Bois.ToString();
        _energieJoueurTexte.text = Mathf.RoundToInt(energieJoueur.Energie * 100) + "%";

        if (energieJoueur.EnergieFaible)
            _energieJoueurTexte.color = Color.red;
        else
            _energieJoueurTexte.color = couleurParDefautEnergie;
    }
}