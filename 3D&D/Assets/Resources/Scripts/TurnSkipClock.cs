using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSkipClock : MonoBehaviour
{
    public GameObject generator;
    private ChangePosition cam;
    public bool isLooked = false; 
    // Start is called before the first frame update
    void Start()
    {
        this.cam = FindObjectOfType<ChangePosition>();
        SetEnabledAnimation(false);
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
    }

    public void EnableAnimation()
    {
        isLooked = true;
        SetEnabledAnimation(true);
        GetComponentInChildren<ParticleSystem>().enableEmission = true;
        

    }
    public void DisableAnimation()
    {
        isLooked = false;
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
    private void Update()
    {
        if (isLooked && Input.GetAxis("Fire1") > 0)
        {
            cam.enabled = true;
            cam.setChangePosition(true);
            generator.GetComponent<GenerateAround>().SetRefill(true);
        }
    }
}
