using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public int activePlayer = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckGameIsOver)
            StartCoroutine(FinishGame());
    }

    public void ChangePlayer(){
        if(! CheckGameIsOver()){
            if(activePlayer == 1){
                activePlayer = 2;
            }else{
                activePlayer = 1;
            }
        }
    }

    private bool CheckGameIsOver(){
        var avatars = GameObject.FindGameObjectsWithTag("Avatar");
        foreach(GameObject minion in avatars){
            if(minion.GetComponent<MinionCharacter>().currentHealth <= 0){
                return true;
            }
        }
        return false;
    }

    private IEnumerator FinishGame(){
        yield return new WaitForSeconds(10);
        var victoryText = GameObject.FindGameObjectsWithTag("VictoryText");
        victoryText.active = true;
        if(activePlayer == 1){
            victoryText.text = "Knight Warrior won";
        }else{
            victoryText.text = "Demonic Mage won";
        }
    }
}
