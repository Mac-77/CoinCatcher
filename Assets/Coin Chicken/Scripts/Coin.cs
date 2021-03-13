using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;           // Vrijednost jednog novcica
    public GameObject coin = null;      // Objekt koji unistimo nakon sto pustimo Audio Clip
    public AudioClip audioClip = null;  // Audio clip koji cemo pustiti kada stanemo na objekt

    // Kada Igrac ude u objekt, objekt ce nestati, povecati kolicinu novcica za 1 i pustiti audio clip
    void OnTriggerEnter ( Collider other )
    {
        if ( other.tag == "Player" )
        {
            Debug.Log ( "Player picked up a coin!" );

            Manager.instance.UpdateCoinCount ( coinValue );

            coin.SetActive ( false );   // Prvo maknemo vizualni prikaz novcica

            this.GetComponent<AudioSource> ().PlayOneShot ( audioClip );    // U komponentu AudioSorce postavio audioClip koji se pusti

            Destroy ( this.gameObject, audioClip.length );  // Objekt se unisti nakon duljine Audio Clipa
        }
    }
}
