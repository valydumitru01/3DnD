using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownPosition : MonoBehaviour
{
    public PLAYER sitPlayer = PLAYER.NONE;
    public bool isOccupied = false;
    public enum PLAYER
    {
        NONE,
        MAGE,
        KNIGHT
    }
    public PLAYER getSitCharacter()
    {
        return sitPlayer;   
    }
}
