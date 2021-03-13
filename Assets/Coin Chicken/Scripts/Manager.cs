using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : MonoBehaviour
{
    public int levelCount = 50;
    public Text coin = null;                        // Brojac novcica
    public Text distance = null;                    // Udaljenost koju je Igrac presao
    public Camera camera = null;                    // Pristup kameri za micanje i shake
    public GameObject guiGameOver = null;           // GUI za Game Over screen
    public LevelGenerator levelGenerator = null;

    private int currentCoins = 0;                   // Trenutni broj novcica
    private int currentDistance = 0;                // Trenutan udaljenost
    private bool canPlay = false;

    // Mozemo pristupatu postavkama Managera bez da ga dodjeljujemo objektima
    private static Manager s_Instance;
    public static Manager instance
    {
        get
        {
            if ( s_Instance == null )
            {
                s_Instance = FindObjectOfType ( typeof ( Manager ) ) as Manager;
            }

            return s_Instance;
        }
    }

    // Svaki puta kada se starta Manager loopa RandomGenerator levelCount puta
    void Start ()
    {
        for ( int i = 0; i < levelCount; i++ )
        {
            levelGenerator.RandomGenerator ();
        }
    }

    // Updatea broj pokupljanih novcica
    public void UpdateCoinCount ( int value )
    {
        Debug.Log ( "Player picked up another coin for " + value );

        currentCoins += value;

        coin.text = currentCoins.ToString ();
    }

    // Updatea udaljenost koju je igrac prosao
    public void UpdateDistanceCount ()
    {
        Debug.Log ( "Player moved forward for one point" );

        currentDistance += 1;

        distance.text = currentDistance.ToString ();

        // Svaki puta kada se Igrac pomakne unaprijed generira se level dalje
        levelGenerator.RandomGenerator ();
    }

    // Kada je igrac spreman pokrece igru kada je to moguce
    public bool CanPlay ()
    {
        return canPlay;
    }
    public void StartPlay ()
    {
        canPlay = true;
    }

    // Kada je igrac pogoden (gotHit), pauzira kameru i potrese je
    public void GameOver ()
    {
        camera.GetComponent<CameraShake> ().Shake ();
        camera.GetComponent<CameraFollow> ().enabled = false;

        GuiGameOver ();
    }

    // Postavlja Game Over screen
    void GuiGameOver ()
    {
        Debug.Log ( "Game over!" );

        guiGameOver.SetActive ( true );
    }

    // Postavlja scenu za ponovnu igru pomocu SceneManager-a, svaki puta ista scena sa random generiranom mapom
    public void PlayAgain ()
    {
        Scene scene = SceneManager.GetActiveScene ();

        SceneManager.LoadScene ( scene.name );
    }

    // Za izlaz iz igre
    public void Quit ()
    {
        Application.Quit ();
    }
}
