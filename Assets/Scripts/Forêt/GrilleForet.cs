using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleForet : IForet
{
    private int nombreRangees;
    private float espaceEntreRangees;
    private float espaceEntreArbres;
    private GameObject prefabArbre;
    private Rect zoneExclusion;

    public GrilleForet(int nombreRangees, float espaceEntreRangees, float espaceEntreArbres, GameObject prefabArbre, Rect zoneExclusion)
    {
        this.nombreRangees = nombreRangees;
        this.espaceEntreRangees = espaceEntreRangees;
        this.espaceEntreArbres = espaceEntreArbres;
        this.prefabArbre = prefabArbre;
        this.zoneExclusion = zoneExclusion;
    }

    public void GenererForet(Transform transformForet)
    {
        float largeurTerrain = 100f;
        float profondeurTerrain = 100f;

        int arbresParRangee = Mathf.FloorToInt(largeurTerrain / espaceEntreArbres);

        for (int i = 0; i < nombreRangees; i++)
        {
            float positionRangeeY = i * espaceEntreRangees;

            for (int j = 0; j < arbresParRangee; j++)
            {
                float positionArbresX = j * espaceEntreArbres + Random.Range(-espaceEntreArbres * 0.25f, espaceEntreArbres * 0.25f);
                float positionArbresZ = positionRangeeY + Random.Range(-espaceEntreRangees * 0.25f, espaceEntreRangees * 0.25f);
                Vector3 positionArbre = new Vector3(positionArbresX, 0f, positionArbresZ);

                if (!zoneExclusion.Contains(positionArbre))
                {
                 //   Instantiate(prefabArbre, positionArbre, Quaternion.identity, transformForet);
                }
            }
        }
    }
}