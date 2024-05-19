using UnityEngine;
using System.IO;
using LitJson;
using System.Linq;
using UnityEngine.SceneManagement;

//chapitre 8 ex1
public class GestionnaireSauvegarde : MonoBehaviour
{
    private const string NOM_FICHIER = "sauvegarde.json";
    private const string OBJECTS_KEY = "objects";
    private const string SAVEID_KEY = "$saveID";

    private string _cheminFichier;    // Ne supporte qu'un seul fichier et il porte toujours le m�me nom.
    private JsonData objects = null;  // Les objets � charger une fois la sc�ne est charg�e

    // Dit si le fichier de sauvegarde existe
    public bool FichierExiste
    {
        get => !string.IsNullOrEmpty(_cheminFichier) && File.Exists(_cheminFichier);
    }

    // Start is called before the first frame update
    void Awake()
    {
        _cheminFichier = Path.Combine(Application.persistentDataPath, "sauvegarde.json");
        Debug.Log(Application.persistentDataPath);
    }

    public void SauvegarderPartie()
    {
        Debug.Log("Sauvegarde");
        JsonData result = new JsonData();

        var allSaveables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        Debug.Log(allSaveables.Count());

        JsonData savedObjects = new JsonData();
        foreach (var saveable in allSaveables)
        {
            JsonData data = saveable.SavedData;
            data[SAVEID_KEY] = saveable.SaveID;
            savedObjects.Add(data);
        }
        result[OBJECTS_KEY] = savedObjects;

        // On �crit le fichier avec une indentation pour le rendre lisible
        var writer = new JsonWriter();
        writer.PrettyPrint = true;
        result.ToJson(writer);
        System.IO.File.WriteAllText(_cheminFichier, writer.ToString());
    }

    public void ChargerPartie(string nomScene)
    {
        if (!FichierExiste)
        {
            return;
        }

        string text = File.ReadAllText(_cheminFichier);

        objects = JsonMapper.ToObject(text)[OBJECTS_KEY];
        if (objects != null)
        {

            SceneManager.sceneLoaded += LoadAfter;  // La m�thode sera appel�e apr�s le chargement de la sc�ne
            SceneManager.LoadScene(nomScene, LoadSceneMode.Single);

        }
    }

    private void LoadAfter(Scene s, LoadSceneMode mode)
    {
        var allLoadables = Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToDictionary(o => o.SaveID, o => o);

        int nombreObjets = objects.Count;
        for (int i = 0; i < nombreObjets; i++)
        {
            JsonData data = objects[i];
            string saveID = data[SAVEID_KEY].ToString();

            if (allLoadables.ContainsKey(saveID))
            {
                allLoadables[saveID].LoadFromData(data);
            }
        }
        SceneManager.sceneLoaded -= LoadAfter;
    }

    public void Supprimer()
    {
        if (FichierExiste)
        {
            File.Delete(_cheminFichier);
            Debug.Log("suppresion a cause de mort");
        }
    }
}
