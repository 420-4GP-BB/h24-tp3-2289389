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
    private GameObject[] _pointsRenard;

    private Soleil _soleil;

    void Start()
    {
        _soleil = GameObject.FindObjectOfType<Soleil>();

        _zoneRelachement = GameObject.Find("NavMeshObstacle");
        _joueur = GameObject.Find("Joueur");
        if (ParametresParties.Instance.caraIndex == 1)
            _joueur = GameObject.Find("Joueuse");
        _angleDerriere = Random.Range(-60.0f, 60.0f);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _pointsDeDeplacement = GameObject.FindGameObjectsWithTag("PointsPoulet");
        _pointsRenard = GameObject.FindGameObjectsWithTag("PointRenard");
        //Debug.Log(_pointsRenard.Length);
        //Debug.Log(_pointsDeDeplacement.Length);
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
        _animator.SetBool("Walk", true);
        GameObject[] pointsDeDestination = _pointsDeDeplacement;
        if (_soleil.CurrentTimeOfDay >= 21.0f || _soleil.CurrentTimeOfDay < 8.0f)
        {
            int index = 0;
            pointsDeDestination = new GameObject[_pointsDeDeplacement.Length + _pointsRenard.Length];
            for (int i = 0; i < _pointsDeDeplacement.Length; i++)
            {
                pointsDeDestination[index++] = _pointsDeDeplacement[i];
            }
            for (int i = 0; i < _pointsRenard.Length; i++)
            {
                pointsDeDestination[index++] = _pointsRenard[i];
            }
            //Debug.Log(pointsDeDestination.Length);
        }

        GameObject point = pointsDeDestination[Random.Range(0, pointsDeDestination.Length)];
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
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                ChoisirDestinationAleatoire();
        }
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            if (_arriveFerme)
            {
                _arriveFerme = false;
                _agent.speed = 1.5f;
                _animator.SetBool("Walk", true);
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                    ChoisirDestinationAleatoire();
            }
            else
            {
                _suivreJoueur = true;
            }
        }
    }
}