using UnityEngine;

public class Oeuf : MonoBehaviour, IRamassable
{
    private float _tempsCroissance;
    private Soleil _soleil;
    private int journeesDeVie = 0;
    [SerializeField] private GameObject poulet;

    private void Start()
    {
        _soleil = FindObjectOfType<Soleil>();
    }

    public void Ramasser(Inventaire inventaireJoueur)
    {
        inventaireJoueur.Oeuf++;
        Destroy(gameObject);
    }

    public EtatJoueur EtatAUtiliser(ComportementJoueur Sujet)
    {
        return new EtatRamasserObjet(Sujet, this);
    }

    public bool Permis(ComportementJoueur sujet)
    {
        return true;
    }

    void Update()
    {
        _tempsCroissance += _soleil.DeltaMinutesEcoulees;
        if (_tempsCroissance >= ConstantesJeu.MINUTES_PAR_JOUR)
        {
            _tempsCroissance = 0.0f;
            JourneePassee();
        }
    }

    public void JourneePassee()
    {
        journeesDeVie++;
        //Debug.Log(journeesDeVie);
        if (journeesDeVie >= 3)
        {
            journeesDeVie = 0;
            float rand = Random.Range(0.0f, 1.0f);
            //Debug.Log(rand);
            if (rand < 0.25f)
            {
                Instantiate(poulet, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else if (rand >= 0.25f)
            {
                Destroy(gameObject);
            }
        }
    }
}