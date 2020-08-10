using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimations : MonoBehaviour
{
    [Header("Left Eye")]
    public GameObject leftEyeBone;
    public GameObject leftEye;
    public GameObject leftEyeLid;

    [Header("Right Eye")]
    public GameObject rightEyeBone;
    public GameObject rightEye;
    public GameObject rightEyeLid;

    [Header("Animation")]
    public GameObject lookTarget;
    public static bool lookAtTarget = true;
    public static bool canBlink = true;
    public Animator animController;
    public enum EyeExpressions
    {
        DEFAULT,
        ANGRY,
        SAD
    };
    public EyeExpressions currentExpression = EyeExpressions.DEFAULT;
    
    // Start is called before the first frame update
    void Start()
    {
        // Blinking
        StartCoroutine(DelayBlink());
    }

    // Update is called once per frame
    void Update()
    {
        // Left Eye
        leftEye.transform.position = leftEyeBone.transform.position;
        leftEyeLid.transform.position = leftEyeBone.transform.position;

        // Right Eye
        rightEye.transform.position = rightEyeBone.transform.position;
        rightEyeLid.transform.position = rightEyeBone.transform.position;

        // Default Looking At Target Point
        if (!lookAtTarget)
        {
            lookTarget.transform.localPosition = Vector3.zero;
        }
        LookAtTarget(new GameObject[] { rightEye, leftEye });

        // Eye Expression
        if (animController.gameObject.activeSelf)
        {
            animController.SetInteger("EyeExpression", (int)currentExpression);
        }
    }

    /*
    ====================================================================================================
    Looking At Target
    ====================================================================================================
    */
    private void LookAtTarget(GameObject eyeToLook)
    {
        Quaternion lookRotation = Quaternion.LookRotation(lookTarget.transform.position - eyeToLook.transform.position);
        eyeToLook.transform.rotation = lookRotation;
    }
    private void LookAtTarget(GameObject[] eyesToLook)
    {
        foreach (GameObject eye in eyesToLook)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookTarget.transform.position - eye.transform.position);
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, lookRotation, Time.deltaTime * 10);
        }
    }


    /*
    ====================================================================================================
    Eyelid Animations
    ====================================================================================================
    */
    public void Blink()
    {
        animController.SetTrigger("Blink");
    }
    public IEnumerator DelayBlink()
    {
        float nextBlink = Random.Range(2, 10);

        yield return new WaitForSeconds(nextBlink);

        if (canBlink)
        {
            // Randomly Change Expression
            int i = Random.Range(0, 10);
            if (i == 5)
            {
                currentExpression = (EyeExpressions)Random.Range(0, 3);
            }
            Blink();
        }

        StartCoroutine(DelayBlink());
    }
}
