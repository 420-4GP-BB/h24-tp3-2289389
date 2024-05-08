using UnityEngine;
public interface IPoussable : IActionnable
{
    Transform transform { get; }
    void Pousser(Inventaire inventaireJoueur);
    void AfficherBuche();

}