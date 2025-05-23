using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestionnaireInterface : MonoBehaviour
{
    [SerializeField] private Button _boutonDemarrer;

    enum Difficulte
    {
        Facile,
        Moyen,
        Difficile
    }

    private Difficulte difficulte;
    private ForetGenerator foretGenerator;

    [SerializeField] private TMP_InputField nomJoueur;
    [SerializeField] private TMP_Text presentation;

    [SerializeField] private int[] valeursFacile;
    [SerializeField] private int[] valeursMoyen;
    [SerializeField] private int[] valeursDifficile;

    [SerializeField] private TMP_Text[] valeursDepart;
    [SerializeField] private TMP_Dropdown difficulteDropdown;
    [SerializeField] private TMP_Dropdown caraDropdown;
    [SerializeField] private TMP_Dropdown dropdownForet;
    [SerializeField] private Camera cameraHomme;
    [SerializeField] private Camera cameraFemme;
    [SerializeField] private RawImage maleCharacterImage;
    [SerializeField] private RawImage femaleCharacterImage;

    private int caraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        nomJoueur.text = "Mathurin";
        ChangerNomJoueur();

        difficulte = Difficulte.Facile;
        MettreAJour(valeursFacile);

        cameraFemme.gameObject.SetActive(false);
        femaleCharacterImage.gameObject.SetActive(false);

        foretGenerator = FindObjectOfType<ForetGenerator>();
        ParametresParties._strategieForet = new ForetGrille();
        dropdownForet.onValueChanged.AddListener(delegate
        {
            ChangerForet(dropdownForet.value);
        });
    }

    void Update()
    {
        _boutonDemarrer.interactable = nomJoueur.text != string.Empty;
    }

    public void ChangerDifficulte()
    {
        difficulte = (Difficulte)difficulteDropdown.value;

        switch (difficulte)
        {
            case Difficulte.Facile:
                MettreAJour(valeursFacile);
                break;
            case Difficulte.Moyen:
                MettreAJour(valeursMoyen);
                break;
            case Difficulte.Difficile:
                MettreAJour(valeursDifficile);
                break;
        }
    }

    public void DemarrerPartie()
    {
        PlayerPrefs.SetString("NomJoueur", nomJoueur.text);
        PlayerPrefs.SetInt("Difficulte", (int)difficulte);
        PlayerPrefs.SetInt("CaraIndex", caraIndex);
        PlayerPrefs.SetInt("ForetIndex", dropdownForet.value);

        int[] valeursActuelles = null;
        switch (difficulte)
        {
            case Difficulte.Facile:
                valeursActuelles = valeursFacile;
                break;
            case Difficulte.Moyen:
                valeursActuelles = valeursMoyen;
                break;
            case Difficulte.Difficile:
                valeursActuelles = valeursDifficile;
                break;
        }

        ParametresParties.Instance.NomJoueur = nomJoueur.text;
        ParametresParties.Instance.OrDepart = valeursActuelles[0];
        ParametresParties.Instance.OeufsDepart = valeursActuelles[1];
        ParametresParties.Instance.SemencesDepart = valeursActuelles[2];
        ParametresParties.Instance.TempsCroissance = valeursActuelles[3];
        ParametresParties.Instance.DelaiCueillete = valeursActuelles[4];
        ParametresParties.Instance.caraIndex = caraIndex;
        //Debug.Log(ParametresParties.Instance.caraIndex);    

        if (nomJoueur.text != string.Empty)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ferme");
        }
    }

    public void QuitterJeu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void MettreAJour(int[] valeurs)
    {
        for (int i = 0; i < valeursDepart.Length; i++)
        {
            valeursDepart[i].text = valeurs[i].ToString();
        }
    }

    public void ChangerNomJoueur()
    {
        presentation.text = $"\u266A \u266B Dans la ferme \u00e0  {nomJoueur.text} \u266B \u266A";
    }

    public void ChangerCara()
    {
        caraIndex = caraDropdown.value;
        switch (caraIndex)
        {
            case 0:
                cameraHomme.gameObject.SetActive(true);
                cameraFemme.gameObject.SetActive(false);
                maleCharacterImage.gameObject.SetActive(true);
                femaleCharacterImage.gameObject.SetActive(false);
                break;
            case 1:
                cameraHomme.gameObject.SetActive(false);
                cameraFemme.gameObject.SetActive(true);
                maleCharacterImage.gameObject.SetActive(false);
                femaleCharacterImage.gameObject.SetActive(true);
                break;
        }
    }
    public void ChangerForet(int choixForet)
    {
        switch (choixForet)
        {
            case 0:
                ParametresParties._strategieForet = new ForetGrille();
                break;
            case 1:
                ParametresParties._strategieForet = new ForetRandom();
                break;
            case 2:
                ParametresParties._strategieForet = new ForetSimulation();
                break;
            default:
                break;
        }
        foretGenerator.SetStrategy(ParametresParties._strategieForet);
    }

    public void ContinuerPartie()
    {
        string nomJoueur = PlayerPrefs.GetString("NomJoueur", "Mathurin");
        int difficulte = PlayerPrefs.GetInt("Difficulte", 0);
        int caraIndex = PlayerPrefs.GetInt("CaraIndex", 0);
        int foretIndex = PlayerPrefs.GetInt("ForetIndex", 0);

        ParametresParties.Instance.NomJoueur = nomJoueur;
        ParametresParties.Instance.caraIndex = caraIndex;

        int[] valeursActuelles = null;
        switch ((GestionnaireInterface.Difficulte)difficulte)
        {
            case GestionnaireInterface.Difficulte.Facile:
                valeursActuelles = valeursFacile;
                break;
            case GestionnaireInterface.Difficulte.Moyen:
                valeursActuelles = valeursMoyen;
                break;
            case GestionnaireInterface.Difficulte.Difficile:
                valeursActuelles = valeursDifficile;
                break;
        }

        if (valeursActuelles != null)
        {
            ParametresParties.Instance.OrDepart = valeursActuelles[0];
            ParametresParties.Instance.OeufsDepart = valeursActuelles[1];
            ParametresParties.Instance.SemencesDepart = valeursActuelles[2];
            ParametresParties.Instance.TempsCroissance = valeursActuelles[3];
            ParametresParties.Instance.DelaiCueillete = valeursActuelles[4];
        }
        ChangerForet(foretIndex);
        
        GetComponent<GestionnaireSauvegarde>().ChargerPartie("Ferme");
        
    }
}
