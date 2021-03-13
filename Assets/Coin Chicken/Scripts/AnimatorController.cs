using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour
{
    //Povezuje se sa PlayerController skriptom u kojoj su varijable stanja i koristi ih za odabir animacija
    public PlayerController playerController = null; 
    private Animator animator = null;

    //Postavlja animator na Animator komponentu chick objekta
    void Start ()
    {
        animator = this.GetComponent<Animator> ();
    }

    //Provjerava u kojem je stanju objekt i prema tome pokrece prikladnu animaciju
    void Update ()
    {
        //Provjera stanja iz PlayerController skripte i postavljanje parametra u Unity-u za tranzicije i animacije
        if ( playerController.isDead )
        {
            animator.SetBool ( "dead", true );
        }
        if ( playerController.jumpStart )
        {
            animator.SetBool ( "jumpStart", true );
        }
        else if ( playerController.isJumping )
        {
            animator.SetBool ( "jump", true );
        }
        else
        {
            animator.SetBool ( "jump", false );
            animator.SetBool ( "jumpStart", false );
        }
    }
}
