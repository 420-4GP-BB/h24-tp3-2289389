using UnityEngine;
using LitJson;

public class SaveJoueur : MonoBehaviour, ISaveable, ISerializationCallbackReceiver
{
    [HideInInspector]
    [SerializeField] private string _saveID;
    public string SaveID
    {
        set => _saveID = value;
        get => _saveID;
    }

    private Inventaire _inventaire;
    private EnergieJoueur _energieJoueur;
    private DonneesJoueur _donneesJoueur;
    private GameManager _gamemanager;
    private Soleil _soleil;
    //private Buche[] _buche;

    private const string NAME_KEY = "name";
    private const string POSITION_X_KEY = "positionX";
    private const string POSITION_Y_KEY = "positionY";
    private const string POSITION_Z_KEY = "positionZ";

    private const string ROTATION_X_KEY = "rotationX";
    private const string ROTATION_Y_KEY = "rotationY";
    private const string ROTATION_Z_KEY = "rotationZ";
    private const string ROTATION_W_KEY = "rotationW";

    private const string ENERGY_KEY = "energy";
    private const string INVENTORY_KEY = "inventory";
    private const string DAY_KEY = "day";
    private const string TIME_KEY = "time";

    private void Awake()
    {
        _inventaire = GetComponent<Inventaire>();
        _energieJoueur = GetComponent<EnergieJoueur>();
        _donneesJoueur = GetComponent<DonneesJoueur>();
        _gamemanager = FindObjectOfType<GameManager>();
        _soleil = FindObjectOfType<Soleil>();
        //_buche = FindObjectsOfType<Buche>();
    }

    public JsonData SavedData => BuildData();

    public void LoadFromData(JsonData data)
    {
        _donneesJoueur.Nom = data[NAME_KEY].ToString();
        transform.position = new Vector3(
            float.Parse(data[POSITION_X_KEY].ToString()),
            float.Parse(data[POSITION_Y_KEY].ToString()),
            float.Parse(data[POSITION_Z_KEY].ToString()));
        transform.rotation = new Quaternion(
            float.Parse(data[ROTATION_X_KEY].ToString()),
            float.Parse(data[ROTATION_Y_KEY].ToString()),
            float.Parse(data[ROTATION_Z_KEY].ToString()),
            float.Parse(data[ROTATION_W_KEY].ToString()));
        _inventaire.Or = int.Parse(data[INVENTORY_KEY]["Or"].ToString());
        _inventaire.Oeuf = int.Parse(data[INVENTORY_KEY]["Oeuf"].ToString());
        _inventaire.Choux = int.Parse(data[INVENTORY_KEY]["Choux"].ToString());
        _inventaire.Graines = int.Parse(data[INVENTORY_KEY]["Graines"].ToString());
        _inventaire.Bois = int.Parse(data[INVENTORY_KEY]["Bois"].ToString());
        _gamemanager.NumeroJour = int.Parse(data[DAY_KEY].ToString());
        _soleil.SetTimeOfDay(float.Parse(data[TIME_KEY].ToString()));
        _energieJoueur.Energie = float.Parse(data[ENERGY_KEY].ToString());
    
    }

    private JsonData BuildData()
    {
        var result = new JsonData();
        result[NAME_KEY] = _donneesJoueur.Nom;
        result[POSITION_X_KEY] = transform.position.x.ToString();
        result[POSITION_Y_KEY] = transform.position.y.ToString();
        result[POSITION_Z_KEY] = transform.position.z.ToString();
        result[ROTATION_X_KEY] = transform.rotation.x.ToString();
        result[ROTATION_Y_KEY] = transform.rotation.y.ToString();
        result[ROTATION_Z_KEY] = transform.rotation.z.ToString();
        result[ROTATION_W_KEY] = transform.rotation.w.ToString();
        result[ENERGY_KEY] = _energieJoueur.Energie.ToString();
        var inventory = new JsonData();
        inventory["Or"] = _inventaire.Or.ToString();
        inventory["Oeuf"] = _inventaire.Oeuf.ToString();
        inventory["Choux"] = _inventaire.Choux.ToString();
        inventory["Graines"] = _inventaire.Graines.ToString();
        inventory["Bois"] = _inventaire.Bois.ToString();
        result[DAY_KEY] = _gamemanager.NumeroJour.ToString();
        result[TIME_KEY] = _soleil.CurrentTimeOfDay.ToString();
        result[INVENTORY_KEY] = inventory;
        return result;
    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(_saveID))
        {
            _saveID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnAfterDeserialize()
    {
    }
}