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
        var avatars = GameObject.FindGameObjectsWithTag("Avatar");
        foreach(GameObject minion in avatars){
            if(minion.GetComponent<MinionCharacter>().currentHealth <= 0){
                StartCoroutine(FinishGame());
            }
        }
    }

    public void ChangePlayer(){
        if(activePlayer == 1){
            activePlayer = 2;
        }else{
            activePlayer = 1;
        }
    }

    private IEnumerator FinishGame(){
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
}
