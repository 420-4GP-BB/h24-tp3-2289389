using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestChoux
{
    private GameObject chou, soleil;
    private Inventaire inventaire;

    [SetUp]
    public void CreerObjets()
    {
        soleil = new GameObject("Directional Light");
        soleil.AddComponent<Light>();
        soleil.AddComponent<Soleil>();
        chou = GameObject.Instantiate(PrefabUtility.LoadPrefabContents("Assets/Prefabs/Chou.prefab"));

        var joueur = new GameObject("Joueur");
        inventaire = joueur.AddComponent<Inventaire>();
    }

    [TearDown]
    public void DetruireObjets()
    {
        GameObject.Destroy(soleil);
        GameObject.Destroy(chou);
        GameObject.Destroy(inventaire.gameObject);
    }

    [UnityTest]
    public IEnumerator TestChouCueillir()
    {
        // ====== EXEMPLE DE TEST DÃ‰JÃ€ FONCTIONNEL ======
        // Valide ce qui se passe quand on plante un chou, qu'on attend 3 jours, puis qu'on le cueille.
        // On vÃ©rifie que le nombre de choux


        // ARRANGE: dans le SetUp + ici
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 1;
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;

        var chouCroissant = chou.GetComponent<ChouCroissant>();
        yield return null;

        // Trois jours pour pousser :
        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        var chouPret = chou.GetComponent<ChouPret>();

        chouPret.Ramasser(inventaire);
        yield return null;

        // ASSERT
        Assert.AreEqual(inventaire.Choux, 1);
    }

    [UnityTest]
    public IEnumerator TestChouPerdGraine()
    {
        // TODO: Tester que quand on vient de planter un chou, l'inventaire a une graine en moins
        //
        // Faites un :         yield return null;
        // aprÃ¨s avoir plantÃ© le chou, question de simuler qu'au moins 1 frame s'est Ã©coulÃ©e avant que
        // vous fassiez votre test

        // ARRANGE
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 1;
        emplacement.Planter(inventaire);
        yield return null;

        // ASSERT
        Assert.AreEqual(inventaire.Graines, 0);
    }

    [UnityTest]
    public IEnumerator TestChouJourneesPassees()
    {
        // TODO: Tester qu'au bout de 3 jours, le chou est prÃªt Ã  se faire cueillir
        //
        // Faites un :         yield return null;
        // aprÃ¨s chaque appel de la mÃ©thode JourneePassee(); du composant ChouCroissant, question de simuler
        // qu'au moins 1 frame s'Ã©coule entre chaque appel

        // ARRANGE
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 1;
        emplacement.Planter(inventaire);
        yield return null;
        var chouCroissant = chou.GetComponent<ChouCroissant>();

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        var chouPret = chou.GetComponent<ChouPret>();

        // ASSERT
        Assert.IsTrue(chouPret != null);
    }

    [UnityTest]
    public IEnumerator TestChouReplanter()
    {
        // TODO: VÃ©rifier qu'on peut replanter un deuxiÃ¨me chou sur le mÃªme emplacement
        // aprÃ¨s l'avoir cueilli

        // ARRANGE 
        var emplacement = chou.GetComponent<EmplacementChouVide>();

        // ACT
        inventaire.Graines = 2;
        inventaire.Choux = 0;
        emplacement.Planter(inventaire);
        yield return null;
        var chouCroissant = chou.GetComponent<ChouCroissant>();

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        chouCroissant.JourneePassee();
        yield return null;

        var chouPret = chou.GetComponent<ChouPret>();
        chouPret.Ramasser(inventaire);
        yield return null;
   
        emplacement = chou.GetComponent<EmplacementChouVide>();
        emplacement.Planter(inventaire);
        yield return null;

        //ASSERT
        Assert.AreEqual(inventaire.Graines, 0);
    }
}