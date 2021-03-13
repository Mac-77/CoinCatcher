using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed = 1.0f;              // Brzina gibanja objekta
    public float moveDirection = 0;         // Smijer gibanja objekta
    public bool parentOnTrigger = true;     // Dali se moze skociti na objekt koji se giba
    public bool hitBoxOnTrigger = false;    // Dali objekt dodirom sa igracem postavlja isDead stanje
    public GameObject moverObject = null;   // Deklariranje entiteta

    private Renderer renderer = null;
    private bool isVisible = false;         // Provjerava dali je objekt koji se giba u vidnom polju kamere

    void Start ()
    {
        renderer = moverObject.GetComponent<Renderer> ();
    }
    void Update ()
    {
        this.transform.Translate ( speed * Time.deltaTime, 0, 0 );

        IsVisible ();
    }

    // Ako objekt nije u vidnom polju kamere brise ga
    void IsVisible ()
    {
        if ( renderer.isVisible )
        {
            isVisible = true;
        }

        if ( !renderer.isVisible && isVisible )
        {
            Debug.Log ( "Remove object. No longer seen by camera." );

            Destroy ( this.gameObject );
        }
    }

    // Provjeravaju dali ce se objekt sudariti sa Igracem, ulazi u igraca (other je Igrac)
    void OnTriggerEnter ( Collider other )
    {
        if ( other.tag == "Player" )
        {
            Debug.Log ( "Enter." );

            // Ako objekt moze postati parent kada se dogodi okidac, postavlja poziciju objekta na sebe
            if ( parentOnTrigger )
            {
                Debug.Log ( "Enter: Parent to me" );

                other.transform.parent = this.transform;

                // U Player objektu postavljamo true da mu je sada ovaj objekt parent tj. da je parented
                PlayerController pc = other.GetComponent<PlayerController>();
                pc.enableAngleCheckOnMove = true;
                pc.parentedToObject = true; 

            }

            // Ako je objekt neprijatelj prilikom sudara, postavlja stanje Igraca na GotHit
            if ( hitBoxOnTrigger )
            {
                Debug.Log ( "Enter: Gothit. Game over." );

                other.GetComponent<PlayerController> ().GotHit ();
            }
        }
    }

    // Objekt izlazi iz Igraca i ako je parent, prestane postavljati poziciju igraca na svoju
    void OnTriggerExit ( Collider other )
    {
        if ( other.tag == "Player" )
        {
            if ( parentOnTrigger )
            {
                Debug.Log ( "Exit." );

                other.transform.parent = null;

                PlayerController pc = other.GetComponent<PlayerController>();
                pc.enableAngleCheckOnMove = false;
                pc.parentedToObject = false;
            }
        }
    }
}
