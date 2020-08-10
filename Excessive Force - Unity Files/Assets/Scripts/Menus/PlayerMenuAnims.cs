using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuAnims : MonoBehaviour
{
    public Animator animController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IdleAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator IdleAnimation()
    {
        float idleTime = Random.Range(30, 90);
        yield return new WaitForSeconds(idleTime);

        EyeAnimations.lookAtTarget = false;
        EyeAnimations.canBlink = false;

        animController.SetTrigger("Stretch");
        yield return new WaitForSeconds(5);

        EyeAnimations.lookAtTarget = true;

        StartCoroutine(IdleAnimation());
    }
}
