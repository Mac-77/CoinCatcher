using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float speed = 0.25f;                         // Brzina automatskog kretanja kamere
    public bool autoMove = true;                        // Odreduje dali ce se kamera automatski kretati po z-osi
    public GameObject player = null;
    public Vector3 offset = new Vector3 ( 3, 6, -3 );   // Inicijalno odstupanje kamere
    Vector3 depth = Vector3.zero;
    Vector3 pos = Vector3.zero;

    void Update ()
    {
        // Ako nismo krenuli igrat kamera se nece automatski pomicati
        if ( !Manager.instance.CanPlay () ) return;

        // Kamera se automatski krece po z-osi, ali se i pomice za igracem ovisno u kojem smijeru se krece
        if ( autoMove )
        {
            // 
            depth = this.gameObject.transform.position += new Vector3 ( 0, 0, speed * Time.deltaTime );
            pos = Vector3.Lerp ( gameObject.transform.position, player.transform.position + offset, Time.deltaTime );
            gameObject.transform.position = new Vector3 ( pos.x, offset.y, depth.z );
        }
        else
        {
            pos = Vector3.Lerp ( gameObject.transform.position, player.transform.position + offset, Time.deltaTime );
            gameObject.transform.position = new Vector3 ( pos.x, offset.y, pos.z );
        }
    }
}
