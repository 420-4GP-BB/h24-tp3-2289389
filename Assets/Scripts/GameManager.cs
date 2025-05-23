using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Soleil _soleil;
    [SerializeField] private GameObject arbrePrefab;

    private ComportementJoueur _joueur;

    private GestionnaireInterface _gestionnaireInterface;

    private const float DISTANCE_ACTION = 3.0f;

    private Inventaire _inventaireJoueur;
    private EnergieJoueur _energieJoueur;
    private ChouMesh3D[] _chous;
    public int NumeroJour = 1;


    void Start()
    {
        ParametresParties._strategieForet.GenerateForet(arbrePrefab);

        if (ParametresParties.Instance.caraIndex == 0)
            _joueur = GameObject.Find("Joueur").GetComponent<ComportementJoueur>();
        else if (ParametresParties.Instance.caraIndex == 1)
            _joueur = GameObject.Find("Joueuse").GetComponent<ComportementJoueur>();
        _inventaireJoueur = _joueur.GetComponent<Inventaire>();
        _energieJoueur = _joueur.GetComponent<EnergieJoueur>();
        _chous = FindObjectsByType<ChouMesh3D>(FindObjectsSortMode.None);

        // Patron de conception: Observateur
        FindObjectOfType<Soleil>().OnJourneeTerminee += NouvelleJournee;

        _energieJoueur.OnEnergieVide += EnergieVide;
    }

    void NouvelleJournee()
    {
        NumeroJour++;

        GameObject[] poules = GameObject.FindGameObjectsWithTag("Poule");
        foreach (var poule in poules)
        {
            poule.GetComponent<PondreOeufs>().DeterminerPonte();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuConfiguration");
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 45;
        }
        else
        {
            Time.timeScale = 1;
        }

        // L'?tat du joueur peut affecter le passage du temps (ex.: Dodo: tout va vite, menus: le temps est stopp?, etc)
        Time.timeScale *= _joueur.GetComponent<ComportementJoueur>().MultiplicateurScale;
    }

    /// <summary>
    /// Partie perdue quand l'?nergie tombe ? z?ro
    /// </summary>
    private void EnergieVide()
    {
        _joueur.ChangerEtat(new EtatDansMenu(_joueur));
        GetComponent<GestionnaireSauvegarde>().Supprimer();
        GestionnaireMessages.Instance.AfficherMessageFin(
            "Plus d'?nergie!",
            "Vous n'avez pas r?ussi ? vous garder en vie, vous tombez sans connaissance au milieu du champ." +
            "Un loup passe et vous d?guste en guise de d?ner. Meilleure chance la prochaine partie!");
        Time.timeScale = 0;
    }
}