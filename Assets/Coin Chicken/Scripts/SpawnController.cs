using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
    public bool goLeft = false;
    public bool goRight = false;
    public bool both = false;
    public List<GameObject> items = new List<GameObject> ();    // Lista svih objekata koji se mogu stvarati na ovom tipu spawnera, npr. za cestu svi auti, za tracnice svi vlakovi
    public List<Spawner> spawnersLeft = new List<Spawner> ();   // Lista svih spawnera koji su lijevo
    public List<Spawner> spawnersRight = new List<Spawner> ();  // Lista svih spawnera koji su desno

     // Pokrece se pri startanju igre
    void Start ()
    {
        // Odabire random objekt u listi ovisno o random ID-u (crveni auto ili narancasti auto ..itd)
        int itemId = Random.Range ( 0, items.Count );        
        GameObject item = items [ itemId ];

        // Odabir nasumicnog smijera gibanja
        int direction = Random.Range ( 0, 2 );

        if ( both ) { goLeft = true; goRight = true; }
        else if ( direction > 0 ) { goLeft = false; goRight = true; } else { goLeft = true; goRight = false; }

        for ( int i = 0; i < spawnersLeft.Count; i++ )
        {
            spawnersLeft [ i ].item = item;                                             // Postavljamo item u Spawner.cs na ovaj u SpawnConroller.cs koji dolazi iz liste items
            spawnersLeft [ i ].goLeft = goLeft;                                         // Postavljamo goLeft na true ili false ovisno o odabiru gore za smijer
            spawnersLeft [ i ].gameObject.SetActive ( goRight );                        // ovisno o goLeft postavljamo goRight na true ili false
            spawnersLeft [ i ].spawnLeftPos = spawnersLeft [ i ].transform.position.x;  // Postavljamo poziciju iz spawnersLeft liste
        }

        for ( int i = 0; i < spawnersRight.Count; i++ )
        {
            spawnersRight [ i ].item = item;
            spawnersRight [ i ].goLeft = goLeft;
            spawnersRight [ i ].gameObject.SetActive ( goLeft );
            spawnersRight [ i ].spawnRightPos = spawnersRight [ i ].transform.position.x;
        }
    }
}
