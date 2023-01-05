using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsDead : MonoBehaviour
{
    private MinionCharacter minionCharacter;

    // Start is called before the first frame update
    void Start()
    {
        minionCharacter = gameObject.GetComponent<MinionCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(minionCharacter.currentHealth <= 0){
            StartCoroutine(FinishGame());
        }
    }

    private IEnumerator FinishGame(){
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
}
