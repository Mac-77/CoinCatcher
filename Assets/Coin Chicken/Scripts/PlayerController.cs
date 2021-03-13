using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float          moveDistance              = 1;        // Udaljenost za koju se Igrac pomice prilikom pritiska gumba
    public float          moveTime                  = 0.4f;     // Vrijeme potrebno za jedan pomak
    public float          colliderDistCheck         = 1;        // Provjerava dali je ispred Igraca podrucje na koje se nemoze pomaknuti za udaljenost 1 (pomak)
    public bool           isIdle                    = true;     // Provjeravanje pozicije u kojoj se Igrac trenutno nalazi
    public bool           isDead                    = false;
    public bool           isMoving                  = false;
    public bool           isJumping                 = false;
    public bool           jumpStart                 = false;
    public bool           enableAngleCheckOnMove    = false;    // Dali treba provjera lijevo i desno u slucaju da se Igrac pomice na objektu
    public float          angleCheck                = 1;        // Pomak u lijevo ili desno za Angle Check
    public float          angleCheckDist            = 0.5f;     // Dodatna udaljenost od Origina za Raycasting u lijevo ili desno
    public ParticleSystem particle                  = null;     // Sluzi za ukljucivanje Emitera za ParticleSystem
    public GameObject     chick                     = null;     // Objekt u igri
    private Renderer      renderer                  = null;     // Provjerava dali je Igrac unutar kamere tj, dali je vidljiv na ekranu
    private bool          isVisible                 = false;    // Sluzi za istu stvar kao i renderer

    public AudioClip audioIdle1     = null;             // Audio klipovi koje cemo pustati za odredene animacije/stanja
    public AudioClip audioIdle2     = null;
    public AudioClip audioHop       = null;
    public AudioClip audioHit       = null;
    public AudioClip audioSplash    = null;

    public ParticleSystem splash = null;                // Za particle system kada Igrac padne u vodu
    public bool parentedToObject = false;               // Za provjeru dali je Igrac povezan na objekt (drvo)

    //Stvara renderer od komponente chick za provjeru dali kamera vidi objekt
    void Start ()
    {
        renderer = chick.GetComponent<Renderer> ();
    }

    //Ako je Igrac mrtav ili izvan polja vidljivosti kamere sprijecava izvrsavanje radnji, i ne krece dok Manager ne postavi CanPlay na true, svaki frame
    void Update ()
    {
        if ( !Manager.instance.CanPlay () ) return;

        if ( isDead ) return;
        
        CanIdle ();

        CanMove ();

        IsVisible ();
    }

    // Provjerava ako je igrac u Idle stanju i pritisku tipke za pomak tj. jumpStart stanje
    void CanIdle ()
    {
        //Ovdje se koristi GetKeyDown jer kada pritisnemo tipku tek ide u JumpStart animaciju, a tek kada otpustimo tipku u Jump i salje rotaciju na CheckIfIdle ovisno o tipki/smijeru
        if ( isIdle )
        {
            if ( Input.GetKey ( KeyCode.UpArrow    ) ) { CheckIfIdle ( 270,   0, 0 ); }
            if ( Input.GetKey ( KeyCode.DownArrow  ) ) { CheckIfIdle ( 270, 180, 0 ); }
            if ( Input.GetKey ( KeyCode.LeftArrow  ) ) { CheckIfIdle ( 270, -90, 0 ); }
            if ( Input.GetKey ( KeyCode.RightArrow ) ) { CheckIfIdle ( 270,  90, 0 ); }
        }
    }

    //Igrac je bio u Idle stanju i zatim ide u JumpStart stanje, okrece igraca ovisno o pritisnutoj tipki
    void CheckIfIdle ( float x, float y, float z )
    {
        chick.transform.rotation = Quaternion.Euler ( x, y, z );

        if ( enableAngleCheckOnMove )
        {
            CheckIfCanMoveAngles();
        }
        else
        {
            CheckIfCanMoveSingleRay();
        }

        // Uzimamo random range da se zvuk ne pusta konstantno
        int a = Random.Range ( 0, 12 );
        if ( a < 4 ) PlayAudioClip ( audioIdle1 );
    }
    
    // Provjerava dali se igrac moze Pomaknuti (nema collidera ispred) u jednom smijeru
    void CheckIfCanMoveSingleRay ()
    {
        //Raycast provjerava dali je Collider ispred Igraca i sprijecava pomak u tome smijeru
        //Nacin na koji radi je da salje liniju od pocetne tocke to krajnje tocke i ako pogodi neki objekt dobit cemo nazad informacije o tome
        RaycastHit hit;

        Physics.Raycast ( this.transform.position, -chick.transform.up, out hit, colliderDistCheck );

        Debug.DrawRay ( this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2 ); //, Color.red, 2 za prikaz linije

        if ( hit.collider == null )
        {
            SetMove ();
        }
        else
        {
            //Provjerava se dali Objekt ispred nas ima Tag "collider"
            if ( hit.collider.tag == "collider" )
            {
                Debug.Log ( "Hit something with collider tag." );

                isIdle = true;
            }
            else
            {
                SetMove ();
            }
        }
    }

    // Raycasting u vise smijerova
    void CheckIfCanMoveAngles()
    {
        RaycastHit hit;
        RaycastHit hitLeft;
        RaycastHit hitRight;

        // Provjerava od trenutne pozicije, u smijeru suprotno od gore jer je model originalno zarotiran za 90
        // Za provjere lijevo i desno pomice origin provjere ulijevo za 1 (x+1) ili ako je -angleCheck u desno (x-1) i za maksimalnu udaljenost nadodaje 0.5 
        Physics.Raycast ( this.transform.position, -chick.transform.up, out hit, colliderDistCheck );
        Physics.Raycast ( this.transform.position, -chick.transform.up + new Vector3 ( angleCheck, 0, 0 ), out hitLeft, colliderDistCheck + angleCheckDist );
        Physics.Raycast ( this.transform.position, -chick.transform.up + new Vector3 ( -angleCheck, 0, 0 ), out hitRight, colliderDistCheck + angleCheckDist );

        Debug.DrawRay ( this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2 );
        Debug.DrawRay ( this.transform.position, ( -chick.transform.up + new Vector3 ( angleCheck, 0, 0 ) ) * ( colliderDistCheck + angleCheckDist ), Color.green, 2 );
        Debug.DrawRay ( this.transform.position, ( -chick.transform.up + new Vector3 ( -angleCheck, 0, 0 ) ) * ( colliderDistCheck + angleCheckDist ), Color.blue, 2 );

        // Ako Raycast vrati informaciju da nije naletio na collider, mozemo se micati
        if ( hit.collider == null && hitLeft.collider == null && hitRight.collider == null )
        {
            SetMove();
        }
        else
        {
            if ( hit.collider != null && hit.collider.tag == "collider" )
            {
                Debug.Log ( "Hit something with collider." );
                isIdle = true;
            }
            else if ( hitLeft.collider != null && hitLeft.collider.tag == "collider" )
            {
                Debug.Log ( "Hit Left something with collider.");

                isIdle = true;
            }
            else if ( hitRight.collider != null && hitRight.collider.tag == "collider" )
            {
                Debug.Log ( "Hit Right something with collider.");

                isIdle = true;
            }
            else
            {
                SetMove ();
            }
        }

    }

    //Postavlja u Move stanje
    void SetMove ()
    {
        Debug.Log ( "Hit nothing. Keep moving." );

        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }

    // Provjerava dali se Igrac moze pomaknuti i zatim ovisno o pritisnutoj tipki salje dalje vektor sa izmjenom vrijednost ovisno o smijeru (x ili z)
    void CanMove ()
    {
        //Ovdje koristimo GetKeyUp jer u stanje pomicanja (Jump) se ide tek nakon otpustanja tipke
        if ( isMoving )
        {
                 if ( Input.GetKeyUp ( KeyCode.UpArrow    ) ) { Moving ( new Vector3 ( transform.position.x, transform.position.y, transform.position.z + moveDistance ) ); SetMoveForwardState (); }
            else if ( Input.GetKeyUp ( KeyCode.DownArrow  ) ) { Moving ( new Vector3 ( transform.position.x, transform.position.y, transform.position.z - moveDistance ) ); }
            else if ( Input.GetKeyUp ( KeyCode.LeftArrow  ) ) { Moving ( new Vector3 ( transform.position.x - moveDistance, transform.position.y, transform.position.z ) ); }
            else if ( Input.GetKeyUp ( KeyCode.RightArrow ) ) { Moving ( new Vector3 ( transform.position.x + moveDistance, transform.position.y, transform.position.z ) ); }
        }
    }

    //Pomicanje Igraca ovisno o novom vektoru, koristi se plugin LeanTweem
    void Moving ( Vector3 pos )
    {
        //Posto je pomicanje Jumping animacija, samo se isJumping postavlja na true
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;

        PlayAudioClip ( audioHop );

        //LeanTween postavlja Move Complete kako bi se opet mogli pomaknuti
        LeanTween.move ( this.gameObject, pos, moveTime ).setOnComplete ( MoveComplete );
    }

    //Pomicanje Igraca obavljeno, reseta varijable za novi pomak
    void MoveComplete ()
    {
        isJumping = false;
        isIdle = true;

        // Uzimamo random range da se zvuk ne pusta konstantno
        int a = Random.Range ( 0, 12 );
        if ( a < 4 ) PlayAudioClip ( audioIdle2 );
    }

    // Daje nam do znanja da se Igrac pomaknu unaprijed, daje bodove za pomicanje unaprijed i trazi generiranje levela
    void SetMoveForwardState ()
    {
        Manager.instance.UpdateDistanceCount ();
    }

    //Provjerava dali je Igrac u vidnom polju kamere, ako nije ide u GotHit stanje tj. Game Over
    void IsVisible ()
    {
        if ( renderer.isVisible )
        {
            isVisible = true;
        }

        if ( !renderer.isVisible && isVisible )
        {
            Debug.Log ( "Player off screen. Apply GotHit()" );

            GotHit ();
        }

    }

    //Daje do znanja da je Igrac pogoden, postavlja stanje u isDead i ukljucuje Particle Emission
    public void GotHit ()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = particle.emission;
        em.enabled = true;

        PlayAudioClip ( audioHit );

        // Game Manageru dajemo do znanja da je Game Over stanje
        Manager.instance.GameOver ();
    }

    //Daje do znanja da je Igrac pao u vodu i ukljucuje Particle Emission za tu situaciju, pokrece Audio clip i daje do znanja Manageru da je Game Over
    public void GotSoaked ()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = splash.emission;
        em.enabled = true;

        PlayAudioClip ( audioSplash );

        chick.SetActive ( false );

        Manager.instance.GameOver ();
    }

    //Pokrece Audio clip
    void PlayAudioClip ( AudioClip clip )
    {
        this.GetComponent<AudioSource> ().PlayOneShot ( clip );
    }
}
