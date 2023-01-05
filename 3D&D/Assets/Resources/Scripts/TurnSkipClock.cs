using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSkipClock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetEnabledAnimation(false);
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
    }

    public void EnableAnimation()
    {
        SetEnabledAnimation(true);
        GetComponentInChildren<ParticleSystem>().enableEmission = true;


    }
    public void DisableAnimation()
    {
        SetEnabledAnimation(false);
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
    }
    private void SetEnabledAnimation(bool value)
    {
        foreach(Animator anim in GetComponentsInChildren<Animator>())
        {
            anim.enabled = value;
        }
    }
}
