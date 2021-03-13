using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float jumpIter = 4.5f;   // Koliko jako ce se kamera tresti

    public void Shake ()
    {
        float height = Mathf.PerlinNoise ( jumpIter, 0f ) * 5;
        height = height * height * 0.2f;

        float shakeAmt = height;        // * 0.01f; // stupnjevi po kojima se kamera trese
        float shakePeriodTime = 0.25f;  // period
        float dropOffTime = 1.25f;      // koliko dugo treba da se stabilizira

        LTDescr shakeTween = LeanTween.rotateAroundLocal ( gameObject, Vector3.right, shakeAmt, shakePeriodTime ).setEase ( LeanTweenType.easeShake ).setLoopClamp ().setRepeat ( -1 );

        // LeanTween ce koristiti easeShake od sve dok shakeAmt ne postane 0, a trebat ce 1.25 sekundi
        LeanTween.value ( gameObject, shakeAmt, 0, dropOffTime ).setOnUpdate ( ( float val ) => { shakeTween.setTo ( Vector3.right * val ); } ).setEase ( LeanTweenType.easeOutQuad );
    }
}
