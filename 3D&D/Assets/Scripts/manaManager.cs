using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaManager : MonoBehaviour
{
    public int maxMana=100;
    public int currentMana=100;
    public int manaRecoveryPerTurn=10;
    private TextMesh text;
    
    private void Start() {
        text=GetComponentInChildren<TextMesh>();
        updateString();
    }
    public void nextTurn(){
        if(currentMana<maxMana){
            currentMana=Mathf.Min(currentMana+manaRecoveryPerTurn,maxMana);
        }
        updateString();
    }
    public void useCard(int manaCost){
        if(currentMana<manaCost) 
            Debug.LogError("Shouldn't be using more mana than you have");
        else
            currentMana-=manaCost;
        
        updateString();
    }
    private void updateString(){
        text.text=currentMana+"/"+maxMana;
    }
}
