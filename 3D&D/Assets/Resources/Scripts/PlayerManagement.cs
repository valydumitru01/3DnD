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
        
    }

    public void ChangePlayer(){
        if(activePlayer == 1){
            activePlayer = 2;
        }else{
            activePlayer = 1;
        }
    }

    public enum PLAYER
    {
        NONE,
        MAGE,
        KNIGHT
    }
}
