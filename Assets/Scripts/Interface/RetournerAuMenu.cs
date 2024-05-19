using UnityEngine;

public class RetournerAuMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.FindObjectOfType<GestionnaireSauvegarde>().SauvegarderPartie();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
