using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // This should not be used on production it is only to test the behaviour in the editor
    // public bool testIsSelected = false;
    private bool isSelected = false;
    private ParticleSystem particleSystem;


    // Start is called before the first frame update
    void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Test
        // if (testIsSelected != isSelected)
        // {
        //     if (testIsSelected)
        //     {
        //         Select();
        //     }
        //     else
        //     {
        //         Deselect();
        //     }
        // }
    }

    public void Select()
    {
        isSelected = true;
        particleSystem.Play();
    }

    public void Deselect()
    {
        isSelected = false;
        particleSystem.Stop();
    }
}

