using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownPosition : MonoBehaviour
{
    private int sitPlayer=0;
    public void setSitCharacter(int player)
    {
        sitPlayer = player; 
    }
    public int getSitCharacter()
    {
        return sitPlayer;   
    }
}
