using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour
{
    public GameObject light = null;

    // Provjerava dali je objekt sa tagom Train unutar ovog objekta i prema tome pali lampicu
    void OnTriggerEnter ( Collider other )
    {
        if ( other.tag == "train" )
        {
            light.SetActive ( true );
        }
    }

    void OnTriggerExit ( Collider other )
    {
        if ( other.tag == "train" )
        {
            light.SetActive ( false );
        }
    }
}
