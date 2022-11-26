using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaManager : MonoBehaviour
{
    [Range(0, 1000)]
    public int maxMana=100;
    [Range(0, 1000)]
    public int currentMana=100;
    [Range(0, 1000)]
    public int manaRecoveryPerTurn=10;
    private TextMesh text;
    
    private void Start() {
        text=GetComponentInChildren<TextMesh>();
        updateString();
    }
    private void Update() {
        updateString();
        updateColor();
    }
    public void nextTurn(){
        if(currentMana<maxMana){
            currentMana=Mathf.Min(currentMana+manaRecoveryPerTurn,maxMana);
        }
    }
    public void useCard(int manaCost){
        if(currentMana<manaCost) 
            Debug.LogError("Shouldn't be using more mana than you have");
        else
            currentMana-=manaCost;
        
    }
    private void updateString(){
        text.text=currentMana+"/"+maxMana;
    }
    private void updateColor(){
        
        if(maxMana>0 && currentMana>0){
            float r=255-255*currentMana/maxMana;
            float g=Color.cyan.g*currentMana/maxMana;
            float b=Color.cyan.b*currentMana/maxMana;

            //Cyan=rgb(0,188,227)
            text.color=new Color(r,g,b,255);
        }
        
    }
}
