using UnityEngine;
using UnityEngine.AI;

public class MouvementPoulet : MonoBehaviour
{
    private GameObject _zoneRelachement;
    private float _angleDerriere;
    private GameObject _joueur;
    private bool _suivreJoueur = true;
    private bool _arriveFerme = false;
    private bool _estInitialise = false;
    private float _distanceJoueur = 3f;

    private NavMeshAgent _agent;
    private Animator _animator;

    private GameObject[] _pointsDeDeplacement;

    void Start()
    {
        _zoneRelachement = GameObject.Find("NavMeshObstacle");
        _joueur = GameObject.Find("Joueur");
        _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pointsDeDeplacement = GameObject.FindGameObjectsWithTag("PointsPoulet");
        _animator.SetBool("Walk", true);
        if (!_estInitialise)
        {
            Initialiser();
            _estInitialise = true;
        }
    }

    void Initialiser()
    {
        Vector3 nouvellePosition = TrouverEspace(_joueur.transform.position, 1.5f);
        _agent.enabled = false;
        var point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)];
        transform.position = nouvellePosition;
        _agent.enabled = true;
        if (Vector3.Distance(transform.position, _joueur.transform.position) > 3.0f)
        {
            if (nouvellePosition != Vector3.zero)
            {
                transform.position = nouvellePosition;
            }

        }

        gameObject.GetComponent<PondreOeufs>().enabled = true;
        _suivreJoueur = true;
        _arriveFerme = false;
        _agent.speed = 4f;

        ChoisirDestinationAleatoire();
    }

    Vector3 TrouverEspace(Vector3 centre, float rayon)
    {
        Vector3 positionApparition = Vector3.zero;
        int tentatives = 10;
        while (tentatives > 0)
        {
            Vector3 directionAleatoire = Random.insideUnitSphere * rayon;
            directionAleatoire += centre;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(directionAleatoire, out hit, rayon, NavMesh.AllAreas))
            {
                positionApparition = hit.position;
                break;
            }
            tentatives--;
        }
        return positionApparition;
    }

    void ChoisirDestinationAleatoire()
    {
        GameObject point = _pointsDeDeplacement[Random.Range(0, _pointsDeDeplacement.Length)];
        _agent.SetDestination(point.transform.position);
    }

    void Update()
    {
        float distancePoulet = Vector3.Distance(transform.position, _joueur.transform.position);
        bool joueurDansMaison = Vector3.Distance(_joueur.transform.position, _zoneRelachement.transform.position) <= _zoneRelachement.GetComponent<BoxCollider>().size.magnitude / 2f;

        if (_suivreJoueur && !joueurDansMaison)
        {
            if (distancePoulet < _distanceJoueur)
            {
                _agent.SetDestination(transform.position);
                _animator.SetBool("Walk", false);
            }
            else
            {
                Vector3 directionPoulet = (_joueur.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(directionPoulet);
                _agent.SetDestination(_joueur.transform.position - directionPoulet * _distanceJoueur);
                _animator.SetBool("Walk", true);
            }
        }
        else
        {
            _arriveFerme = true;
            _suivreJoueur = false;
            _animator.SetBool("Walk", false);
            ChoisirDestinationAleatoire();
        }
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            if (_arriveFerme)
            {
                ChoisirDestinationAleatoire();
                _arriveFerme = false;
                _agent.speed = 1.5f;
            }
            else
            {
                _suivreJoueur = true;
            }
        }
    }
}