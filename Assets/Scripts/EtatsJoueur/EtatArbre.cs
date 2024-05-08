using System.Collections;
using UnityEngine;

public class EtatArbre : EtatJoueur
{
    //https://forum.unity.com/threads/lerp-falling-rotating-object.544384/
    private IPoussable _arbre;
    private float _tempsPousse = 0f;
    private float _dureeMaxPousse = 2f;
    private float _dureeResteAuSol = 1f;
    private float _dureeDisparition = 0.5f;

    private bool _estEnTrainDeTomber = false;

    public override bool EstActif => true;
    public override bool DansDialogue => false;
    public override float EnergieDepensee => ConstantesJeu.COUT_POUSSER_ARBRE;

    public EtatArbre(ComportementJoueur sujet, IPoussable arbre) : base(sujet)
    {
        _arbre = arbre;
    }

    public override void Enter()
    {
        _tempsPousse = 0f;
        _estEnTrainDeTomber = false;
        Vector3 directionToTree = (_arbre.transform.position - Sujet.transform.position).normalized;
        Sujet.transform.rotation = Quaternion.LookRotation(directionToTree);
        Sujet.StartCoroutine(Animation());
        Sujet.StartCoroutine(FaireTomberArbre());
    }

    public override void Handle()
    {

    }
    private IEnumerator Animation()
    {
        Animateur.SetBool("Pousser", true);
        Debug.Log(Animateur.GetBool("Pousser"));
        while (_tempsPousse < _dureeMaxPousse - 0.3f)
        {
            yield return null;
        }

        Animateur.SetBool("Pousser", false);
    }
    private IEnumerator FaireTomberArbre()
    {
        while (_tempsPousse < _dureeMaxPousse)
        {
            _tempsPousse += Time.deltaTime;
            yield return null;
        }
        _estEnTrainDeTomber = true;
        Vector3 fallAngle = (_arbre.transform.position - Sujet.transform.position).normalized;
        Quaternion quatStart = _arbre.transform.rotation;
        Quaternion quatEnd = Quaternion.FromToRotation(Vector3.up, fallAngle) * _arbre.transform.rotation;
        float timeStart = Time.time;
        float timePassed;

        while (_estEnTrainDeTomber)
        {
            timePassed = Time.time - timeStart;
            _arbre.transform.rotation = Quaternion.Slerp(quatStart, quatEnd, timePassed);

            if (timePassed >= 1f)
            {
                _estEnTrainDeTomber = false;
            }

            yield return null;
        }
        yield return new WaitForSeconds(_dureeResteAuSol);


        _arbre.AfficherBuche();
        Sujet.ChangerEtat(Sujet.EtatNormal);
        yield return new WaitForSeconds(_dureeDisparition);
        _arbre.Pousser(Inventaire);
    }

    public override void Exit()
    {
    }
}