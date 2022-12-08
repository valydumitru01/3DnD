using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionPlay : MonoBehaviour,MenuOption
{
    public void Execute()
    {
        GameObject orb=GameObject.FindGameObjectWithTag("Orbit");
        orb.GetComponent<AutoRotate>().deactivate();
        

        StartCoroutine(BattleRecursive());
    }

    // every 2 seconds perform the print()
   

    public IEnumerator BattleRecursive()
    {

        yield return new WaitForSeconds(0.1f);

        GameObject music = GameObject.FindGameObjectWithTag("MusicPlayer");
        music.GetComponent<AudioSource>().Stop();
        music = GameObject.FindGameObjectWithTag("MusicPlayerGame");
        music.GetComponent<AudioSource>().Play();

    }
}
