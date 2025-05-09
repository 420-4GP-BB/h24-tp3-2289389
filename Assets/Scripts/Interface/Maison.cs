using UnityEngine;
using UnityEngine.UI;

public class Maison : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _panelMaison;
    [SerializeField] private GameManager _gameManager;
    
    [SerializeField] private Button _boutonManger;
    
    private ComportementJoueur _joueur;

    void Start()
    {
        _panelMaison.SetActive(false);
        if(ParametresParties.Instance.caraIndex == 0){
            _joueur = UnityEngine.GameObject.Find("Joueur").GetComponent<ComportementJoueur>();
        }
        else if(ParametresParties.Instance.caraIndex == 1){
            _joueur = UnityEngine.GameObject.Find("Joueuse").GetComponent<ComportementJoueur>();
        }
    }

    private void OnGUI()
    {
        _boutonManger.interactable = _joueur.PeutManger;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _joueur.gameObject)
        {
            _panelMaison.SetActive(true);
            _joueur.ChangerEtat(new EtatDansMenu(_joueur));
        }
    }
    
    public void Dodo()
    {
        _joueur.ChangerEtat(new EtatDormir(_joueur, _panelMaison));
    }

    void FermerMenu()
    {
        _panelMaison.SetActive(false);
        _joueur.ChangerEtat(_joueur.EtatNormal);
        _joueur.ReplacerPositionDepart();
    }

    public void Manger()
    {
//        FermerMenu();
        _joueur.Manger();
    }

    public void SortirMaison()
    {
        FermerMenu();
    }
}