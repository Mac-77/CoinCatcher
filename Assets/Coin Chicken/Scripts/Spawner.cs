using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Transform startPos = null;

    // Stvaranje objekata bazirano na vremenu
    public float delayMin = 1.5f;
    public float delayMax = 5;
    public float speedMin = 1;  
    public float speedMax = 4;

    // Stvaranje objekata na pocetku
    public bool useSpawnPlacement = false;
    public int spawnCountMin = 4;
    public int spawnCountMax = 20;

    private float lastTime = 0;
    private float delayTime = 0;
    private float speed = 0;

    [ HideInInspector ] public GameObject item = null;
    [ HideInInspector ] public bool goLeft = false;
    [ HideInInspector ] public float spawnLeftPos = 0;
    [ HideInInspector ] public float spawnRightPos = 0;

    // Dali stvaramo stacionarni objekta (novcica) ili objekta koji se giba
    void Start ()
    {
        //Ako je stacionarni (coin), odredujemo random broj objekata i toliko ih stvaramo u petlji, inace odredimo brzinu gibanja
        if ( useSpawnPlacement )
        {
            int spawnCount = Random.Range ( spawnCountMin, spawnCountMax );

            for ( int i = 0; i < spawnCount; i++ )
            {
                SpawnItem ();
            }
        }
        else
        {
            speed = Random.Range ( speedMin, speedMax );
        }
    }

    // Konstantno stvaranje objekata, nakon nekog delay vremena
    void Update ()
    {
        // Ako je coin, ne spawnamo ih vise, samo jednom
        if ( useSpawnPlacement ) return;

        if ( Time.time > lastTime + delayTime )
        {
            lastTime = Time.time;

            delayTime = Random.Range ( delayMin, delayMax );

            SpawnItem ();
        }
    }

    // Stvaramo objekt i odredujemo poziciju i smijer
    void SpawnItem ()
    {
        Debug.Log ( "Spawn Item" );

        GameObject obj = Instantiate ( item ) as GameObject;

        obj.transform.position = GetSpawnPosition ();

        // Ako je odabrano goLeft stanje, zarotiramo objekt za 180
        float direction = 0; if ( goLeft ) direction = 180;

        // Ako nije novcic, moramo mu odrediti brzinu i smijer ovisno o strani na kojoj se stvorio
        if ( !useSpawnPlacement )
        {
            obj.GetComponent<Mover> ().speed = speed;

            obj.transform.rotation = obj.transform.rotation * Quaternion.Euler ( 0, direction, 0 );
        }
    }
    
    // Odabir izmedju stvaranja novcica ili objekta koji se pomice sa jednog kraja na drugi tj. odabira pozicije stvaranja objekta
    Vector3 GetSpawnPosition ()
    {
        // Ako koristimo useSpawnPlacement znaci da se radi o novcicu, i moze se stvoriti na random poziciji izmedju 2 krajnje pozicije
        if ( useSpawnPlacement )
        {
            int x = ( int ) Random.Range ( spawnLeftPos, spawnRightPos );

            Vector3 pos = new Vector3 ( x, startPos.position.y, startPos.position.z );

            return pos;
        }
        else
        {
            return startPos.position;
        }
    }
} 
