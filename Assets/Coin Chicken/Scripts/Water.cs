using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    bool hitWater = false;

    // 
    void OnTriggerStay ( Collider other )
    {
        // Ako je vec pogodio vodu, ne izvodi skriptu
        if ( hitWater ) return;

        // Ako je objekt sa tagom Player dosao u kontakt sa ovim objektom
        if ( other.tag == "Player" )
        {
            PlayerController playerController = other.GetComponent<PlayerController> ();

            // Ako igrac nije u stanju skakanja i nije parentan na objekt, pokrece GotSoaked na PlayerController (Pao je u vodu i Game Over)
            if ( !playerController.parentedToObject && !playerController.isJumping )
            {
                Debug.Log ( "Player fell in to the water!" );

                hitWater = true;

                playerController.GotSoaked ();
            }
        }
    }
}
