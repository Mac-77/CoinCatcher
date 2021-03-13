using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> platform = new List<GameObject> (); // Lista objekata koji se mogu generirati
    public List<float> height = new List<float> ();

    private int rndRange = 0;       // Kolicina platforma za redom
    private float lastPos = 0;      // Na kojoj poziciji je stvoren zadnji objekt, da znamo na kojoj ce biti sljedeci
    private float lastScale = 0;    // Velicina zadnje stvorenog objekta, tako da znamo za koliko pomaknuti sljedeci na z-osi

    // Stvara nasumican broj platforma (Iz liste platform) koje ce se generirati, poziva se iz managera na Start
    public void RandomGenerator ()
    {
        rndRange = Random.Range ( 0, platform.Count );

        for ( int i = 0; i < platform.Count; i++ )
        {
            CreateLevelObject ( platform [ i ], height [ i ], i );
        }
    }

    // Stvara objekte, svaki pomice unaprijed ovisno o poziciji i skali prethodnog
    public void CreateLevelObject ( GameObject obj, float height, int value )
    {
        if ( rndRange == value )
        {
            GameObject go = Instantiate ( obj ) as GameObject;

            float offset = lastPos + ( lastScale * 0.5f );      // Uzimamo pola vrijednosti da nademo centar
            offset += ( go.transform.localScale.z ) * 0.5f;     // Ukupno dostupanje pozicije od zadnjeg objekta
            Vector3 pos = new Vector3 ( 0, height, offset );    // Stvaranje vektora nove pozicije sa istim y i z uvecanim za offset

            go.transform.position = pos;

            lastPos = go.transform.position.z;          // Pozicija ovog objekta se sprema u lastPos kao zadnja pozicija
            lastScale = go.transform.localScale.z;      // Skala ovog objekta u zadnje koristenu skalu

            go.transform.parent = this.transform;       // Sve roditelje stavljamo u jedan objekt
        }
    }
}
