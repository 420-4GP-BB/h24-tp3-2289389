using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Renard : MonoBehaviour
{
    private GameObject[] pointsRenard;
    private GameObject[] poulets;
    private GameObject cibleActuelle;
    private int index = 0;
    private Animator _animator;
    private NavMeshAgent _agent;
    private float distancePoursuite = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        pointsRenard = GameObject.FindGameObjectsWithTag("PointRenard");
        poulets = GameObject.FindGameObjectsWithTag("Poule");

        Destination();

        _animator.SetBool("Walk", true);
    }
    
    // Update is called once per frame
    void Update()
    {
        poulets = GameObject.FindGameObjectsWithTag("Poule");
        GameObject cibleProche = Cible();
        if (cibleProche != null)
        {
            PoursuivreCible(cibleProche);
        }
        else
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                Destination();
            }
        }
    }

    private void Destination()
    {
        index = Random.Range(0, pointsRenard.Length);
        _agent.SetDestination(pointsRenard[index].transform.position);
    }

    private GameObject Cible()
    {
        GameObject cibleProche = null;
        float distanceMin = distancePoursuite;

        foreach (GameObject poulet in poulets)
        {
            float distance = Vector3.Distance(transform.position, poulet.transform.position);
            if (distance < distanceMin)
            {
                cibleProche = poulet;
                distanceMin = distance;
            }
        }
        return cibleProche;
    }

    private void PoursuivreCible(GameObject cible)
    {
        cibleActuelle = cible;
        _agent.SetDestination(cible.transform.position);

        float distanceAuCible = Vector3.Distance(transform.position, cible.transform.position);
        if (distanceAuCible <= 0.9f)
        {
            StartCoroutine(DetruirePoule(cible));
        }
    }

    private IEnumerator DetruirePoule(GameObject poule)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(poule);

        cibleActuelle = null;
        GameObject cibleProche = Cible();
        if (cibleProche != null)
        {
            PoursuivreCible(cibleProche);
        }
        else
        {
            Destination();
        }
    }
}